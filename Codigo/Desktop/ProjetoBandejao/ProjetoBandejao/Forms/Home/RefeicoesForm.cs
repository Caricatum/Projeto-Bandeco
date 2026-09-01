using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ProjetoBandejao.Models;
using ProjetoBandejao.Services;

namespace ProjetoBandejao.Forms.Home
{
    public partial class frmRefeicoes : Form
    {
        private FlowLayoutPanel flowLayoutPanelMeals;
        private readonly PratoService pratoService = new PratoService();
        private List<Prato> listaPratosAtual = new List<Prato>();

        public frmRefeicoes()
        {
            InitializeComponent();
            InitializeMealsList();
            ConfigurarFiltrosECategorias();
            ConfigurarEventos();
        }

        private void InitializeMealsList()
        {
            // Cria e posiciona o FlowLayoutPanel dentro do painel principal de pratos
            flowLayoutPanelMeals = new FlowLayoutPanel
            {
                AutoScroll = true,
                Location = new Point(14, 55),
                Size = new Size(886, 415),
                BackColor = Color.White,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            guna2Panel2.Controls.Add(flowLayoutPanelMeals);

            // Carrega os pratos iniciais da API
            CarregarPratos();
        }

        private void ConfigurarFiltrosECategorias()
        {
            try
            {
                // Carrega as categorias reais da API para o ComboBox
                var categorias = pratoService.ListarCategorias();
                guna2ComboBox1.Items.Clear();
                guna2ComboBox1.Items.Add("Todas as categorias");

                foreach (var cat in categorias)
                {
                    guna2ComboBox1.Items.Add(cat.Descricao);
                }

                guna2ComboBox1.SelectedIndex = 0;
            }
            catch
            {
                // Fallback seguro
                if (guna2ComboBox1.Items.Count == 0)
                {
                    guna2ComboBox1.Items.AddRange(new object[] { "Todas as categorias", "Carnes", "Vegetariano", "Guarnição", "Salada", "Sobremesa" });
                    guna2ComboBox1.SelectedIndex = 0;
                }
            }
        }

        private void ConfigurarEventos()
        {
            // Botão "➕ Novo Prato" abre o formulário de cadastro e recarrega a lista
            guna2Button1.Click += (s, e) =>
            {
                using var frmCadastro = new FrmCadastroPratosNovo();
                frmCadastro.ShowDialog();
                CarregarPratos();
            };

            // Botão "🔍 Pesquisar"
            guna2Button2.Click += (s, e) => AplicarFiltros();

            // Enter no campo de busca
            guna2TextBox1.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    AplicarFiltros();
                }
            };

            // Filtro por Categoria
            guna2ComboBox1.SelectedIndexChanged += (s, e) => AplicarFiltros();

            // Ordenação (A-Z / Z-A)
            guna2ComboBox2.SelectedIndexChanged += (s, e) => AplicarFiltros();
        }

        public void CarregarPratos(string? termoBusca = null)
        {
            flowLayoutPanelMeals.Controls.Clear();

            // Busca na API
            if (string.IsNullOrWhiteSpace(termoBusca))
            {
                listaPratosAtual = pratoService.Listar();
            }
            else
            {
                listaPratosAtual = pratoService.BuscarPorNome(termoBusca);
            }

            AplicarFiltrosEmMemoria();
        }

        private void AplicarFiltros()
        {
            string termo = guna2TextBox1.DefaultText.Trim();
            if (guna2TextBox1.Text != null && guna2TextBox1.Text != guna2TextBox1.PlaceholderText)
            {
                termo = guna2TextBox1.Text.Trim();
            }

            CarregarPratos(termo);
        }

        private void AplicarFiltrosEmMemoria()
        {
            flowLayoutPanelMeals.Controls.Clear();

            IEnumerable<Prato> pratosFiltrados = listaPratosAtual;

            // Filtro de Categoria
            string categoriaSelecionada = guna2ComboBox1.SelectedItem?.ToString() ?? "Todas as categorias";
            if (categoriaSelecionada != "Todas as categorias" && !string.IsNullOrWhiteSpace(categoriaSelecionada))
            {
                pratosFiltrados = pratosFiltrados.Where(p => 
                    string.Equals(p.CategoriaTexto, categoriaSelecionada, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(p.Categoria?.Descricao, categoriaSelecionada, StringComparison.OrdinalIgnoreCase) ||
                    (categoriaSelecionada.Equals("Vegetariano", StringComparison.OrdinalIgnoreCase) && p.Vegano) ||
                    (categoriaSelecionada.Equals("Vegano", StringComparison.OrdinalIgnoreCase) && p.Vegano)
                );
            }

            // Ordenação
            string ordenacao = guna2ComboBox2.SelectedItem?.ToString() ?? "Nome (A-Z)";
            if (ordenacao.Contains("Z-A"))
            {
                pratosFiltrados = pratosFiltrados.OrderByDescending(p => p.Nome);
            }
            else
            {
                pratosFiltrados = pratosFiltrados.OrderBy(p => p.Nome);
            }

            var listaFinal = pratosFiltrados.ToList();

            // Atualiza o contador de pratos
            lblPratosCadastrados.Text = $"{listaFinal.Count} prato{(listaFinal.Count == 1 ? "" : "s")} cadastrado{(listaFinal.Count == 1 ? "" : "s")}";

            if (listaFinal.Count == 0)
            {
                var pnlAviso = new Panel
                {
                    Width = 840,
                    Height = 120,
                    BackColor = Color.FromArgb(248, 250, 248),
                    Margin = new Padding(20)
                };

                var lblSemPratos = new Label
                {
                    Text = "Nenhum prato encontrado no momento.\nClique em '➕ Novo Prato' acima para cadastrar a primeira refeição.",
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                    ForeColor = Color.DimGray
                };

                pnlAviso.Controls.Add(lblSemPratos);
                flowLayoutPanelMeals.Controls.Add(pnlAviso);
                return;
            }

            foreach (var prato in listaFinal)
            {
                var card = new CardRefeicaoControl
                {
                    Width = 860,
                    Margin = new Padding(5, 5, 5, 20)
                };

                card.PreencherDados(prato);
                card.OnExcluirPrato += (s, pratoExcluido) => CarregarPratos();

                flowLayoutPanelMeals.Controls.Add(card);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}
