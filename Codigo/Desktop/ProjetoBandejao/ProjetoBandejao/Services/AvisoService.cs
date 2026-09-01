using ProjetoBandejao.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace ProjetoBandejao.Services
{
    public class AvisoService
    {
        private readonly HttpClient client = new HttpClient();
        private const string API_URL = "http://localhost:8080/avisos";

        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public List<Aviso> Listar()
        {
            try
            {
                HttpResponseMessage response = client.GetAsync($"{API_URL}/all").Result;
                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    var avisos = JsonSerializer.Deserialize<List<Aviso>>(json, jsonOptions);
                    return avisos ?? new List<Aviso>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AvisoService.Listar] Erro: {ex.Message}");
            }
            return new List<Aviso>();
        }

        public bool Cadastrar(Aviso aviso)
        {
            try
            {
                var payload = new
                {
                    titulo = aviso.Titulo,
                    descricao = aviso.Descricao,
                    user = aviso.User != null && aviso.User.Id > 0 ? new { id = aviso.User.Id } : null
                };

                string json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync($"{API_URL}/cadastrar", content).Result;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao cadastrar aviso: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool Deletar(int id)
        {
            try
            {
                HttpResponseMessage response = client.DeleteAsync($"{API_URL}/deletar/{id}").Result;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao deletar aviso: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
