using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using ProjetoBandejao.Data;
using ProjetoBandejao.Models;

namespace ProjetoBandejao.Services
{
    public class CardapioService
    {
        private static readonly HttpClient clienteCompartilhado = new()
        {
            BaseAddress = new Uri("http://localhost:8080"),
            Timeout = TimeSpan.FromSeconds(3)
        };
        private readonly HttpClient api;

        public CardapioService() : this(clienteCompartilhado) { }

        internal CardapioService(HttpClient api) => this.api = api;

        private sealed record DiaRemoto(int Id, DateOnly Data, RefeicaoRemota? PadraoAlmoco, RefeicaoRemota? VeganoAlmoco,
            RefeicaoRemota? PadraoJantar, RefeicaoRemota? VeganoJantar);

        private sealed record RefeicaoRemota(int Id, bool Vegano, Prato? Acompanhamento, Prato? Prato_principal,
            Prato? Guarnicao, Prato? Salada, Prato? Sobremesa, Prato? Refresco);

        private sealed record Referencia(int Id);

        private sealed record RefeicaoPublicada(bool Vegano, Referencia? Acompanhamento, Referencia? Prato_principal,
            Referencia? Guarnicao, Referencia? Salada, Referencia? Sobremesa, Referencia? Refresco);

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
                // Tenta carregar do SQLite
                string? jsonSalvo = SQLiteDatabase.ObterCardapioSemana(segunda);
                if (!string.IsNullOrWhiteSpace(jsonSalvo))
                {
                    try
                    {
                        var carregado = JsonSerializer.Deserialize<List<DiaCardapio>>(jsonSalvo);
                        if (carregado != null && carregado.Count > 0)
                        {
                            var modelos = GerarDadosSemana(segunda).ToDictionary(dia => dia.Data.Date);
                            foreach (var dia in carregado)
                            {
                                if (!dia.RecebidoDaApi && !dia.AlteradoLocalmente
                                    && modelos.TryGetValue(dia.Data.Date, out var modelo))
                                    dia.AlteradoLocalmente = JsonSerializer.Serialize(dia) != JsonSerializer.Serialize(modelo);
                            }
                            _cacheSemanas[segunda] = carregado;
                            return carregado;
                        }
                    }
                    catch { }
                }

                _cacheSemanas[segunda] = GerarDadosSemana(segunda);
                SalvarSemanaNoBanco(segunda, _cacheSemanas[segunda]);
            }

            return _cacheSemanas[segunda];
        }

        public async Task<bool> AtualizarSemanaDaApiAsync(DateTime data, CancellationToken cancellationToken)
        {
            DateTime segunda = ObterSegundaFeira(data);
            List<DiaRemoto>? remotos;
            try
            {
                // A rota /semana usa a semana corrente do servidor. A listagem geral atende à navegação por data.
                remotos = await api.GetFromJsonAsync<List<DiaRemoto>>("/cardapioDia", cancellationToken);
            }
            catch (Exception ex) when (ex is HttpRequestException or OperationCanceledException or JsonException)
            {
                return false;
            }

            if (remotos == null || cancellationToken.IsCancellationRequested)
                return false;

            var porData = remotos.ToLookup(dia => dia.Data);
            var semana = ListarSemana(segunda);
            bool atualizou = false;
            foreach (var dia in semana.Where(dia => !dia.AlteradoLocalmente))
            {
                var remoto = porData[DateOnly.FromDateTime(dia.Data)].FirstOrDefault();
                if (remoto == null && !dia.RecebidoDaApi)
                    continue;

                dia.AlmocoPadrao = ConverterRefeicao(remoto?.PadraoAlmoco, false, dia.AlmocoPadrao);
                dia.AlmocoVegano = ConverterRefeicao(remoto?.VeganoAlmoco, true, dia.AlmocoVegano);
                dia.JantarPadrao = ConverterRefeicao(remoto?.PadraoJantar, false, dia.JantarPadrao);
                dia.JantarVegano = ConverterRefeicao(remoto?.VeganoJantar, true, dia.JantarVegano);
                dia.Definido = remoto?.PadraoAlmoco != null || remoto?.VeganoAlmoco != null
                    || remoto?.PadraoJantar != null || remoto?.VeganoJantar != null;
                dia.RecebidoDaApi = true;
                atualizou = true;
            }

            if (atualizou)
                SalvarSemanaNoBanco(segunda, semana);

            return atualizou;
        }

        public async Task PublicarDiaAsync(DiaCardapio dia, int usuarioId)
        {
            string versao = JsonSerializer.Serialize(dia);
            var rascunho = JsonSerializer.Deserialize<DiaCardapio>(versao)!;
            var data = DateOnly.FromDateTime(dia.Data);
            var pratos = await api.GetFromJsonAsync<List<Prato>>("/pratos")
                ?? throw new InvalidOperationException("A API não retornou os pratos cadastrados.");
            var porNome = pratos.Where(prato => prato.Id > 0)
                .ToLookup(prato => prato.Nome.Trim(), StringComparer.OrdinalIgnoreCase);

            RefeicaoPublicada? almoco = PrepararRefeicao(rascunho.AlmocoPadrao, false, porNome);
            RefeicaoPublicada? almocoVegano = PrepararRefeicao(rascunho.AlmocoVegano, true, porNome);
            RefeicaoPublicada? jantar = PrepararRefeicao(rascunho.JantarPadrao, false, porNome);
            RefeicaoPublicada? jantarVegano = PrepararRefeicao(rascunho.JantarVegano, true, porNome);

            // Reaproveita composições, inclusive quando um envio anterior parou antes de vincular o dia.
            var refeicoes = await api.GetFromJsonAsync<List<RefeicaoRemota>>("/cardapio")
                ?? throw new InvalidOperationException("A API não retornou os cardápios cadastrados.");
            var payload = new
            {
                data,
                padraoAlmoco = await ObterReferenciaRefeicaoAsync(almoco, refeicoes),
                veganoAlmoco = await ObterReferenciaRefeicaoAsync(almocoVegano, refeicoes),
                padraoJantar = await ObterReferenciaRefeicaoAsync(jantar, refeicoes),
                veganoJantar = await ObterReferenciaRefeicaoAsync(jantarVegano, refeicoes),
                user = new Referencia(usuarioId)
            };

            using var consulta = await api.GetAsync($"/cardapioDia/data/{data:yyyy-MM-dd}");
            if (consulta.StatusCode != HttpStatusCode.NotFound)
                consulta.EnsureSuccessStatusCode();
            var existente = consulta.StatusCode == HttpStatusCode.NotFound
                ? null
                : await consulta.Content.ReadFromJsonAsync<DiaRemoto>()
                    ?? throw new InvalidOperationException("A API não retornou o cardápio do dia.");

            using var resposta = existente == null
                ? await api.PostAsJsonAsync("/cardapioDia", payload)
                : await api.PutAsJsonAsync($"/cardapioDia/{existente.Id}", payload);
            resposta.EnsureSuccessStatusCode();

            var semana = ListarSemana(dia.Data);
            if (semana.Contains(dia) && JsonSerializer.Serialize(dia) == versao)
            {
                dia.AlteradoLocalmente = false;
                dia.RecebidoDaApi = true;
                SalvarSemanaNoBanco(ObterSegundaFeira(dia.Data), semana);
            }
        }

        private static RefeicaoPublicada? PrepararRefeicao(List<ItemRefeicao> itens, bool vegano,
            ILookup<string, Prato> pratos)
        {
            Referencia? ObterPrato(string categoria)
            {
                string? nome = itens.FirstOrDefault(item => item.Categoria == categoria)?.Nome.Trim();
                if (string.IsNullOrEmpty(nome) || nome is "-" or "Não informado" or "Aguardando definição")
                    return null;

                var opcoes = pratos[nome].Where(prato => !vegano || prato.Vegano).ToList();
                if (opcoes.Count != 1)
                    throw new InvalidOperationException($"Escolha um prato cadastrado com nome único para {categoria}: {nome}.");

                return new Referencia(opcoes[0].Id);
            }

            var refeicao = new RefeicaoPublicada(vegano, ObterPrato("Acompanhamentos"), ObterPrato("Prato Principal"),
                ObterPrato("Guarnição"), ObterPrato("Salada"), ObterPrato("Sobremesa"), ObterPrato("Refresco"));
            return refeicao == new RefeicaoPublicada(vegano, null, null, null, null, null, null) ? null : refeicao;
        }

        private async Task<Referencia?> ObterReferenciaRefeicaoAsync(RefeicaoPublicada? refeicao,
            List<RefeicaoRemota> refeicoes)
        {
            if (refeicao == null)
                return null;

            static Referencia? Referenciar(Prato? prato) => prato == null ? null : new Referencia(prato.Id);
            var existente = refeicoes.FirstOrDefault(item => item.Id > 0 &&
                new RefeicaoPublicada(item.Vegano, Referenciar(item.Acompanhamento), Referenciar(item.Prato_principal),
                    Referenciar(item.Guarnicao), Referenciar(item.Salada), Referenciar(item.Sobremesa), Referenciar(item.Refresco)) == refeicao);
            if (existente != null)
                return new Referencia(existente.Id);

            using var resposta = await api.PostAsJsonAsync("/cardapio", refeicao);
            resposta.EnsureSuccessStatusCode();
            var criada = await resposta.Content.ReadFromJsonAsync<RefeicaoRemota>();
            if (criada == null || criada.Id <= 0)
                throw new InvalidOperationException("A API não retornou o identificador da refeição cadastrada.");

            refeicoes.Add(criada);
            return new Referencia(criada.Id);
        }

        private static List<ItemRefeicao> ConverterRefeicao(RefeicaoRemota? refeicao, bool vegano,
            List<ItemRefeicao> itensLocais)
        {
            (string Categoria, Prato? Prato)[] itens =
            {
                ("Acompanhamentos", refeicao?.Acompanhamento),
                ("Prato Principal", refeicao?.Prato_principal),
                ("Guarnição", refeicao?.Guarnicao),
                ("Salada", refeicao?.Salada),
                ("Sobremesa", refeicao?.Sobremesa),
                ("Refresco", refeicao?.Refresco)
            };

            return itens.Select(item => new ItemRefeicao(item.Categoria, item.Prato?.Nome ?? "-", vegano)
            {
                ImagemUrl = item.Prato?.Imagem
            }).Concat(itensLocais.Where(local => !itens.Any(item => item.Categoria == local.Categoria))).ToList();
        }

        public void SalvarAlteracaoLocal(DiaCardapio dia)
        {
            dia.AlteradoLocalmente = true;
            DateTime segunda = ObterSegundaFeira(dia.Data);
            SalvarSemanaNoBanco(segunda, ListarSemana(segunda));
        }

        private void SalvarSemanaNoBanco(DateTime segunda, List<DiaCardapio> semana)
        {
            try
            {
                string json = JsonSerializer.Serialize(semana);
                SQLiteDatabase.SalvarCardapioSemana(segunda, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CardapioService.SalvarSemanaNoBanco] Erro: {ex.Message}");
            }
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
            SalvarAlteracaoLocal(dia);
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

            SalvarAlteracaoLocal(dia);
        }

        public void LimparDia(DateTime data)
        {
            var dia = ObterPorData(data);
            foreach (var item in dia.AlmocoPadrao) item.Nome = "-";
            foreach (var item in dia.AlmocoVegano) item.Nome = "-";
            foreach (var item in dia.JantarPadrao) item.Nome = "-";
            foreach (var item in dia.JantarVegano) item.Nome = "-";
            dia.Definido = false;

            SalvarAlteracaoLocal(dia);
        }

        public void CopiarDia(DateTime origemData, DateTime destinoData)
        {
            var origem = ObterPorData(origemData);
            var destino = ObterPorData(destinoData);
            if (ReferenceEquals(origem, destino)) return;

            destino.AlmocoPadrao.Clear();
            foreach (var it in origem.AlmocoPadrao) destino.AlmocoPadrao.Add(it.Clone());

            destino.AlmocoVegano.Clear();
            foreach (var it in origem.AlmocoVegano) destino.AlmocoVegano.Add(it.Clone());

            destino.JantarPadrao.Clear();
            foreach (var it in origem.JantarPadrao) destino.JantarPadrao.Add(it.Clone());

            destino.JantarVegano.Clear();
            foreach (var it in origem.JantarVegano) destino.JantarVegano.Add(it.Clone());

            destino.Definido = true;

            SalvarAlteracaoLocal(destino);
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
