using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ProjetoBandejao.Models
{
    public class Categoria
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; } = string.Empty;

        public override string ToString() => Descricao;
    }

    public class ValorNutricionalModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("kcal")]
        public float Kcal { get; set; }

        [JsonPropertyName("carboidratos")]
        public float Carboidratos { get; set; }

        [JsonPropertyName("proteinas")]
        public float Proteinas { get; set; }

        [JsonPropertyName("lipidios")]
        public float Lipidios { get; set; }

        [JsonPropertyName("medida")]
        public string Medida { get; set; } = "100g";
    }

    public class Prato
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; } = string.Empty;

        [JsonPropertyName("vegano")]
        public bool Vegano { get; set; }

        [JsonPropertyName("imagem")]
        public string? Imagem { get; set; }

        [JsonPropertyName("notaTecnica")]
        public string? NotaTecnica { get; set; }

        [JsonPropertyName("descricaoIA")]
        public string? DescricaoIA { get; set; }

        [JsonPropertyName("categoria")]
        public Categoria? Categoria { get; set; }

        // Propriedades auxiliares para compatibilidade da UI e do formulário Desktop
        public string CategoriaTexto => Categoria?.Descricao ?? (Vegano ? "Vegano" : "Tradicional");
        public string Tipo { get; set; } = "Prato Principal";
        public string Ingredientes { get; set; } = string.Empty;
        public string? ImagemBase64 { get; set; }

        // Informações nutricionais
        public double Calorias { get; set; }
        public double Proteinas { get; set; }
        public double Carboidratos { get; set; }
        public double Gorduras { get; set; }
        public double Fibras { get; set; }
        public double Sodio { get; set; }
        public double Acucares { get; set; }

        [JsonPropertyName("valorNutricional")]
        public ValorNutricionalModel? ValorNutricional { get; set; }
    }
}
