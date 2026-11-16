using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoBandejao.Models;

namespace ProjetoBandejao.Services
{
    public class CardapioService
    {
        // Cache indexado pela data da segunda-feira de cada semana
        private static readonly Dictionary<DateTime, List<DiaCardapio>> _cacheSemanas = new Dictionary<DateTime, List<DiaCardapio>>();

        public static DateTime ObterSegundaFeira(DateTime data)
        {
            int diff = (7 + (data.DayOfWeek - DayOfWeek.Monday)) % 7;
            return data.Date.AddDays(-diff);
        }

        public List<DiaCardapio> ListarSemana(DateTime? dataReferencia = null)
        {
            DateTime segunda = ObterSegundaFeira(dataReferencia ?? DateTime.Today);

            if (!_cacheSemanas.ContainsKey(segunda))
            {
                _cacheSemanas[segunda] = GerarDadosSemana(segunda);
            }

            return _cacheSemanas[segunda];
        }

        public DiaCardapio ObterPorData(DateTime data)
        {
            var semana = ListarSemana(data);
            var dia = semana.FirstOrDefault(d => d.Data.Date == data.Date);
            return dia ?? semana[0];
        }

        public void AtualizarItem(DateTime data, bool isNoturno, bool isVegano, string categoria, string novoNome)
        {
            var dia = ObterPorData(data);
            List<ItemRefeicao> lista;

            if (!isNoturno)
            {
                lista = isVegano ? dia.AlmocoVegano : dia.AlmocoPadrao;
            }
            else
            {
                lista = isVegano ? dia.JantarVegano : dia.JantarPadrao;
            }

            var item = lista.FirstOrDefault(i => i.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase));
            if (item != null)
            {
                item.Nome = novoNome;
            }
            else
            {
                lista.Add(new ItemRefeicao(categoria, novoNome, isVegano));
            }
            dia.Definido = true;
        }

        public void RemoverItem(DateTime data, bool isNoturno, bool isVegano, string categoria)
        {
            var dia = ObterPorData(data);
            List<ItemRefeicao> lista = !isNoturno
                ? (isVegano ? dia.AlmocoVegano : dia.AlmocoPadrao)
                : (isVegano ? dia.JantarVegano : dia.JantarPadrao);

            var item = lista.FirstOrDefault(i => i.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase));
            if (item != null)
            {
                item.Nome = "Não informado";
            }
        }

        public void LimparDia(DateTime data)
        {
            var dia = ObterPorData(data);
            foreach (var item in dia.AlmocoPadrao) item.Nome = "-";
            foreach (var item in dia.AlmocoVegano) item.Nome = "-";
            foreach (var item in dia.JantarPadrao) item.Nome = "-";
            foreach (var item in dia.JantarVegano) item.Nome = "-";
            dia.Definido = false;
        }

        public void CopiarDia(DateTime origemData, DateTime destinoData)
        {
            var origem = ObterPorData(origemData);
            var destino = ObterPorData(destinoData);

            destino.AlmocoPadrao.Clear();
            foreach (var it in origem.AlmocoPadrao) destino.AlmocoPadrao.Add(it.Clone());

            destino.AlmocoVegano.Clear();
            foreach (var it in origem.AlmocoVegano) destino.AlmocoVegano.Add(it.Clone());

            destino.JantarPadrao.Clear();
            foreach (var it in origem.JantarPadrao) destino.JantarPadrao.Add(it.Clone());

            destino.JantarVegano.Clear();
            foreach (var it in origem.JantarVegano) destino.JantarVegano.Add(it.Clone());

            destino.Definido = true;
        }

        private List<DiaCardapio> GerarDadosSemana(DateTime segundaFeira)
        {
            var lista = new List<DiaCardapio>();

            string[] nomes = { "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado", "Domingo" };
            DayOfWeek[] diasSemana = {
                DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
                DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday
            };

            for (int i = 0; i < 7; i++)
            {
                DateTime dt = segundaFeira.AddDays(i);
                bool definido = i < 5; // Segunda a Sexta definidos por padrão
                var dia = CriarDiaPadrao(dt, nomes[i], diasSemana[i]);
                dia.Definido = definido;

                if (!definido)
                {
                    foreach (var it in dia.AlmocoPadrao) it.Nome = "Aguardando definição";
                    foreach (var it in dia.AlmocoVegano) it.Nome = "Aguardando definição";
                }

                lista.Add(dia);
            }

            return lista;
        }

        private DiaCardapio CriarDiaPadrao(DateTime data, string nomeDia, DayOfWeek diaSemana)
        {
            var dia = new DiaCardapio
            {
                Data = data,
                NomeDia = nomeDia,
                DiaSemana = diaSemana,
                Definido = true
            };

            // Cardápio Padrão (Almoço)
            dia.AlmocoPadrao = new List<ItemRefeicao>
            {
                new ItemRefeicao("Acompanhamentos", "Arroz e Feijão", false, "🍚"),
                new ItemRefeicao("Prato Principal", "Cubos de pernil acebolado", false, "🍖"),
                new ItemRefeicao("Guarnição", "Repolho refogado", false, "🥗"),
                new ItemRefeicao("Salada", "Alface, escarola e almeirão", false, "🥬"),
                new ItemRefeicao("Sobremesa", "Maçã Gala", false, "🍎"),
                new ItemRefeicao("Refresco", "Pêssego", false, "🧃"),
                new ItemRefeicao("Pão e Café", "Pão Francês e Café", false, "☕")
            };

            // Cardápio Vegano (Almoço)
            dia.AlmocoVegano = new List<ItemRefeicao>
            {
                new ItemRefeicao("Acompanhamentos", "Arroz integral e feijão", true, "🌾"),
                new ItemRefeicao("Prato Principal", "PTS ao molho escuro com legumes", true, "🌱"),
                new ItemRefeicao("Guarnição", "Quibebe", true, "🥣"),
                new ItemRefeicao("Salada", "Alface, escarola e almeirão", true, "🥬"),
                new ItemRefeicao("Sobremesa", "Maçã Gala", true, "🍎"),
                new ItemRefeicao("Refresco", "Pêssego", true, "🧃"),
                new ItemRefeicao("Pão e Café", "Pão Francês e Café", true, "☕")
            };

            // Jantar
            dia.JantarPadrao = new List<ItemRefeicao>
            {
                new ItemRefeicao("Acompanhamentos", "Arroz e Feijão", false, "🍚"),
                new ItemRefeicao("Prato Principal", "Frango ao molho de tomate", false, "🍗"),
                new ItemRefeicao("Guarnição", "Purê de batata", false, "🥔"),
                new ItemRefeicao("Salada", "Tomate com orégano e pepino", false, "🍅"),
                new ItemRefeicao("Sobremesa", "Banana Prata", false, "🍌"),
                new ItemRefeicao("Refresco", "Laranja", false, "🍊"),
                new ItemRefeicao("Pão e Café", "Pão Francês e Chá", false, "🍵")
            };

            dia.JantarVegano = new List<ItemRefeicao>
            {
                new ItemRefeicao("Acompanhamentos", "Arroz integral e lentilha", true, "🌾"),
                new ItemRefeicao("Prato Principal", "Moqueca de palmito e grão-de-bico", true, "🌱"),
                new ItemRefeicao("Guarnição", "Jardineira de legumes ao vapor", true, "🥦"),
                new ItemRefeicao("Salada", "Tomate com orégano e pepino", true, "🍅"),
                new ItemRefeicao("Sobremesa", "Banana Prata", true, "🍌"),
                new ItemRefeicao("Refresco", "Laranja", true, "🍊"),
                new ItemRefeicao("Pão e Café", "Pão Francês e Chá", true, "🍵")
            };

            return dia;
        }
    }
}
