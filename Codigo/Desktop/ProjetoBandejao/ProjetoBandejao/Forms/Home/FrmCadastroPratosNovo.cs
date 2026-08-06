using ProjetoBandejao.Models;
using ProjetoBandejao.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace ProjetoBandejao.Forms.Home
{
    public partial class FrmCadastroPratosNovo : Form
    {
        private List<string> listaIngredientes = new List<string>();
        private string imagemBase64 = string.Empty;
        private PratoService pratoService = new PratoService();

        public FrmCadastroPratosNovo()
        {
            InitializeComponent();

            // Populating ComboBoxes
            cbCategoria.Items.AddRange(new string[] { "Tradicional", "Vegetariano", "Fitness" });
            cbCategoria.SelectedIndex = 0;

            cbTipo.Items.AddRange(new string[] { "Prato Principal", "Guarnição", "Salada", "Sobremesa", "Suco" });
            cbTipo.SelectedIndex = 0;

            // Events
            btnAdicionarIngrediente.Click += BtnAdicionarIngrediente_Click;
            btnSelecionarImagem.Click += BtnSelecionarImagem_Click;
            btnLimpar.Click += BtnLimpar_Click;
            
            // Responsiveness
            this.Resize += FrmCadastroPratosNovo_Resize;
        }

        private void FrmCadastroPratosNovo_Resize(object? sender, EventArgs e)
        {
            if (cardMain != null)
            {
                int x = Math.Max(20, (this.ClientSize.Width - 200 - cardMain.Width) / 2 + 200);
                int y = Math.Max(20, (this.ClientSize.Height - cardMain.Height) / 2);
                cardMain.Location = new Point(x, y);
            }
        }

        private void BtnSelecionarImagem_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imagens (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        byte[] imageBytes = File.ReadAllBytes(ofd.FileName);
                        if (imageBytes.Length > 5 * 1024 * 1024)
                        {
                            MessageBox.Show("A imagem excede o tamanho máximo de 5MB.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        imagemBase64 = Convert.ToBase64String(imageBytes);
                        lblUpload1.Text = Path.GetFileName(ofd.FileName);
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
            cbCategoria.SelectedIndex = 0;
            cbTipo.SelectedIndex = 0;
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

            // Parsing Nutricional Info safely
            double.TryParse(txtCalorias.Text, out double calorias);
            double.TryParse(txtProteinas.Text, out double proteinas);
            double.TryParse(txtCarbs.Text, out double carbs);
            double.TryParse(txtGorduras.Text, out double gorduras);
            double.TryParse(txtFibras.Text, out double fibras);
            double.TryParse(txtSodio.Text, out double sodio);
            double.TryParse(txtAcucares.Text, out double acucares);

            Prato prato = new Prato
            {
                Nome = txtNome.Text,
                Categoria = cbCategoria.SelectedItem?.ToString() ?? "",
                Tipo = cbTipo.SelectedItem?.ToString() ?? "",
                Descricao = txtDescricao.Text,
                Ingredientes = string.Join(",", listaIngredientes),
                Calorias = calorias,
                Proteinas = proteinas,
                Carboidratos = carbs,
                Gorduras = gorduras,
                Fibras = fibras,
                Sodio = sodio,
                Acucares = acucares,
                ImagemBase64 = imagemBase64
            };

            // Call API
            bool sucesso = pratoService.Cadastrar(prato);

            if (sucesso)
            {
                MessageBox.Show("Prato salvo com sucesso no banco de dados!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BtnLimpar_Click(this, EventArgs.Empty);
            }
            // The service already handles the error message if it fails
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sidebar_Load(object sender, EventArgs e)
        {

        }
    }
}
