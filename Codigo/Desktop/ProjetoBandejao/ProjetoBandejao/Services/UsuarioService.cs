using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using ProjetoBandejao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
namespace ProjetoBandejao.Services
{
    public class UsuarioService
    {
        private readonly HttpClient client = new HttpClient();

        private const string API_URL =
            "http://localhost:8080/user";

        //ValidarMetodo

        public bool ValidarLogin(Usuario usuario)
        {
            try
            {
                string url =
                    $"{API_URL}/validarFunc?login={usuario.Login}&senhaHash={usuario.Senha}";

                HttpResponseMessage response =
                    client.GetAsync(url).Result;

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        //SolicitarResetSenhaMetodo
        public bool SolicitarResetSenha(string email)
        {
            try
            {
                string url =
                    $"{API_URL}/solicitarResetSenha?login={email}";

                HttpResponseMessage response =
                    client.PostAsync(url, null).Result;

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        //ResetarSenhaMetodo
        public bool ResetarSenha(
            string email,
            string codigo,
            string novaSenha)
        {
            try
            {
                string url =
                    $"{API_URL}/resetSenha" +
                    $"?login={email}" +
                    $"&codigo={codigo}" +
                    $"&novaSenha={novaSenha}";

                HttpRequestMessage request =
                    new HttpRequestMessage(
                        HttpMethod.Put,
                        url);

                HttpResponseMessage response =
                    client.SendAsync(request).Result;

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        //ConfirmarEmailMetodo

        public bool ConfirmarEmail(
    string email,
    string codigo)
        {
            try
            {
                string url =
                    $"{API_URL}/confirmarEmail?email={email}&codigo={codigo}";

                HttpResponseMessage response =
                    client.PostAsync(url, null).Result;

                string resposta =
                    response.Content.ReadAsStringAsync().Result;

                MessageBox.Show(
                    $"Status: {(int)response.StatusCode}\n\n{resposta}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //CadastrarMetodo

        public bool Cadastrar(Usuario usuario)
        {
            try
            {
                var dados = new
                {
                    nome = usuario.Nome,
                    login = usuario.Login,
                    senhaHash = usuario.Senha,
                    funcionario = usuario.Funcionario
                };

                string json =
                    JsonSerializer.Serialize(dados);

                StringContent content =
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json");

                HttpResponseMessage response =
                    client.PostAsync(
                        $"{API_URL}/cadastrar",
                        content).Result;

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}