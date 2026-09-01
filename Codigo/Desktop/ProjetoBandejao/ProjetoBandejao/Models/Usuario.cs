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

        public string Cargo => Funcionario ? "Administrador / Funcionário" : "Cliente / Aluno";
        public string StatusFormatado => EmailConfirmado ? "Ativo" : "Confirmação Pendente";
    }
}
