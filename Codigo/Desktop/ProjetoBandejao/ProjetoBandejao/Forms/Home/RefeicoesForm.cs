using Guna.UI2.WinForms;
using ProjetoBandejao.Data;
using ProjetoBandejao.Models;
using ProjetoBandejao.Services;

namespace ProjetoBandejao.Forms.Home
{
    public partial class frmRefeicoes : Form
    {
        private const int PratosPorPagina = 6;
        private const string TodasCategorias = "Todas as categorias";
        private readonly FlowLayoutPanel lista;
        private readonly Guna2Button anterior;
        private readonly Guna2Button proxima;
        private readonly Label paginas;
        private readonly PratoService pratoService = new();
        private List<Prato> pratos = [];
        private int pagina;
        private bool atualizandoCategorias;
        private int versaoCarga;

        public frmRefeicoes()
        {
            InitializeComponent();
            MinimumSize = Size.Empty;
            BackColor = Color.FromArgb(248, 244, 246);
            guna2CirclePictureBox1.Dispose();
            lblSubtitulo.Visible = false;
            lblTitulo.Left = 29;
            lblTitulo.BackColor = Color.Transparent;
            lblTitulo.ForeColor = Color.FromArgb(107, 29, 47);
            label2.Text = "Lista de refeições";
            var novoPrato = new Guna2Button
            {
                Text = "Novo prato", Size = new Size(128, 38), Location = btnNovoPrato.Location,
                Anchor = AnchorStyles.Top | AnchorStyles.Right, BorderRadius = 8,
                FillColor = Color.FromArgb(107, 29, 47), Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnNovoPrato.Dispose();
            Controls.Add(novoPrato);
            ConfigurarLayoutFiltros();
            lista = new FlowLayoutPanel
            {
                AutoScroll = true, FlowDirection = FlowDirection.TopDown, WrapContents = false,
                BackColor = Color.White, Location = new Point(14, 56),
                Size = new Size(guna2Panel2.ClientSize.Width - 28, guna2Panel2.ClientSize.Height - 112),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            guna2Panel2.Controls.Add(lista);

            var paginacao = new FlowLayoutPanel
            {
                AutoSize = true, WrapContents = false, Location = new Point(14, guna2Panel2.ClientSize.Height - 46),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            anterior = CriarBotaoPagina("Anterior");
            proxima = CriarBotaoPagina("Próxima");
            paginas = new Label
            {
                Size = new Size(115, 30), TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9), ForeColor = Color.FromArgb(92, 68, 77)
            };
            paginacao.Controls.AddRange([anterior, paginas, proxima]);
            guna2Panel2.Controls.Add(paginacao);

            guna2ComboBox1.Items.Add(TodasCategorias);
            guna2ComboBox1.SelectedIndex = 0;
            guna2Button2.Click += (_, _) => AplicarFiltros();
            guna2TextBox1.KeyDown += (_, e) =>
            {
                if (e.KeyCode != Keys.Enter) return;
                e.SuppressKeyPress = true;
                AplicarFiltros();
            };
            guna2ComboBox1.SelectedIndexChanged += (_, _) =>
            {
                if (!atualizandoCategorias) AplicarFiltros();
            };
            guna2ComboBox2.SelectedIndexChanged += (_, _) => AplicarFiltros();
            lista.ClientSizeChanged += (_, _) => AjustarLarguras();
            anterior.Click += (_, _) => { pagina--; RenderizarPagina(); };
            proxima.Click += (_, _) => { pagina++; RenderizarPagina(); };
            novoPrato.Click += async (_, _) => await AbrirEditorAsync(null);
            Load += async (_, _) => await CarregarPratosAsync();
            anterior.Enabled = proxima.Enabled = false;
            lblPratosCadastrados.Text = "Carregando pratos…";
        }

        private async Task AbrirEditorAsync(Prato? prato)
        {
            using var editor = new EditarPratoForm(pratoService, prato);
            if (editor.ShowDialog(this) == DialogResult.OK && !IsDisposed) await CarregarPratosAsync();
        }

        private static Guna2Button CriarBotaoPagina(string texto) => new()
        {
            Text = texto, Size = new Size(92, 30), BorderRadius = 7,
            FillColor = Color.FromArgb(245, 237, 240), ForeColor = Color.FromArgb(107, 29, 47),
            Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand
        };

        private void ConfigurarLayoutFiltros()
        {
            PnlPesquisa.Top = 82;
            guna2Panel2.Top = 154;
            guna2Panel2.Height += 75;
            label3.Dispose();
            var filtros = new TableLayoutPanel
            {
                Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 1, Padding = new Padding(9, 11, 9, 11)
            };
            filtros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 56));
            filtros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44));
            filtros.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 122));
            filtros.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 103));
            filtros.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            Control[] campos = [guna2TextBox1, guna2ComboBox1, guna2ComboBox2, guna2Button2];
            for (int i = 0; i < campos.Length; i++)
            {
                campos[i].Dock = DockStyle.Fill;
                campos[i].Margin = new Padding(3, 2, 3, 2);
                filtros.Controls.Add(campos[i], i, 0);
            }
            guna2TextBox1.PlaceholderText = "Buscar prato…";
            guna2Button2.Text = "Buscar";
            guna2Button2.FillColor = Color.FromArgb(107, 29, 47);
            PnlPesquisa.Controls.Add(filtros);
        }

        private async Task CarregarPratosAsync()
        {
            int versao = ++versaoCarga;
            var cache = await Task.Run(SQLiteDatabase.ObterPratos);
            if (IsDisposed || versao != versaoCarga) return;
            AtualizarPratos(cache);

            var remotos = await Task.Run(pratoService.ListarAsync);
            if (IsDisposed || versao != versaoCarga) return;
            if (!pratos.Select(DadosDoCard).SequenceEqual(remotos.Select(DadosDoCard)))
                AtualizarPratos(remotos);
        }

        private static object DadosDoCard(Prato prato) => new
        {
            prato.Id, prato.Nome, prato.Descricao, prato.Ingredientes, prato.CategoriaTexto, prato.Vegano, prato.Alergenicos,
            prato.Imagem, prato.LocalImagePath, prato.ImagemBase64, prato.DescricaoIA,
            Calorias = prato.ValorNutricional?.Kcal ?? prato.Calorias,
            Carboidratos = prato.ValorNutricional?.Carboidratos ?? prato.Carboidratos,
            Proteinas = prato.ValorNutricional?.Proteinas ?? prato.Proteinas,
            Gorduras = prato.ValorNutricional?.Lipidios ?? prato.Gorduras
        };

        private void AtualizarPratos(List<Prato> novos)
        {
            pratos = novos;
            string? selecao = guna2ComboBox1.SelectedItem?.ToString();
            atualizandoCategorias = true;
            try
            {
                guna2ComboBox1.Items.Clear();
                guna2ComboBox1.Items.Add(TodasCategorias);
                guna2ComboBox1.Items.AddRange(pratos.Select(prato => prato.CategoriaTexto)
                    .Concat(pratos.Any(prato => prato.Vegano) ? new[] { "Vegano" } : Array.Empty<string>())
                    .Distinct(StringComparer.CurrentCultureIgnoreCase)
                    .Order(StringComparer.CurrentCultureIgnoreCase).Cast<object>().ToArray());
                int indice = selecao == null ? -1 : guna2ComboBox1.FindStringExact(selecao);
                guna2ComboBox1.SelectedIndex = Math.Max(0, indice);
            }
            finally
            {
                atualizandoCategorias = false;
            }
            RenderizarPagina();
        }

        private void AplicarFiltros()
        {
            pagina = 0;
            RenderizarPagina();
        }

        private void RenderizarPagina()
        {
            string termo = guna2TextBox1.Text.Trim();
            string categoria = guna2ComboBox1.SelectedItem?.ToString() ?? TodasCategorias;
            var filtrados = pratos.Where(prato =>
                (categoria == TodasCategorias || string.Equals(categoria, prato.CategoriaTexto, StringComparison.CurrentCultureIgnoreCase)
                    || (categoria is "Vegetariano" or "Vegano" && prato.Vegano))
                && (termo.Length == 0 || prato.Nome.Contains(termo, StringComparison.CurrentCultureIgnoreCase)
                    || prato.Descricao?.Contains(termo, StringComparison.CurrentCultureIgnoreCase) == true
                    || prato.Ingredientes?.Contains(termo, StringComparison.CurrentCultureIgnoreCase) == true));
            bool decrescente = guna2ComboBox2.SelectedItem?.ToString()?.Contains("Z-A") == true;
            var resultado = (decrescente ? filtrados.OrderByDescending(prato => prato.Nome) : filtrados.OrderBy(prato => prato.Nome)).ToList();
            int totalPaginas = Math.Max(1, (resultado.Count + PratosPorPagina - 1) / PratosPorPagina);
            pagina = Math.Clamp(pagina, 0, totalPaginas - 1);
            lblPratosCadastrados.Text = resultado.Count == 1 ? "1 prato" : $"{resultado.Count} pratos";
            paginas.Text = $"{pagina + 1} de {totalPaginas}";
            anterior.Enabled = pagina > 0;
            proxima.Enabled = pagina < totalPaginas - 1;

            lista.SuspendLayout();
            try
            {
                foreach (Control item in lista.Controls.Cast<Control>().ToArray()) item.Dispose();
                lista.AutoScrollPosition = Point.Empty;
                if (resultado.Count == 0)
                {
                    lista.Controls.Add(new Label
                    {
                        Height = 70, Text = termo.Length > 0 || categoria != TodasCategorias
                            ? "Nenhum prato corresponde aos filtros." : "Nenhum prato cadastrado.",
                        TextAlign = ContentAlignment.MiddleCenter, ForeColor = Color.DimGray, Font = new Font("Segoe UI", 11)
                    });
                }
                foreach (var prato in resultado.Skip(pagina * PratosPorPagina).Take(PratosPorPagina))
                {
                    var card = new CardRefeicaoControl { Margin = new Padding(0, 0, 0, 12) };
                    card.PreencherDados(prato);
                    card.OnExcluirPrato += (_, excluido) => AtualizarPratos(pratos.Where(item => item.Id != excluido.Id).ToList());
                    card.OnEditarPrato += async (_, editado) => await AbrirEditorAsync(editado);
                    card.OnVerAvaliacoes += (_, selecionado) =>
                    {
                        using var avaliacoes = new AvaliacoesPratoForm(selecionado);
                        avaliacoes.ShowDialog(this);
                    };
                    lista.Controls.Add(card);
                }
                AjustarLarguras();
            }
            finally
            {
                lista.ResumeLayout();
            }
        }

        private void AjustarLarguras()
        {
            int largura = Math.Max(280, lista.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 4);
            foreach (Control item in lista.Controls)
                if (item.Width != largura) item.Width = largura;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e) { }
        private void guna2Button1_Click(object sender, EventArgs e) { }
    }
}
