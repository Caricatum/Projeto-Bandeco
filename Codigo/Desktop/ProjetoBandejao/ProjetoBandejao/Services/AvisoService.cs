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
                HttpResponseMessage response = client.GetAsync(API_URL).Result;
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
                int userId = 0;
                if (aviso.User != null && aviso.User.Id > 0)
                {
                    userId = aviso.User.Id;
                }
                else if (UsuarioSession.UsuarioLogado != null && UsuarioSession.UsuarioLogado.Id > 0)
                {
                    userId = UsuarioSession.UsuarioLogado.Id;
                }
                else
                {
                    var primeiro = SQLiteDatabase.ObterFuncionarios().Find(u => u.Id > 0);
                    if (primeiro != null)
                    {
                        userId = primeiro.Id;
                    }
                }

                if (userId <= 0)
                {
                    MessageBox.Show("Não foi possível identificar o usuário para publicar o aviso. Efetue o login novamente.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                var payload = new
                {
                    titulo = aviso.Titulo,
                    descricao = aviso.Descricao,
                    model = string.IsNullOrWhiteSpace(aviso.Model) ? "Mural" : aviso.Model,
                    data_inicial = (aviso.DataInicial ?? DateTime.Today).ToString("yyyy-MM-dd"),
                    data_final = (aviso.DataFinal ?? DateTime.Today.AddDays(15)).ToString("yyyy-MM-dd"),
                    user = new { id = userId }
                };

                string json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync(API_URL, content).Result;
                if (!response.IsSuccessStatusCode)
                {
                    string erroMsg = response.Content.ReadAsStringAsync().Result;
                    MessageBox.Show($"Erro ao cadastrar aviso ({response.StatusCode}):\n{erroMsg}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                return true;
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
                HttpResponseMessage response = client.DeleteAsync($"{API_URL}/{id}").Result;
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
