using ProjetoBandejao.Data;
using ProjetoBandejao.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace ProjetoBandejao.Services
{
    public class UsuarioService
    {
        private readonly HttpClient client = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };

        private const string API_URL = "http://localhost:8080/user";

        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Listar Todos os Usuários (Sincronizado com SQLite)
        public List<Usuario> ListarTodos()
        {
            try
            {
                HttpResponseMessage response = client.GetAsync(API_URL).Result;
                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    var usuarios = JsonSerializer.Deserialize<List<Usuario>>(json, jsonOptions);
                    if (usuarios != null && usuarios.Count > 0)
                    {
                        // Sincroniza com o banco local SQLite
                        SQLiteDatabase.SalvarFuncionarios(usuarios);
                        return usuarios;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UsuarioService.ListarTodos] Falha na API ({ex.Message}). Carregando do banco local SQLite...");
            }

            // Fallback: carrega do banco local SQLite se a API estiver inacessível
            return SQLiteDatabase.ObterFuncionarios();
        }

        // Deletar Usuário (Remove da API e do banco local)
        public bool Deletar(int id)
        {
            try
            {
                HttpResponseMessage response = client.DeleteAsync($"{API_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    SQLiteDatabase.DeletarFuncionario(id);
                    return true;
                }

                string msg = response.Content.ReadAsStringAsync().Result;
                MessageBox.Show($"Erro ao excluir usuário na API ({response.StatusCode}):\n{msg}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao excluir usuário: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Atualizar Usuário
        public bool Atualizar(Usuario usuario)
        {
            try
            {
                var dados = new
                {
                    id = usuario.Id,
                    nome = usuario.Nome,
                    login = usuario.Login,
                    funcionario = usuario.Funcionario
                };

                string json = JsonSerializer.Serialize(dados);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PutAsync($"{API_URL}/{usuario.Id}", content).Result;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Validar Login (com contingência para Modo Offline caso o servidor esteja inacessível)
        public bool ValidarLogin(Usuario usuario, out string mensagemErro, out bool isOffline)
        {
            mensagemErro = string.Empty;
            isOffline = false;

            try
            {
                var credenciais = new
                {
                    login = usuario.Login.Trim(),
                    senha = usuario.Senha
                };

                string json = JsonSerializer.Serialize(credenciais);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync($"{API_URL}/login-funcionario", content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    mensagemErro = response.Content.ReadAsStringAsync().Result;
                    return false;
                }

                isOffline = false;
                return true;
            }
            catch (Exception)
            {
                // API inacessível - Tenta autenticar pelo banco local SQLite em contingência
                var userLocal = SQLiteDatabase.ObterFuncionarios()
                    .Find(u => string.Equals(u.Login, usuario.Login.Trim(), StringComparison.OrdinalIgnoreCase));

                if (userLocal != null)
                {
                    isOffline = true;
                    mensagemErro = "Servidor offline. Acesso liberado pelo banco local (Modo Offline).";
                    return true;
                }

                mensagemErro = "Servidor da API inacessível e o usuário não foi localizado no banco local SQLite.";
                return false;
            }
        }

        public bool ValidarLogin(Usuario usuario, out string mensagemErro)
        {
            return ValidarLogin(usuario, out mensagemErro, out _);
        }

        // Solicitar Reset Senha (REST: POST /user/solicitarResetSenha com JSON body)
        public bool SolicitarResetSenha(string email, out string mensagemErro)
        {
            mensagemErro = string.Empty;
            try
            {
                var payload = new { login = email.Trim() };
                string json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync($"{API_URL}/solicitarResetSenha", content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        mensagemErro = "Nenhum usuário cadastrado com este e-mail.";
                    }
                    else
                    {
                        string body = response.Content.ReadAsStringAsync().Result;
                        mensagemErro = string.IsNullOrWhiteSpace(body) ? $"Erro na solicitação ({response.StatusCode})." : body;
                    }
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                mensagemErro = $"Não foi possível conectar à API: {ex.Message}";
                return false;
            }
        }

        public bool SolicitarResetSenha(string email)
        {
            return SolicitarResetSenha(email, out _);
        }

        // Resetar Senha (REST: POST /user/resetSenha com JSON body)
        public bool ResetarSenha(string email, string codigo, string novaSenha, out string mensagemErro)
        {
            mensagemErro = string.Empty;
            try
            {
                string codigoLimpo = codigo.Replace(" ", "").Replace("_", "").Trim();

                var payload = new
                {
                    login = email.Trim(),
                    codigo = codigoLimpo,
                    novaSenha = novaSenha
                };

                string json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync($"{API_URL}/resetSenha", content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    string body = response.Content.ReadAsStringAsync().Result;
                    mensagemErro = string.IsNullOrWhiteSpace(body) ? "Código inválido ou expirado." : body;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                mensagemErro = $"Erro de conexão com o servidor: {ex.Message}";
                return false;
            }
        }

        public bool ResetarSenha(string email, string codigo, string novaSenha)
        {
            return ResetarSenha(email, codigo, novaSenha, out _);
        }

        // Confirmar Email (REST: POST /user/confirmarEmail com JSON body)
        public bool ConfirmarEmail(string email, string codigo, out string respostaErro)
        {
            respostaErro = string.Empty;
            try
            {
                string codigoLimpo = codigo.Replace(" ", "").Replace("_", "").Trim();

                var payload = new
                {
                    email = email.Trim(),
                    codigo = codigoLimpo
                };

                string json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync($"{API_URL}/confirmarEmail", content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    respostaErro = response.Content.ReadAsStringAsync().Result;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                respostaErro = ex.Message;
                return false;
            }
        }

        // Obter Usuário por Login
        public Usuario? ObterPorLogin(string login)
        {
            try
            {
                string url = $"{API_URL}/login/{Uri.EscapeDataString(login.Trim())}";
                HttpResponseMessage res = client.GetAsync(url).Result;
                if (res.IsSuccessStatusCode)
                {
                    string json = res.Content.ReadAsStringAsync().Result;
                    return JsonSerializer.Deserialize<Usuario>(json, jsonOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UsuarioService.ObterPorLogin] Erro: {ex.Message}");
            }

            return SQLiteDatabase.ObterFuncionarios().Find(u => string.Equals(u.Login, login, StringComparison.OrdinalIgnoreCase));
        }

        // Reenviar Código de Cadastro
        public bool ReenviarCodigoCadastro(string email, out string mensagemRetorno)
        {
            mensagemRetorno = string.Empty;
            try
            {
                var usuario = ObterPorLogin(email);
                if (usuario == null || usuario.Id <= 0)
                {
                    mensagemRetorno = "Usuário não localizado na base de dados.";
                    return false;
                }

                string urlReenvio = $"{API_URL}/reenviarCodigoCadastro?id={usuario.Id}";
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, urlReenvio);
                HttpResponseMessage resReenvio = client.SendAsync(request).Result;

                if (resReenvio.IsSuccessStatusCode)
                {
                    mensagemRetorno = "Novo código enviado para o seu e-mail.";
                    return true;
                }

                mensagemRetorno = resReenvio.Content.ReadAsStringAsync().Result;
                return false;
            }
            catch (Exception ex)
            {
                mensagemRetorno = ex.Message;
                return false;
            }
        }

        // Cadastrar (REST: POST /user)
        public bool Cadastrar(Usuario usuario, out string mensagemErro)
        {
            mensagemErro = string.Empty;
            try
            {
                var dados = new
                {
                    nome = usuario.Nome,
                    login = usuario.Login,
                    senhaHash = usuario.Senha,
                    funcionario = usuario.Funcionario
                };

                string json = JsonSerializer.Serialize(dados);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync(API_URL, content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    mensagemErro = response.Content.ReadAsStringAsync().Result;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                mensagemErro = ex.Message;
                return false;
            }
        }
    }
}
