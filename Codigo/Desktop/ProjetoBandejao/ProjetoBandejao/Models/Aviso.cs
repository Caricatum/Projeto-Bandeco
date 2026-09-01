using System;
using System.Text.Json.Serialization;

namespace ProjetoBandejao.Models
{
    public class Aviso
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("titulo")]
        public string Titulo { get; set; } = string.Empty;

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; } = string.Empty;

        [JsonPropertyName("dataCriacao")]
        public DateTime? DataCriacao { get; set; }

        [JsonPropertyName("user")]
        public Usuario? User { get; set; }

        // Propriedade auxiliar para exibição
        public string DataFormatada => DataCriacao?.ToString("dd/MM/yyyy HH:mm") ?? DateTime.Now.ToString("dd/MM/yyyy");
    }
}
