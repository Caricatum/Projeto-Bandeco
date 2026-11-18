using System;
using System.Text.Json.Serialization;

namespace ProjetoBandejao.Models
{
    public class Usuario
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;

        [JsonPropertyName("login")]
        public string Login { get; set; } = string.Empty;

        [JsonPropertyName("senhaHash")]
        public string Senha { get; set; } = string.Empty;

        [JsonPropertyName("funcionario")]
        public bool Funcionario { get; set; }

        [JsonPropertyName("emailConfirmado")]
        public bool EmailConfirmado { get; set; }

        [JsonPropertyName("imagem")]
        public string? Imagem { get; set; }

        [JsonPropertyName("alergiaLeite")]
        public bool AlergiaLeite { get; set; }

        [JsonPropertyName("alergiaOvo")]
        public bool AlergiaOvo { get; set; }

        [JsonPropertyName("alergiaAmendoim")]
        public bool AlergiaAmendoim { get; set; }

        [JsonPropertyName("alergiaCastanhas")]
        public bool AlergiaCastanhas { get; set; }

        [JsonPropertyName("alergiaFrutosDoMar")]
        public bool AlergiaFrutosDoMar { get; set; }

        [JsonPropertyName("alergiaPeixes")]
        public bool AlergiaPeixes { get; set; }

        [JsonPropertyName("alergiaTrigo")]
        public bool AlergiaTrigo { get; set; }

        [JsonPropertyName("alergiaSoja")]
        public bool AlergiaSoja { get; set; }

        [JsonPropertyName("alergiaGergilim")]
        public bool AlergiaGergilim { get; set; }

        public static string[] NomesRestricoes { get; } = ["Leite", "Ovo", "Amendoim", "Castanhas", "Frutos do mar", "Peixes", "Trigo", "Soja", "Gergelim"];

        [JsonIgnore]
        public int MascaraRestricoes
        {
            get => (AlergiaLeite ? 1 : 0)
                | (AlergiaOvo ? 2 : 0)
                | (AlergiaAmendoim ? 4 : 0)
                | (AlergiaCastanhas ? 8 : 0)
                | (AlergiaFrutosDoMar ? 16 : 0)
                | (AlergiaPeixes ? 32 : 0)
                | (AlergiaTrigo ? 64 : 0)
                | (AlergiaSoja ? 128 : 0)
                | (AlergiaGergilim ? 256 : 0);
            set
            {
                AlergiaLeite = (value & 1) != 0;
                AlergiaOvo = (value & 2) != 0;
                AlergiaAmendoim = (value & 4) != 0;
                AlergiaCastanhas = (value & 8) != 0;
                AlergiaFrutosDoMar = (value & 16) != 0;
                AlergiaPeixes = (value & 32) != 0;
                AlergiaTrigo = (value & 64) != 0;
                AlergiaSoja = (value & 128) != 0;
                AlergiaGergilim = (value & 256) != 0;
            }
        }

        [JsonIgnore]
        public string Restricoes
        {
            get
            {
                int mascara = MascaraRestricoes;
                return mascara == 0 ? "Nenhuma alergia informada"
                    : string.Join(", ", NomesRestricoes.Where((_, i) => (mascara & (1 << i)) != 0));
            }
        }

        [JsonPropertyName("setor")]
        public string Setor { get; set; } = "Administração";

        [JsonPropertyName("telefone")]
        public string Telefone { get; set; } = "-";

        [JsonPropertyName("cargoEspecifico")]
        public string? CargoEspecifico { get; set; }

        public string Cargo => !string.IsNullOrWhiteSpace(CargoEspecifico)
            ? CargoEspecifico
            : (Funcionario ? "Funcionário" : "Cliente / Aluno");

        public string StatusFormatado => EmailConfirmado ? "Ativo" : "Confirmação Pendente";
    }
}
