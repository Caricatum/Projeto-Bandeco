using ProjetoBandejao.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace ProjetoBandejao.Services
{
    public class PratoService
    {
        private readonly HttpClient client = new HttpClient();
        
        // Assumindo que a API de pratos fica no endpoint /prato ou /pratos
        private const string API_URL = "http://localhost:8080/prato";

        public bool Cadastrar(Prato prato)
        {
            try
            {
                string json = JsonSerializer.Serialize(prato);

                StringContent content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                // Utilizando a mesma convenção do UsuarioService (chamada sincrona .Result)
                HttpResponseMessage response = client.PostAsync($"{API_URL}/cadastrar", content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    string errorMsg = response.Content.ReadAsStringAsync().Result;
                    MessageBox.Show($"Erro na API ({response.StatusCode}): {errorMsg}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Falha de conexão com a API: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
