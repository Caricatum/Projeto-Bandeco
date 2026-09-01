using ProjetoBandejao.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace ProjetoBandejao.Services
{
    public class PratoService
    {
        private readonly HttpClient client = new HttpClient();
        
        private const string API_URL = "http://localhost:8080/pratos";
        private const string CATEGORIA_URL = "http://localhost:8080/categoria";
        private const string NUTRI_URL = "http://localhost:8080/valorNutricional";

        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public List<Prato> Listar()
        {
            try
            {
                HttpResponseMessage response = client.GetAsync($"{API_URL}/all").Result;
                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    var pratos = JsonSerializer.Deserialize<List<Prato>>(json, jsonOptions);
                    return pratos ?? new List<Prato>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PratoService.Listar] Erro: {ex.Message}");
            }
            return new List<Prato>();
        }

        public List<Prato> BuscarPorNome(string nome)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nome))
                    return Listar();

                string url = $"{API_URL}/nome?nome={Uri.EscapeDataString(nome)}";
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    var pratos = JsonSerializer.Deserialize<List<Prato>>(json, jsonOptions);
                    return pratos ?? new List<Prato>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PratoService.BuscarPorNome] Erro: {ex.Message}");
            }
            return new List<Prato>();
        }

        public List<Categoria> ListarCategorias()
        {
            try
            {
                HttpResponseMessage response = client.GetAsync($"{CATEGORIA_URL}/all").Result;
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
                var pratoPayload = new
                {
                    nome = prato.Nome,
                    descricao = string.IsNullOrWhiteSpace(prato.Descricao) ? "Sem descrição" : prato.Descricao,
                    vegano = prato.Vegano,
                    notaTecnica = prato.NotaTecnica,
                    descricaoIA = prato.DescricaoIA,
                    categoria = prato.Categoria != null && prato.Categoria.Id > 0 
                        ? new { id = prato.Categoria.Id, descricao = prato.Categoria.Descricao }
                        : new { id = 1, descricao = "Carnes" }
                };

                string pratoJson = JsonSerializer.Serialize(pratoPayload);
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

                HttpResponseMessage response = client.PostAsync($"{API_URL}/cadastrar", multipartContent).Result;

                if (!response.IsSuccessStatusCode)
                {
                    string errorMsg = response.Content.ReadAsStringAsync().Result;
                    MessageBox.Show($"Erro ao cadastrar prato na API ({response.StatusCode}):\n{errorMsg}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // Tenta salvar os valores nutricionais se preenchidos
                string responseBody = response.Content.ReadAsStringAsync().Result;
                try
                {
                    var pratoCriado = JsonSerializer.Deserialize<Prato>(responseBody, jsonOptions);
                    if (pratoCriado != null && pratoCriado.Id > 0)
                    {
                        CadastrarValorNutricional(pratoCriado.Id, prato);
                    }
                }
                catch
                {
                    // Falha não crítica se o valor nutricional não for salvo
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Falha de conexão com a API: {ex.Message}", "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void CadastrarValorNutricional(int pratoId, Prato prato)
        {
            if (prato.Calorias <= 0 && prato.Proteinas <= 0 && prato.Carboidratos <= 0 && prato.Gorduras <= 0)
                return;

            try
            {
                var nutriPayload = new
                {
                    kcal = (float)prato.Calorias,
                    carboidratos = (float)prato.Carboidratos,
                    proteinas = (float)prato.Proteinas,
                    lipidios = (float)prato.Gorduras,
                    medida = "100g",
                    prato = new { id = pratoId }
                };

                string json = JsonSerializer.Serialize(nutriPayload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.PostAsync($"{NUTRI_URL}/cadastrar", content).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CadastrarValorNutricional] Erro: {ex.Message}");
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
                MessageBox.Show($"Erro ao excluir prato: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
