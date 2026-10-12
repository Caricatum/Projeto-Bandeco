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
                BaseAddress = new Uri("http://localhost:8080")
            };
        }

        public async Task<List<Usuario>> ObterUsuarios()
        {
            var usuarios = await _httpClient.GetFromJsonAsync<List<Usuario>>("/user/all");

            return usuarios ?? new List<Usuario>();
        }
    }
}