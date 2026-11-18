using ProjetoBandejao.Data;
using ProjetoBandejao.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace ProjetoBandejao.Services
{
    public class PratoService
    {
        private readonly HttpClient client = new HttpClient { Timeout = TimeSpan.FromSeconds(3) };
        
        private const string API_URL = "http://localhost:8080/pratos";
        private const string CATEGORIA_URL = "http://localhost:8080/categoria";
        private const string NUTRI_URL = "http://localhost:8080/valorNutricional";

        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<List<Prato>> ListarAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(API_URL);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var pratos = JsonSerializer.Deserialize<List<Prato>>(json, jsonOptions);
                    if (pratos != null)
                    {
                        SQLiteDatabase.SalvarPratosApi(pratos);
                        return SQLiteDatabase.ObterPratos();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PratoService.ListarAsync] API inacessível ({ex.Message}). Utilizando banco local SQLite.");
            }

            return SQLiteDatabase.ObterPratos();
        }

        public List<Prato> Listar()
        {
            try
            {
                HttpResponseMessage response = client.GetAsync(API_URL).Result;
                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    var pratos = JsonSerializer.Deserialize<List<Prato>>(json, jsonOptions);
                    if (pratos != null)
                    {
                        SQLiteDatabase.SalvarPratosApi(pratos);
                        return SQLiteDatabase.ObterPratos();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PratoService.Listar] API inacessível ({ex.Message}). Utilizando banco local SQLite.");
            }

            // Retorna a lista unificada do SQLite (inclui registros da API + registros criados offline)
            var listaLocal = SQLiteDatabase.ObterPratos();
            return listaLocal;
        }

        public List<Prato> BuscarPorNome(string nome)
        {
            var todos = Listar();
            if (string.IsNullOrWhiteSpace(nome))
                return todos;

            return todos.FindAll(p => p.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));
        }

        public List<Categoria> ListarCategorias()
        {
            try
            {
                HttpResponseMessage response = client.GetAsync(CATEGORIA_URL).Result;
                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    var categorias = JsonSerializer.Deserialize<List<Categoria>>(json, jsonOptions);
                    if (categorias != null && categorias.Count > 0)
                        return categorias;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PratoService.ListarCategorias] Erro: {ex.Message}");
            }

            // Fallback de categorias padrão
            return new List<Categoria>
            {
                new Categoria { Id = 1, Descricao = "Carnes" },
                new Categoria { Id = 2, Descricao = "Vegetariano" },
                new Categoria { Id = 3, Descricao = "Guarnição" },
                new Categoria { Id = 4, Descricao = "Salada" },
                new Categoria { Id = 5, Descricao = "Sobremesa" }
            };
        }

        public bool Cadastrar(Prato prato, string? imageFilePath = null)
        {
            try
            {
                using var multipartContent = new MultipartFormDataContent();

                // Monta o payload do prato para o Spring Boot
                string pratoJson = MontarDadosPrato(prato).ToJsonString();
                var pratoContent = new StringContent(pratoJson, Encoding.UTF8, "application/json");
                pratoContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                multipartContent.Add(pratoContent, "prato");

                // Anexa o arquivo de imagem caso tenha sido selecionado
                if (!string.IsNullOrEmpty(imageFilePath) && File.Exists(imageFilePath))
                {
                    byte[] fileBytes = File.ReadAllBytes(imageFilePath);
                    var fileContent = new ByteArrayContent(fileBytes);
                    
                    string ext = Path.GetExtension(imageFilePath).ToLower();
                    string mime = ext switch
                    {
                        ".png" => "image/png",
                        ".jpg" or ".jpeg" => "image/jpeg",
                        ".webp" => "image/webp",
                        _ => "application/octet-stream"
                    };
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(mime);
                    multipartContent.Add(fileContent, "imagem", Path.GetFileName(imageFilePath));
                }

                HttpResponseMessage response = client.PostAsync(API_URL, multipartContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    try
                    {
                        var pratoCriado = JsonSerializer.Deserialize<Prato>(responseBody, jsonOptions);
                        if (pratoCriado != null && pratoCriado.Id > 0)
                        {
                            if (TemValoresNutricionais(prato))
                            {
                                prato.Id = pratoCriado.Id;
                                prato.Imagem = pratoCriado.Imagem;
                                int pendenteId = SQLiteDatabase.SalvarPratoLocal(prato, imageFilePath, pendenteEnvio: true);
                                if (pendenteId > 0)
                                    SQLiteDatabase.RegistrarEnvioPrato(pendenteId, pratoCriado.Id, pendente: true);
                                else
                                    MessageBox.Show("O prato foi cadastrado, mas os valores nutricionais não puderam ser guardados para envio.", "Cadastro de prato");
                            }
                            else
                            {
                                SQLiteDatabase.SalvarPratosApi(new List<Prato> { pratoCriado }, listaCompleta: false);
                            }
                        }
                    }
                    catch
                    {
                        // Falha não crítica na gravação local
                    }

                    return true;
                }

                string errorMsg = response.Content.ReadAsStringAsync().Result;
                MessageBox.Show($"Erro retornado pela API ({response.StatusCode}):\n{errorMsg}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            catch (Exception ex)
            {
                // API inacessível -> Salva localmente no SQLite como Pendente de Sincronização
                Console.WriteLine($"[PratoService.Cadastrar] API offline ({ex.Message}). Salvando prato no SQLite com pendência de envio.");
                int localId = SQLiteDatabase.SalvarPratoLocal(prato, imageFilePath, pendenteEnvio: true);

                if (localId > 0)
                {
                    MessageBox.Show(
                        "O servidor da API não respondeu no momento.\n\n" +
                        $"O prato '{prato.Nome}' foi salvo com sucesso no banco de dados local SQLite!\n" +
                        "Ele será sincronizado automaticamente com a API assim que a conexão for reestabelecida.",
                        "Salvo em Modo Offline",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Não foi possível salvar o prato nem na API nem no banco local.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        internal static bool TemValoresNutricionais(Prato prato) =>
            prato.Calorias > 0 || prato.Proteinas > 0 || prato.Carboidratos > 0 || prato.Gorduras > 0;

        internal static System.Text.Json.Nodes.JsonObject MontarDadosPrato(Prato prato,
            System.Text.Json.Nodes.JsonObject? dados = null)
        {
            dados ??= new System.Text.Json.Nodes.JsonObject
            {
                ["notaTecnica"] = prato.NotaTecnica,
                ["descricaoIA"] = prato.DescricaoIA,
                ["categoria"] = new System.Text.Json.Nodes.JsonObject { ["id"] = 1 }
            };
            dados["nome"] = prato.Nome;
            dados["descricao"] = string.IsNullOrWhiteSpace(prato.Descricao) ? "Sem descrição" : prato.Descricao;
            dados["vegano"] = prato.Vegano;
            if (prato.Categoria is { Id: > 0 } categoria)
                dados["categoria"] = new System.Text.Json.Nodes.JsonObject { ["id"] = categoria.Id };
            for (int i = 0; i < Prato.CamposAlergenicos.Count; i++)
                dados[Prato.CamposAlergenicos[i].Campo] = prato.TemAlergenico(i);
            return dados;
        }

        internal static async Task EnviarValorNutricionalAsync(HttpClient cliente, int pratoId, Prato prato)
        {
            var remoto = await cliente.GetFromJsonAsync<Prato>($"{API_URL}/{pratoId}")
                ?? throw new JsonException("A API não retornou o prato cadastrado.");
            var atual = remoto.ValorNutricional;
            if (atual == null && !TemValoresNutricionais(prato))
                return;
            var valores = new
            {
                kcal = (float)prato.Calorias,
                carboidratos = (float)prato.Carboidratos,
                proteinas = (float)prato.Proteinas,
                lipidios = (float)prato.Gorduras,
                medida = "100g",
                prato = new { id = pratoId }
            };
            if (atual != null && atual.Kcal == valores.kcal && atual.Carboidratos == valores.carboidratos
                && atual.Proteinas == valores.proteinas && atual.Lipidios == valores.lipidios
                && atual.Medida == valores.medida)
                return;

            using var resposta = atual?.Id > 0
                ? await cliente.PutAsJsonAsync($"{NUTRI_URL}/{atual.Id}", valores)
                : await cliente.PostAsJsonAsync(NUTRI_URL, valores);
            resposta.EnsureSuccessStatusCode();
        }

        public async Task<string?> AtualizarAsync(Prato prato, string? caminhoImagem)
        {
            if (prato.Id < 0)
                return SQLiteDatabase.AtualizarPratoPendente(prato, caminhoImagem) ? null : "O prato não está mais pendente neste computador.";
            try
            {
                var dados = await client.GetFromJsonAsync<System.Text.Json.Nodes.JsonObject>($"{API_URL}/{prato.Id}");
                if (dados == null) return "A API não retornou os dados atuais do prato.";
                var anterior = dados["valorNutricional"]?.Deserialize<ValorNutricionalModel>(jsonOptions);
                bool nutricaoMudou = (float)prato.Calorias != (anterior?.Kcal ?? 0)
                    || (float)prato.Proteinas != (anterior?.Proteinas ?? 0)
                    || (float)prato.Carboidratos != (anterior?.Carboidratos ?? 0)
                    || (float)prato.Gorduras != (anterior?.Lipidios ?? 0);
                MontarDadosPrato(prato, dados);
                dados.Remove("valorNutricional");
                using var multipart = new MultipartFormDataContent();
                multipart.Add(new StringContent(dados.ToJsonString(), Encoding.UTF8, "application/json"), "prato");
                if (caminhoImagem != null)
                {
                    var arquivo = new StreamContent(File.OpenRead(caminhoImagem));
                    arquivo.Headers.ContentType = new MediaTypeHeaderValue(Path.GetExtension(caminhoImagem).Equals(".png", StringComparison.OrdinalIgnoreCase)
                        ? "image/png" : "image/jpeg");
                    multipart.Add(arquivo, "imagem", Path.GetFileName(caminhoImagem));
                }
                using var resposta = await client.PutAsync($"{API_URL}/{prato.Id}", multipart);
                string corpo = await resposta.Content.ReadAsStringAsync();
                if (!resposta.IsSuccessStatusCode)
                    return UsuarioService.ExtrairMensagemErro(corpo, $"A API recusou a edição (HTTP {(int)resposta.StatusCode}).");
                var salvo = JsonSerializer.Deserialize<Prato>(corpo, jsonOptions);
                if (salvo != null)
                {
                    if (nutricaoMudou)
                    {
                        salvo.ValorNutricional = null;
                        salvo.Calorias = prato.Calorias;
                        salvo.Proteinas = prato.Proteinas;
                        salvo.Carboidratos = prato.Carboidratos;
                        salvo.Gorduras = prato.Gorduras;
                    }
                    SQLiteDatabase.SalvarPratosApi([salvo], listaCompleta: false, nutricaoPendente: nutricaoMudou);
                }
                return null;
            }
            catch (Exception ex) when (ex is HttpRequestException or IOException or OperationCanceledException or JsonException)
            {
                return "Não foi possível editar o prato. Confira a conexão com a API e a imagem selecionada.";
            }
        }

        public bool Deletar(int id)
        {
            try
            {
                if (id < 0)
                    return SQLiteDatabase.DeletarPrato(id);

                using var response = client.DeleteAsync($"{API_URL}/{id}").GetAwaiter().GetResult();
                return response.IsSuccessStatusCode && SQLiteDatabase.DeletarPrato(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao excluir prato: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
