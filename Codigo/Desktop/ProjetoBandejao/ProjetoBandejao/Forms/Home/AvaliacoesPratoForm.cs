using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ProjetoBandejao.Models;

namespace ProjetoBandejao.Forms.Home
{
    internal sealed class AvaliacoesPratoForm : Form
    {
        private static readonly Color Vinho = Color.FromArgb(107, 29, 47);
        private static readonly HttpClient api = new()
        {
            BaseAddress = new Uri("http://localhost:8080/"),
            Timeout = TimeSpan.FromSeconds(6)
        };
        private readonly CancellationTokenSource fechar = new();
        private readonly Label estado = CriarRotulo("Carregando avaliações…", 10);
        private readonly TextBox resumo = new()
        {
            Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical,
            BorderStyle = BorderStyle.FixedSingle, BackColor = Color.White, AccessibleName = "Resumo por IA"
        };
        private readonly DataGridView grade = new()
        {
            Dock = DockStyle.Fill, BackgroundColor = Color.White, BorderStyle = BorderStyle.None,
            ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, RowHeadersVisible = false,
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        };

        private sealed record Avaliacao(
            [property: JsonPropertyName("nota")] int Nota,
            [property: JsonPropertyName("avaliacao")] string? Comentario);

        public AvaliacoesPratoForm(Prato prato)
        {
            Text = "Avaliações do prato";
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(490, 445);
            ClientSize = new Size(680, 535);
            BackColor = Color.FromArgb(249, 244, 246);
            Font = new Font("Segoe UI", 10);
            Padding = new Padding(22);
            ShowInTaskbar = false;
            MinimizeBox = false;

            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 6 };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            foreach (int altura in new[] { 44, 35, 30, 108, 34 })
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, altura));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.Controls.Add(CriarRotulo(prato.Nome, 19, FontStyle.Bold), 0, 0);
            layout.Controls.Add(estado, 0, 1);
            layout.Controls.Add(CriarRotulo("Resumo por IA", 11, FontStyle.Bold), 0, 2);
            resumo.Text = TextoResumo(prato.DescricaoIA);
            layout.Controls.Add(resumo, 0, 3);
            layout.Controls.Add(CriarRotulo("Notas e comentários", 11, FontStyle.Bold), 0, 4);
            grade.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Nota", Width = 69 });
            grade.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Comentário", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { WrapMode = DataGridViewTriState.True }
            });
            grade.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 242, 246);
            layout.Controls.Add(grade, 0, 5);
            Controls.Add(layout);

            Shown += async (_, _) => await CarregarAsync(prato, fechar.Token);
            Disposed += (_, _) =>
            {
                fechar.Cancel();
                fechar.Dispose();
            };
        }

        private async Task CarregarAsync(Prato prato, CancellationToken token)
        {
            var atualizarResumo = AtualizarResumoAsync(prato.Id, token);
            try
            {
                var avaliacoes = await api.GetFromJsonAsync<List<Avaliacao>>($"avaliacoes?pratoId={prato.Id}", token)
                    ?? throw new JsonException("Lista de avaliações ausente.");
                if (IsDisposed || token.IsCancellationRequested) return;
                estado.Text = avaliacoes.Count == 0 ? "Este prato ainda não recebeu avaliações."
                    : $"Nota média: {avaliacoes.Average(avaliacao => avaliacao.Nota):0.0}  ·  {avaliacoes.Count} avaliação(ões)";
                foreach (var avaliacao in avaliacoes)
                    grade.Rows.Add(avaliacao.Nota,
                        string.IsNullOrWhiteSpace(avaliacao.Comentario) ? "Sem comentário" : avaliacao.Comentario);
                grade.ClearSelection();
            }
            catch (Exception ex) when (ex is HttpRequestException or OperationCanceledException or JsonException)
            {
                if (!IsDisposed && !token.IsCancellationRequested)
                    estado.Text = "Avaliações indisponíveis. Confira a conexão com a API.";
            }
            finally
            {
                await atualizarResumo;
            }
        }

        private async Task AtualizarResumoAsync(int id, CancellationToken token)
        {
            try
            {
                var prato = await api.GetFromJsonAsync<Prato>($"pratos/{id}", token);
                if (prato != null && !IsDisposed && !token.IsCancellationRequested)
                    resumo.Text = TextoResumo(prato.DescricaoIA);
            }
            catch (Exception ex) when (ex is HttpRequestException or OperationCanceledException or JsonException)
            {
                if (!IsDisposed && !token.IsCancellationRequested && resumo.Text != TextoResumo(null))
                    resumo.Text += "\r\n\r\nResumo salvo neste computador.";
            }
        }

        private static string TextoResumo(string? texto) =>
            string.IsNullOrWhiteSpace(texto) ? "Ainda não há um resumo por IA para este prato." : texto.Trim();

        private static Label CriarRotulo(string texto, float tamanho, FontStyle estilo = FontStyle.Regular) =>
            new()
            {
                Text = texto, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, AutoEllipsis = true,
                ForeColor = Vinho, Font = new Font("Segoe UI", tamanho, estilo)
            };
    }
}
