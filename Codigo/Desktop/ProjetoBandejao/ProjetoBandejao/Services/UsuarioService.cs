using ProjetoBandejao.Data;
using ProjetoBandejao.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace ProjetoBandejao.Services
{
    public class UsuarioService
    {
        private readonly HttpClient client = new HttpClient();

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
                HttpResponseMessage response = client.GetAsync($"{API_URL}/all").Result;
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
                HttpResponseMessage response = client.DeleteAsync($"{API_URL}/deletar/{id}").Result;
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

                HttpResponseMessage response = client.PutAsync($"{API_URL}/atualizar", content).Result;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Validar Login
        public bool ValidarLogin(Usuario usuario, out string mensagemErro)
        {
            mensagemErro = string.Empty;

            try
            {
                string url =
                    $"{API_URL}/validarFunc" +
                    $"?login={Uri.EscapeDataString(usuario.Login)}" +
                    $"&senhaHash={Uri.EscapeDataString(usuario.Senha)}";

                HttpResponseMessage response =
                    client.GetAsync(url).Result;

                if (!response.IsSuccessStatusCode)
                {
                    mensagemErro =
                        response.Content.ReadAsStringAsync().Result;

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

        // Solicitar Reset Senha
        public bool SolicitarResetSenha(string email)
        {
            try
            {
                string url = $"{API_URL}/solicitarResetSenha?login={Uri.EscapeDataString(email)}";

                HttpResponseMessage response = client.PostAsync(url, null).Result;

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // Resetar Senha
        public bool ResetarSenha(string email, string codigo, string novaSenha)
        {
            try
            {
                // Limpa espaços ou traços do código
                string codigoLimpo = codigo.Replace(" ", "").Replace("_", "").Trim();

                string url = $"{API_URL}/resetSenha" +
                    $"?login={Uri.EscapeDataString(email)}" +
                    $"&codigo={Uri.EscapeDataString(codigoLimpo)}" +
                    $"&novaSenha={Uri.EscapeDataString(novaSenha)}";

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url);

                HttpResponseMessage response = client.SendAsync(request).Result;

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // Confirmar Email
        public bool ConfirmarEmail(string email, string codigo, out string respostaErro)
        {
            respostaErro = string.Empty;
            try
            {
                // Limpa espaços ou traços do código
                string codigoLimpo = codigo.Replace(" ", "").Replace("_", "").Trim();

                string url = $"{API_URL}/confirmarEmail?email={Uri.EscapeDataString(email.Trim())}&codigo={Uri.EscapeDataString(codigoLimpo)}";

                HttpResponseMessage response = client.PostAsync(url, null).Result;

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

        // Cadastrar com retorno de mensagem de erro detalhada
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

                HttpResponseMessage response = client.PostAsync($"{API_URL}/cadastrar", content).Result;

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