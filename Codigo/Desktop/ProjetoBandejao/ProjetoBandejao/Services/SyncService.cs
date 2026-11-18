using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ProjetoBandejao.Data;
using ProjetoBandejao.Models;

namespace ProjetoBandejao.Services
{
    public static class SyncService
    {
        private static readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(3) };
        private const string BASE_URL = "http://localhost:8080";

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private static CancellationTokenSource? _cts;
        private static int _isSyncing;

        public static bool IsOnline { get; private set; } = false;

        public static event Action<bool>? OnStatusChanged;
        public static event Action<string>? OnSyncCompleted;

        public static void Iniciar(int intervaloSegundos = 15)
        {
            if (_cts != null) return;

            _cts = new CancellationTokenSource();
            Task.Run(() => LoopSincronizacao(_cts.Token, intervaloSegundos));
        }

        public static void Parar()
        {
            _cts?.Cancel();
            _cts = null;
        }

        private static async Task LoopSincronizacao(CancellationToken token, int intervaloSegundos)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await VerificarESincronizarAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[SyncService] Erro no loop de sincronização: {ex.Message}");
                }

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(intervaloSegundos), token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        public static async Task VerificarESincronizarAsync()
        {
            if (Interlocked.CompareExchange(ref _isSyncing, 1, 0) != 0) return;
            try
            {
                bool onlineAntes = IsOnline;
                bool onlineAgora = await TestarConexaoApiAsync();

                IsOnline = onlineAgora;
                UsuarioSession.IsModoOffline = !onlineAgora;
                if (onlineAntes != onlineAgora)
                    OnStatusChanged?.Invoke(onlineAgora);

                if (onlineAgora)
                {
                    await SincronizarDadosPendentesAsync();
                    await AtualizarCacheLocalAsync();
                }
            }
            finally
            {
                Interlocked.Exchange(ref _isSyncing, 0);
            }
        }

        public static async Task<bool> TestarConexaoApiAsync()
        {
            try
            {
                using var response = await _httpClient.GetAsync($"{BASE_URL}/pratos");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                try
                {
                    using var responseCat = await _httpClient.GetAsync($"{BASE_URL}/categoria");
                    return responseCat.IsSuccessStatusCode;
                }
                catch
                {
                    try
                    {
                        using var responseUser = await _httpClient.GetAsync($"{BASE_URL}/user");
                        return responseUser.IsSuccessStatusCode;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }

        private static async Task SincronizarDadosPendentesAsync()
        {
            int sincronizados = 0;

            // 1. Sincroniza Pratos Pendentes
            var pratosPendentes = SQLiteDatabase.ObterPratosPendentes();
            foreach (var item in pratosPendentes)
            {
                try
                {
                    int apiId = item.Prato.Id;
                    if (apiId <= 0)
                    {
                        using var multipartContent = new MultipartFormDataContent();

                        string pratoJson = PratoService.MontarDadosPrato(item.Prato).ToJsonString();
                        var pratoContent = new StringContent(pratoJson, Encoding.UTF8, "application/json");
                        pratoContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        multipartContent.Add(pratoContent, "prato");

                        if (!string.IsNullOrEmpty(item.LocalImagePath) && File.Exists(item.LocalImagePath))
                        {
                            byte[] fileBytes = await File.ReadAllBytesAsync(item.LocalImagePath);
                            var fileContent = new ByteArrayContent(fileBytes);
                            string ext = Path.GetExtension(item.LocalImagePath).ToLower();
                            string mime = ext switch
                            {
                                ".png" => "image/png",
                                ".jpg" or ".jpeg" => "image/jpeg",
                                ".webp" => "image/webp",
                                _ => "application/octet-stream"
                            };
                            fileContent.Headers.ContentType = new MediaTypeHeaderValue(mime);
                            multipartContent.Add(fileContent, "imagem", Path.GetFileName(item.LocalImagePath));
                        }

                        using var resposta = await _httpClient.PostAsync($"{BASE_URL}/pratos", multipartContent);
                        resposta.EnsureSuccessStatusCode();
                        string json = await resposta.Content.ReadAsStringAsync();
                        var cadastrado = JsonSerializer.Deserialize<Prato>(json, _jsonOptions);
                        if (cadastrado == null || cadastrado.Id <= 0)
                            throw new JsonException("A API não retornou o ID do prato cadastrado.");

                        apiId = cadastrado.Id;
                        if (!SQLiteDatabase.RegistrarEnvioPrato(item.LocalId, apiId, pendente: true, imagemUrl: cadastrado.Imagem))
                            continue;
                    }

                    await PratoService.EnviarValorNutricionalAsync(_httpClient, apiId, item.Prato);
                    if (SQLiteDatabase.RegistrarEnvioPrato(item.LocalId, apiId, pendente: false))
                        sincronizados++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[SyncService] Erro ao sincronizar prato local {item.LocalId}: {ex.Message}");
                }
            }

            // 2. Sincroniza Avisos Pendentes
            var avisosPendentes = SQLiteDatabase.ObterAvisosPendentes();
            foreach (var item in avisosPendentes)
            {
                try
                {
                    int userId = item.Aviso.User?.Id ?? (UsuarioSession.UsuarioLogado?.Id ?? 1);

                    var payload = new
                    {
                        titulo = item.Aviso.Titulo,
                        descricao = item.Aviso.Descricao,
                        model = string.IsNullOrWhiteSpace(item.Aviso.Model) ? "Mural" : item.Aviso.Model,
                        data_inicial = (item.Aviso.DataInicial ?? DateTime.Today).ToString("yyyy-MM-dd"),
                        data_final = (item.Aviso.DataFinal ?? DateTime.Today.AddDays(15)).ToString("yyyy-MM-dd"),
                        user = new { id = userId }
                    };

                    string json = JsonSerializer.Serialize(payload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var resp = await _httpClient.PostAsync($"{BASE_URL}/avisos", content);
                    if (resp.IsSuccessStatusCode)
                    {
                        string body = await resp.Content.ReadAsStringAsync();
                        var avisoCriado = JsonSerializer.Deserialize<Aviso>(body, _jsonOptions);
                        if (avisoCriado != null && avisoCriado.Id > 0)
                        {
                            SQLiteDatabase.MarcarAvisoSincronizado(item.LocalId, avisoCriado.Id);
                            sincronizados++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[SyncService] Erro ao sincronizar aviso local {item.LocalId}: {ex.Message}");
                }
            }

            // 3. Sincroniza Funcionários Pendentes
            var funcsPendentes = SQLiteDatabase.ObterFuncionariosPendentes();
            foreach (var func in funcsPendentes)
            {
                try
                {
                    using var multipartContent = new MultipartFormDataContent();
                    var dados = new
                    {
                        nome = func.Nome,
                        login = func.Login,
                        senhaHash = func.Senha,
                        funcionario = func.Funcionario
                    };

                    string json = JsonSerializer.Serialize(dados);
                    var userContent = new StringContent(json, Encoding.UTF8, "application/json");
                    userContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    multipartContent.Add(userContent, "user");

                    var resp = await _httpClient.PostAsync($"{BASE_URL}/user", multipartContent);
                    if (resp.IsSuccessStatusCode)
                    {
                        string body = await resp.Content.ReadAsStringAsync();
                        var userCriado = JsonSerializer.Deserialize<Usuario>(body, _jsonOptions);
                        if (userCriado != null && userCriado.Id > 0)
                        {
                            SQLiteDatabase.MarcarFuncionarioSincronizado(func.Login, userCriado.Id);
                            sincronizados++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[SyncService] Erro ao sincronizar funcionário local {func.Login}: {ex.Message}");
                }
            }

            if (sincronizados > 0)
            {
                OnSyncCompleted?.Invoke($"{sincronizados} item(ns) sincronizado(s) com a API com sucesso.");
            }
        }

        private static async Task AtualizarCacheLocalAsync()
        {
            try
            {
                var api = new ApiService();
                var usuarios = await api.ObterUsuarios();
                if (usuarios != null && usuarios.Count > 0)
                {
                    SQLiteDatabase.SalvarFuncionarios(usuarios);
                }

                var pratos = await api.ObterPratos();
                if (pratos != null)
                {
                    SQLiteDatabase.SalvarPratosApi(pratos);
                }

                var avisos = await api.ObterAvisos();
                if (avisos != null)
                {
                    SQLiteDatabase.SalvarAvisosApi(avisos);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SyncService] Erro ao atualizar cache local da API: {ex.Message}");
            }
        }
    }
}
