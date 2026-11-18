using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ProjetoBandejao.Models;

namespace ProjetoBandejao.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8080"),
                Timeout = TimeSpan.FromSeconds(5)
            };
        }

        public async Task<List<Usuario>> ObterUsuarios()
        {
            try
            {
                var usuarios = await _httpClient.GetFromJsonAsync<List<Usuario>>("/user");
                return usuarios ?? new List<Usuario>();
            }
            catch
            {
                return new List<Usuario>();
            }
        }

        public async Task<List<Prato>?> ObterPratos()
        {
            try
            {
                var pratos = await _httpClient.GetFromJsonAsync<List<Prato>>("/pratos");
                return pratos;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Aviso>?> ObterAvisos()
        {
            try
            {
                var avisos = await _httpClient.GetFromJsonAsync<List<Aviso>>("/avisos");
                return avisos;
            }
            catch
            {
                return null;
            }
        }
    }
}
