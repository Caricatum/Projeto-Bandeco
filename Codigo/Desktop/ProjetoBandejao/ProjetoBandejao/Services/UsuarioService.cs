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

        // Listar Todos os Usuários (REST: GET /user)
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

        // Deletar Usuário (REST: DELETE /user/{id})
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

        // Atualizar Usuário (REST: PUT /user/{id})
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

        // Validar Login (REST Seguro: POST /user/login-funcionario com JSON no corpo)
        public bool ValidarLogin(Usuario usuario, out string mensagemErro)
        {
            mensagemErro = string.Empty;

            try
            {
                var credenciais = new
                {
                    login = usuario.Login,
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

                return true;
            }
            catch (Exception ex)
            {
                mensagemErro = ex.Message;
                return false;
            }
        }

        // Solicitar Reset Senha (REST: POST /user/solicitarResetSenha com JSON body)
        public bool SolicitarResetSenha(string email)
        {
            try
            {
                var payload = new { login = email };
                string json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync($"{API_URL}/solicitarResetSenha", content).Result;

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // Resetar Senha (REST: POST /user/resetSenha com JSON body)
        public bool ResetarSenha(string email, string codigo, string novaSenha)
        {
            try
            {
                string codigoLimpo = codigo.Replace(" ", "").Replace("_", "").Trim();

                var payload = new
                {
                    login = email,
                    codigo = codigoLimpo,
                    novaSenha = novaSenha
                };

                string json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync($"{API_URL}/resetSenha", content).Result;

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
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
