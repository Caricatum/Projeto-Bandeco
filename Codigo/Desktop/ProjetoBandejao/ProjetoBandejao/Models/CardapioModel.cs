using System;
using System.Collections.Generic;

namespace ProjetoBandejao.Models
{
    public class ItemRefeicao
    {
        public string Categoria { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public bool Vegano { get; set; }
        public string? ImagemUrl { get; set; }
        public string? IconeEmoji { get; set; }

        public ItemRefeicao() { }

        public ItemRefeicao(string categoria, string nome, bool vegano = false, string? icone = null)
        {
            Categoria = categoria;
            Nome = nome;
            Vegano = vegano;
            IconeEmoji = icone;
        }

        public ItemRefeicao Clone()
        {
            return new ItemRefeicao
            {
                Categoria = this.Categoria,
                Nome = this.Nome,
                Vegano = this.Vegano,
                ImagemUrl = this.ImagemUrl,
                IconeEmoji = this.IconeEmoji
            };
        }
    }

    public class DiaCardapio
    {
        public DayOfWeek DiaSemana { get; set; }
        public string NomeDia { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public bool Definido { get; set; } = true;
        public bool AlteradoLocalmente { get; set; }
        public bool RecebidoDaApi { get; set; }

        public List<ItemRefeicao> AlmocoPadrao { get; set; } = new List<ItemRefeicao>();
        public List<ItemRefeicao> AlmocoVegano { get; set; } = new List<ItemRefeicao>();
        public List<ItemRefeicao> JantarPadrao { get; set; } = new List<ItemRefeicao>();
        public List<ItemRefeicao> JantarVegano { get; set; } = new List<ItemRefeicao>();

        public string NotaTecnica { get; set; } = "Contém glúten no pão. Não contém ovos e lactose. Opção vegana sem ingredientes de origem animal.";
        public string CafeManha { get; set; } = "Pão Francês, Margarina vegetal, Geleia e Fruta.";

        public DiaCardapio ClonePara(DateTime novaData, string novoNomeDia, DayOfWeek novoDiaSemana)
        {
            var novo = new DiaCardapio
            {
                Data = novaData,
                NomeDia = novoNomeDia,
                DiaSemana = novoDiaSemana,
                Definido = true,
                NotaTecnica = this.NotaTecnica,
                CafeManha = this.CafeManha
            };

            foreach (var item in this.AlmocoPadrao) novo.AlmocoPadrao.Add(item.Clone());
            foreach (var item in this.AlmocoVegano) novo.AlmocoVegano.Add(item.Clone());
            foreach (var item in this.JantarPadrao) novo.JantarPadrao.Add(item.Clone());
            foreach (var item in this.JantarVegano) novo.JantarVegano.Add(item.Clone());

            return novo;
        }
    }
}
