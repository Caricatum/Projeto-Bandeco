using System;
using System.Collections.Generic;

namespace ProjetoBandejao.Models
{
    public class Prato
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        
        // Pode ser armazenado como string separada por vírgulas ou lista na API, 
        // vamos enviar como uma string para simplificar
        public string Ingredientes { get; set; } = string.Empty;

        // Informacoes Nutricionais
        public double Calorias { get; set; }
        public double Proteinas { get; set; }
        public double Carboidratos { get; set; }
        public double Gorduras { get; set; }
        public double Fibras { get; set; }
        public double Sodio { get; set; }
        public double Acucares { get; set; }
        
        // Imagem pode ser salva como byte array, base64 ou URL. Vamos prever um byte[] ou base64
        public string ImagemBase64 { get; set; } = string.Empty;
    }
}
