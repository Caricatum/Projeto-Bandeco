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

        [JsonIgnore]
        public int Alergenicos { get; set; }

        internal static IReadOnlyList<(string Campo, string Rotulo)> CamposAlergenicos { get; } =
        [
            ("leite", "Leite"),
            ("ovo", "Ovo"),
            ("amendoim", "Amendoim"),
            ("castanhas", "Castanhas"),
            ("frutosDoMar", "Frutos do mar"),
            ("peixes", "Peixes"),
            ("trigo", "Trigo"),
            ("soja", "Soja"),
            ("gergilim", "Gergelim"),
        ];

        [JsonPropertyName("leite")]
        public bool Leite { get => TemAlergenico(0); set => DefinirAlergenico(0, value); }

        [JsonPropertyName("ovo")]
        public bool Ovo { get => TemAlergenico(1); set => DefinirAlergenico(1, value); }

        [JsonPropertyName("amendoim")]
        public bool Amendoim { get => TemAlergenico(2); set => DefinirAlergenico(2, value); }

        [JsonPropertyName("castanhas")]
        public bool Castanhas { get => TemAlergenico(3); set => DefinirAlergenico(3, value); }

        [JsonPropertyName("frutosDoMar")]
        public bool FrutosDoMar { get => TemAlergenico(4); set => DefinirAlergenico(4, value); }

        [JsonPropertyName("peixes")]
        public bool Peixes { get => TemAlergenico(5); set => DefinirAlergenico(5, value); }

        [JsonPropertyName("trigo")]
        public bool Trigo { get => TemAlergenico(6); set => DefinirAlergenico(6, value); }

        [JsonPropertyName("soja")]
        public bool Soja { get => TemAlergenico(7); set => DefinirAlergenico(7, value); }

        [JsonPropertyName("gergilim")]
        public bool Gergilim { get => TemAlergenico(8); set => DefinirAlergenico(8, value); }

        internal bool TemAlergenico(int indice) => (Alergenicos & (1 << indice)) != 0;

        private void DefinirAlergenico(int indice, bool presente)
        {
            int bit = 1 << indice;
            Alergenicos = presente ? Alergenicos | bit : Alergenicos & ~bit;
        }

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
        public string? LocalImagePath { get; set; }

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
