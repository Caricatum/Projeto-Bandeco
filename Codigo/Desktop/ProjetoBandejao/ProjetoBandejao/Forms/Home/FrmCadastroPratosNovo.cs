using ProjetoBandejao.Models;
using ProjetoBandejao.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace ProjetoBandejao.Forms.Home
{
    public partial class FrmCadastroPratosNovo : Form
    {
        private readonly List<string> listaIngredientes = new List<string>();
        private string? caminhoArquivoImagem = null;
        private string imagemBase64 = string.Empty;
        private readonly PratoService pratoService = new PratoService();
        private List<Categoria> categoriasDisponiveis = new List<Categoria>();

        public FrmCadastroPratosNovo()
        {
            InitializeComponent();
            CarregarCategorias();

            cbTipo.Items.Clear();
            cbTipo.Items.AddRange(new string[] { "Prato Principal", "Guarnição", "Salada", "Sobremesa", "Suco" });
            cbTipo.SelectedIndex = 0;

            // Eventos
            btnAdicionarIngrediente.Click += BtnAdicionarIngrediente_Click;
            btnSelecionarImagem.Click += BtnSelecionarImagem_Click;
            btnLimpar.Click += BtnLimpar_Click;
            
            // Responsividade
            this.Resize += FrmCadastroPratosNovo_Resize;
        }

        private void CarregarCategorias()
        {
            try
            {
                categoriasDisponiveis = pratoService.ListarCategorias();
                cbCategoria.Items.Clear();

                foreach (var cat in categoriasDisponiveis)
                {
                    cbCategoria.Items.Add(cat.Descricao);
                }

                if (cbCategoria.Items.Count > 0)
                    cbCategoria.SelectedIndex = 0;
            }
            catch
            {
                cbCategoria.Items.AddRange(new string[] { "Carnes", "Vegetariano", "Guarnição", "Salada", "Sobremesa" });
                cbCategoria.SelectedIndex = 0;
            }
        }

        private void FrmCadastroPratosNovo_Resize(object? sender, EventArgs e)
        {
            if (cardMain != null)
            {
                int x = Math.Max(20, (this.ClientSize.Width - cardMain.Width) / 2);
                int y = Math.Max(20, (this.ClientSize.Height - cardMain.Height) / 2);
                cardMain.Location = new Point(x, y);
            }
        }

        private void BtnSelecionarImagem_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imagens (*.png;*.jpg;*.jpeg;*.webp)|*.png;*.jpg;*.jpeg;*.webp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        FileInfo fileInfo = new FileInfo(ofd.FileName);
                        if (fileInfo.Length > 10 * 1024 * 1024)
                        {
                            MessageBox.Show("A imagem excede o tamanho máximo de 10MB.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        caminhoArquivoImagem = ofd.FileName;
                        byte[] imageBytes = File.ReadAllBytes(caminhoArquivoImagem);
                        imagemBase64 = Convert.ToBase64String(imageBytes);

                        lblUpload1.Text = "✓ " + Path.GetFileName(caminhoArquivoImagem);
                        lblUpload1.ForeColor = Color.ForestGreen;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao carregar a imagem: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnAdicionarIngrediente_Click(object? sender, EventArgs e)
        {
            string ingrediente = txtIngrediente.Text.Trim();
            if (!string.IsNullOrEmpty(ingrediente) && !listaIngredientes.Contains(ingrediente))
            {
                listaIngredientes.Add(ingrediente);
                AtualizarPainelIngredientes();
                txtIngrediente.Clear();
            }
        }

        private void AtualizarPainelIngredientes()
        {
            flpIngredientes.Controls.Clear();
            foreach (var ing in listaIngredientes)
            {
                Guna2Chip chip = new Guna2Chip
                {
                    Text = ing,
                    FillColor = Color.Honeydew,
                    ForeColor = Color.ForestGreen,
                    BorderColor = Color.ForestGreen,
                    IsClosable = true,
                    Margin = new Padding(3)
                };

                chip.Click += (s, ev) =>
                {
                    listaIngredientes.Remove(chip.Text);
                    AtualizarPainelIngredientes();
                };

                flpIngredientes.Controls.Add(chip);
            }
        }

        private void BtnLimpar_Click(object? sender, EventArgs e)
        {
            txtNome.Clear();
            if (cbCategoria.Items.Count > 0) cbCategoria.SelectedIndex = 0;
            if (cbTipo.Items.Count > 0) cbTipo.SelectedIndex = 0;
            txtDescricao.Clear();

            txtCalorias.Clear();
            txtProteinas.Clear();
            txtCarbs.Clear();
            txtGorduras.Clear();
            txtFibras.Clear();
            txtSodio.Clear();
            txtAcucares.Clear();

            listaIngredientes.Clear();
            AtualizarPainelIngredientes();

            caminhoArquivoImagem = null;
            imagemBase64 = string.Empty;
            lblUpload1.Text = "Arraste uma imagem ou clique abaixo";
            lblUpload1.ForeColor = Color.DimGray;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Por favor, preencha o nome do prato.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Identifica categoria selecionada
            string catNome = cbCategoria.SelectedItem?.ToString() ?? "Carnes";
            var categoriaObj = categoriasDisponiveis.FirstOrDefault(c => c.Descricao.Equals(catNome, StringComparison.OrdinalIgnoreCase));
            if (categoriaObj == null)
            {
                categoriaObj = new Categoria { Id = 1, Descricao = catNome };
            }

            bool isVegano = catNome.Contains("Vegano", StringComparison.OrdinalIgnoreCase) || 
                            catNome.Contains("Vegetariano", StringComparison.OrdinalIgnoreCase);

            // Parsing Nutricional
            double.TryParse(txtCalorias.Text, out double calorias);
            double.TryParse(txtProteinas.Text, out double proteinas);
            double.TryParse(txtCarbs.Text, out double carbs);
            double.TryParse(txtGorduras.Text, out double gorduras);
            double.TryParse(txtFibras.Text, out double fibras);
            double.TryParse(txtSodio.Text, out double sodio);
            double.TryParse(txtAcucares.Text, out double acucares);

            Prato prato = new Prato
            {
                Nome = txtNome.Text.Trim(),
                Descricao = string.IsNullOrWhiteSpace(txtDescricao.Text) ? txtNome.Text.Trim() : txtDescricao.Text.Trim(),
                Categoria = categoriaObj,
                Vegano = isVegano,
                Tipo = cbTipo.SelectedItem?.ToString() ?? "Prato Principal",
                Ingredientes = string.Join(", ", listaIngredientes),
                Calorias = calorias,
                Proteinas = proteinas,
                Carboidratos = carbs,
                Gorduras = gorduras,
                Fibras = fibras,
                Sodio = sodio,
                Acucares = acucares,
                ImagemBase64 = imagemBase64
            };

            // Chama o serviço com upload de imagem e valores nutricionais
            bool sucesso = pratoService.Cadastrar(prato, caminhoArquivoImagem);

            if (sucesso)
            {
                MessageBox.Show("Prato cadastrado com sucesso na API e banco de dados!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sidebar_Load(object sender, EventArgs e) { }
    }
}
