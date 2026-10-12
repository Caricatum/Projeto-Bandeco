using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace ProjetoBandejao.Services.UserControls
{

        public partial class CadastroPratosControl : UserControl
        {
            // =========================
            // CORES
            // =========================

            private Color VerdePrincipal = Color.FromArgb(35, 92, 49);
            private Color VerdeClaro = Color.FromArgb(235, 244, 237);
            private Color Fundo = Color.FromArgb(247, 248, 247);
            private Color CinzaBorda = Color.FromArgb(220, 225, 220);
            private Color TextoEscuro = Color.FromArgb(45, 55, 48);
            private Color TextoClaro = Color.FromArgb(120, 130, 123);


            // =========================
            // CONTROLES
            // =========================

            private Guna2TextBox txtNome;
            private Guna2ComboBox cmbCategoria;
            private Guna2ComboBox cmbTipoPrato;
            private Guna2TextBox txtDescricao;

            private Guna2TextBox txtIngrediente;
            private Guna2Button btnAdicionarIngrediente;

            private FlowLayoutPanel panelIngredientes;

            private Guna2TextBox txtCalorias;
            private Guna2TextBox txtCarboidratos;
            private Guna2TextBox txtProteinas;
            private Guna2TextBox txtGorduraTotal;
            private Guna2TextBox txtGorduraSaturada;
            private Guna2TextBox txtAcucar;

            private Guna2Button btnImagem;
            private Guna2Button btnVoltar;
            private Guna2Button btnLimpar;
            private Guna2Button btnSalvar;


            public CadastroPratosControl()
            {
                InitializeComponent();

                CriarTela();
            }


            // =========================
            // CONFIGURAÇÃO PRINCIPAL
            // =========================

            private void CriarTela()
            {
                this.Dock = DockStyle.Fill;
                this.BackColor = Fundo;
                this.AutoScroll = true;

                CriarCabecalho();
                CriarInformacoesBasicas();
                CriarIngredientes();
                CriarTabelaNutricional();
                CriarRodape();
            }


            // =========================
            // LABEL
            // =========================

            private Label CriarLabel(
                string texto,
                int x,
                int y,
                int largura,
                int altura,
                float tamanho,
                FontStyle estilo,
                Color cor)
            {
                Label label = new Label();

                label.Text = texto;

                label.Location = new Point(x, y);

                label.Size = new Size(
                    largura,
                    altura);

                label.Font = new Font(
                    "Segoe UI",
                    tamanho,
                    estilo);

                label.ForeColor = cor;

                label.BackColor = Color.Transparent;

                this.Controls.Add(label);

                return label;
            }


            // =========================
            // PAINEL
            // =========================

            private Guna2Panel CriarPainel(
                int x,
                int y,
                int largura,
                int altura)
            {
                Guna2Panel panel =
                    new Guna2Panel();

                panel.Location =
                    new Point(x, y);

                panel.Size =
                    new Size(largura, altura);

                panel.BorderRadius = 10;

                panel.BorderThickness = 1;

                panel.BorderColor =
                    CinzaBorda;

                panel.FillColor =
                    Color.White;

                this.Controls.Add(panel);

                return panel;
            }


            // =========================
            // TEXTBOX
            // =========================

            private Guna2TextBox CriarTextBox(
                Control pai,
                string placeholder,
                int x,
                int y,
                int largura,
                int altura)
            {
                Guna2TextBox textBox =
                    new Guna2TextBox();

                textBox.PlaceholderText =
                    placeholder;

                textBox.Location =
                    new Point(x, y);

                textBox.Size =
                    new Size(largura, altura);

                textBox.Font =
                    new Font(
                        "Segoe UI",
                        9);

                textBox.BorderRadius = 6;

                textBox.BorderColor =
                    CinzaBorda;

                textBox.FocusedState.BorderColor =
                    VerdePrincipal;

                pai.Controls.Add(textBox);

                return textBox;
            }


            // =========================
            // CABEÇALHO
            // =========================

            private void CriarCabecalho()
            {
                CriarLabel(
                    "Refeições  >  Cadastro de Pratos",
                    25,
                    15,
                    350,
                    25,
                    8,
                    FontStyle.Regular,
                    TextoClaro);

                CriarLabel(
                    "🍴",
                    25,
                    48,
                    40,
                    40,
                    20,
                    FontStyle.Regular,
                    VerdePrincipal);

                CriarLabel(
                    "Cadastro de Pratos",
                    70,
                    45,
                    350,
                    30,
                    16,
                    FontStyle.Bold,
                    TextoEscuro);

                CriarLabel(
                    "Preencha as informações para cadastrar um novo prato.",
                    70,
                    73,
                    400,
                    25,
                    8,
                    FontStyle.Regular,
                    TextoClaro);


                btnVoltar =
                    new Guna2Button();

                btnVoltar.Text =
                    "← Voltar";

                btnVoltar.Location =
                    new Point(820, 45);

                btnVoltar.Size =
                    new Size(100, 35);

                btnVoltar.BorderRadius = 6;

                btnVoltar.FillColor =
                    Color.White;

                btnVoltar.ForeColor =
                    VerdePrincipal;

                btnVoltar.BorderColor =
                    VerdePrincipal;

                btnVoltar.BorderThickness = 1;

                btnVoltar.Font =
                    new Font(
                        "Segoe UI",
                        9,
                        FontStyle.Bold);

                this.Controls.Add(btnVoltar);
            }


            // =========================
            // INFORMAÇÕES BÁSICAS
            // =========================

            private void CriarInformacoesBasicas()
            {
                Guna2Panel panel =
                    CriarPainel(
                        25,
                        115,
                        900,
                        235);


                Label titulo =
                    new Label();

                titulo.Text =
                    "Informações Básicas";

                titulo.Location =
                    new Point(15, 10);

                titulo.Size =
                    new Size(250, 25);

                titulo.Font =
                    new Font(
                        "Segoe UI",
                        9,
                        FontStyle.Bold);

                titulo.ForeColor =
                    VerdePrincipal;

                panel.Controls.Add(titulo);


                // Nome

                AdicionarTituloCampo(
                    panel,
                    "Nome do Prato *",
                    15,
                    42);

                txtNome =
                    CriarTextBox(
                        panel,
                        "Ex.: Frango Grelhado",
                        15,
                        60,
                        400,
                        35);


                // Categoria

                AdicionarTituloCampo(
                    panel,
                    "Categoria *",
                    15,
                    105);

                cmbCategoria =
                    CriarComboBox(
                        panel,
                        "Selecione a categoria",
                        15,
                        123,
                        190,
                        35);

                cmbCategoria.Items.AddRange(
                    new object[]
                    {
                    "Prato Principal",
                    "Acompanhamento",
                    "Salada",
                    "Sobremesa",
                    "Bebida"
                    });


                // Tipo

                AdicionarTituloCampo(
                    panel,
                    "Tipo de Prato *",
                    220,
                    105);

                cmbTipoPrato =
                    CriarComboBox(
                        panel,
                        "Selecione o tipo",
                        220,
                        123,
                        195,
                        35);

                cmbTipoPrato.Items.AddRange(
                    new object[]
                    {
                    "Normal",
                    "Vegetariano",
                    "Vegano",
                    "Sem Lactose",
                    "Sem Glúten"
                    });


                // Descrição

                AdicionarTituloCampo(
                    panel,
                    "Descrição",
                    15,
                    168);

                txtDescricao =
                    CriarTextBox(
                        panel,
                        "Descreva o prato e seus ingredientes principais...",
                        15,
                        186,
                        400,
                        40);

                txtDescricao.Multiline =
                    true;


                // =========================
                // IMAGEM
                // =========================

                Label tituloImagem =
                    new Label();

                tituloImagem.Text =
                    "Imagem do Prato";

                tituloImagem.Location =
                    new Point(450, 10);

                tituloImagem.Size =
                    new Size(250, 25);

                tituloImagem.Font =
                    new Font(
                        "Segoe UI",
                        9,
                        FontStyle.Bold);

                tituloImagem.ForeColor =
                    VerdePrincipal;

                panel.Controls.Add(
                    tituloImagem);


                Guna2Panel painelImagem =
                    new Guna2Panel();

                painelImagem.Location =
                    new Point(450, 40);

                painelImagem.Size =
                    new Size(430, 185);

                painelImagem.BorderRadius = 8;

                painelImagem.BorderColor =
                    CinzaBorda;

                painelImagem.BorderThickness = 1;

                painelImagem.FillColor =
                    Color.FromArgb(
                        252,
                        253,
                        252);

                panel.Controls.Add(
                    painelImagem);


                btnImagem =
                    new Guna2Button();

                btnImagem.Text =
                    "☁\n\nArraste a imagem aqui ou clique para selecionar\n\nFormatos: JPG, PNG | Máx. 5MB";

                btnImagem.Dock =
                    DockStyle.Fill;

                btnImagem.FillColor =
                    Color.Transparent;

                btnImagem.ForeColor =
                    TextoClaro;

                btnImagem.Font =
                    new Font(
                        "Segoe UI",
                        8);

                btnImagem.Click +=
                    BtnImagem_Click;

                painelImagem.Controls.Add(
                    btnImagem);
            }


            // =========================
            // INGREDIENTES
            // =========================

            private void CriarIngredientes()
            {
                Guna2Panel panel =
                    CriarPainel(
                        25,
                        365,
                        380,
                        205);


                Label titulo =
                    new Label();

                titulo.Text =
                    "Ingredientes";

                titulo.Location =
                    new Point(15, 10);

                titulo.Size =
                    new Size(200, 25);

                titulo.Font =
                    new Font(
                        "Segoe UI",
                        9,
                        FontStyle.Bold);

                titulo.ForeColor =
                    VerdePrincipal;

                panel.Controls.Add(
                    titulo);


                Label subtitulo =
                    new Label();

                subtitulo.Text =
                    "Adicione os ingredientes do prato.";

                subtitulo.Location =
                    new Point(15, 32);

                subtitulo.Size =
                    new Size(250, 20);

                subtitulo.Font =
                    new Font(
                        "Segoe UI",
                        7);

                subtitulo.ForeColor =
                    TextoClaro;

                panel.Controls.Add(
                    subtitulo);


                txtIngrediente =
                    CriarTextBox(
                        panel,
                        "Ex.: Arroz, Feijão, Frango...",
                        15,
                        62,
                        220,
                        35);


                btnAdicionarIngrediente =
                    new Guna2Button();

                btnAdicionarIngrediente.Text =
                    "+ Adicionar";

                btnAdicionarIngrediente.Location =
                    new Point(245, 62);

                btnAdicionarIngrediente.Size =
                    new Size(115, 35);

                btnAdicionarIngrediente.BorderRadius = 6;

                btnAdicionarIngrediente.FillColor =
                    VerdePrincipal;

                btnAdicionarIngrediente.Font =
                    new Font(
                        "Segoe UI",
                        8,
                        FontStyle.Bold);

                btnAdicionarIngrediente.Click +=
                    BtnAdicionarIngrediente_Click;

                panel.Controls.Add(
                    btnAdicionarIngrediente);


                panelIngredientes =
                    new FlowLayoutPanel();

                panelIngredientes.Location =
                    new Point(15, 110);

                panelIngredientes.Size =
                    new Size(345, 75);

                panelIngredientes.AutoScroll =
                    true;

                panelIngredientes.WrapContents =
                    true;

                panel.Controls.Add(
                    panelIngredientes);
            }


            // =========================
            // TABELA NUTRICIONAL
            // =========================

            private void CriarTabelaNutricional()
            {
                Guna2Panel panel =
                    CriarPainel(
                        420,
                        365,
                        505,
                        205);


                Label titulo =
                    new Label();

                titulo.Text =
                    "Tabela Nutricional (por porção)";

                titulo.Location =
                    new Point(15, 10);

                titulo.Size =
                    new Size(300, 25);

                titulo.Font =
                    new Font(
                        "Segoe UI",
                        9,
                        FontStyle.Bold);

                titulo.ForeColor =
                    VerdePrincipal;

                panel.Controls.Add(
                    titulo);


                // Linha 1

                AdicionarTituloCampo(
                    panel,
                    "Calorias (kcal)",
                    15,
                    45);

                txtCalorias =
                    CriarTextBox(
                        panel,
                        "Ex.: 350",
                        15,
                        63,
                        145,
                        35);


                AdicionarTituloCampo(
                    panel,
                    "Carboidratos (g)",
                    175,
                    45);

                txtCarboidratos =
                    CriarTextBox(
                        panel,
                        "Ex.: 45",
                        175,
                        63,
                        145,
                        35);


                AdicionarTituloCampo(
                    panel,
                    "Proteínas (g)",
                    335,
                    45);

                txtProteinas =
                    CriarTextBox(
                        panel,
                        "Ex.: 25",
                        335,
                        63,
                        145,
                        35);


                // Linha 2

                AdicionarTituloCampo(
                    panel,
                    "Gordura Total (g)",
                    15,
                    112);

                txtGorduraTotal =
                    CriarTextBox(
                        panel,
                        "Ex.: 10",
                        15,
                        130,
                        145,
                        35);


                AdicionarTituloCampo(
                    panel,
                    "Gordura Saturada (g)",
                    175,
                    112);

                txtGorduraSaturada =
                    CriarTextBox(
                        panel,
                        "Ex.: 5",
                        175,
                        130,
                        145,
                        35);


                AdicionarTituloCampo(
                    panel,
                    "Açúcares (g)",
                    335,
                    112);

                txtAcucar =
                    CriarTextBox(
                        panel,
                        "Ex.: 2",
                        335,
                        130,
                        145,
                        35);
            }


            // =========================
            // RODAPÉ
            // =========================

            private void CriarRodape()
            {
                Guna2Panel aviso =
                    new Guna2Panel();

                aviso.Location =
                    new Point(25, 585);

                aviso.Size =
                    new Size(400, 42);

                aviso.BorderRadius =
                    7;

                aviso.FillColor =
                    VerdeClaro;


                Label textoAviso =
                    new Label();

                textoAviso.Text =
                    "ⓘ  Os campos com * são obrigatórios.";

                textoAviso.Dock =
                    DockStyle.Fill;

                textoAviso.TextAlign =
                    ContentAlignment.MiddleCenter;

                textoAviso.Font =
                    new Font(
                        "Segoe UI",
                        8);

                textoAviso.ForeColor =
                    TextoClaro;


                aviso.Controls.Add(
                    textoAviso);

                this.Controls.Add(
                    aviso);


                // LIMPAR

                btnLimpar =
                    new Guna2Button();

                btnLimpar.Text =
                    "🗑 Limpar";

                btnLimpar.Location =
                    new Point(650, 585);

                btnLimpar.Size =
                    new Size(110, 42);

                btnLimpar.BorderRadius =
                    7;

                btnLimpar.FillColor =
                    Color.White;

                btnLimpar.ForeColor =
                    TextoEscuro;

                btnLimpar.BorderColor =
                    CinzaBorda;

                btnLimpar.BorderThickness =
                    1;

                btnLimpar.Click +=
                    BtnLimpar_Click;

                this.Controls.Add(
                    btnLimpar);


                // SALVAR

                btnSalvar =
                    new Guna2Button();

                btnSalvar.Text =
                    "▣  Salvar Prato";

                btnSalvar.Location =
                    new Point(770, 585);

                btnSalvar.Size =
                    new Size(155, 42);

                btnSalvar.BorderRadius =
                    7;

                btnSalvar.FillColor =
                    VerdePrincipal;

                btnSalvar.Font =
                    new Font(
                        "Segoe UI",
                        9,
                        FontStyle.Bold);

                btnSalvar.Click +=
                    BtnSalvar_Click;

                this.Controls.Add(
                    btnSalvar);
            }


            // =========================
            // COMBOBOX
            // =========================

            private Guna2ComboBox CriarComboBox(
                Control pai,
                string texto,
                int x,
                int y,
                int largura,
                int altura)
            {
                Guna2ComboBox combo =
                    new Guna2ComboBox();

                combo.Location =
                    new Point(x, y);

                combo.Size =
                    new Size(largura, altura);

                combo.BorderRadius =
                    6;

                combo.BorderColor =
                    CinzaBorda;

                combo.ForeColor =
                    TextoEscuro;

                combo.Font =
                    new Font(
                        "Segoe UI",
                        8);

                combo.DropDownStyle =
                    ComboBoxStyle.DropDownList;

                combo.Items.Add(texto);

                combo.SelectedIndex =
                    0;

                pai.Controls.Add(
                    combo);

                return combo;
            }


            // =========================
            // TÍTULO DE CAMPO
            // =========================

            private void AdicionarTituloCampo(
                Control pai,
                string texto,
                int x,
                int y)
            {
                Label label =
                    new Label();

                label.Text =
                    texto;

                label.Location =
                    new Point(x, y);

                label.Size =
                    new Size(180, 20);

                label.Font =
                    new Font(
                        "Segoe UI",
                        7,
                        FontStyle.Bold);

                label.ForeColor =
                    TextoClaro;

                pai.Controls.Add(
                    label);
            }


            // =========================
            // ADICIONAR INGREDIENTE
            // =========================

            private void BtnAdicionarIngrediente_Click(
                object sender,
                EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(
                    txtIngrediente.Text))
                {
                    return;
                }


                Guna2Button ingrediente =
                    new Guna2Button();

                ingrediente.Text =
                    txtIngrediente.Text;

                ingrediente.Size =
                    new Size(150, 30);

                ingrediente.BorderRadius =
                    15;

                ingrediente.FillColor =
                    VerdeClaro;

                ingrediente.ForeColor =
                    VerdePrincipal;

                ingrediente.Font =
                    new Font(
                        "Segoe UI",
                        8);

                ingrediente.Click +=
                    (s, ev) =>
                    {
                        panelIngredientes.Controls.Remove(
                            ingrediente);
                    };


                panelIngredientes.Controls.Add(
                    ingrediente);

                txtIngrediente.Clear();
            }


            // =========================
            // ESCOLHER IMAGEM
            // =========================

            private void BtnImagem_Click(
                object sender,
                EventArgs e)
            {
                OpenFileDialog dialog =
                    new OpenFileDialog();

                dialog.Filter =
                    "Imagens|*.jpg;*.jpeg;*.png";

                if (dialog.ShowDialog() ==
                    DialogResult.OK)
                {
                    btnImagem.Text =
                        "✓ Imagem selecionada\n\n" +
                        dialog.FileName;
                }
            }


            // =========================
            // LIMPAR
            // =========================

            private void BtnLimpar_Click(
                object sender,
                EventArgs e)
            {
                txtNome.Clear();

                cmbCategoria.SelectedIndex =
                    0;

                cmbTipoPrato.SelectedIndex =
                    0;

                txtDescricao.Clear();

                txtIngrediente.Clear();

                panelIngredientes.Controls.Clear();

                txtCalorias.Clear();

                txtCarboidratos.Clear();

                txtProteinas.Clear();

                txtGorduraTotal.Clear();

                txtGorduraSaturada.Clear();

                txtAcucar.Clear();

                btnImagem.Text =
                    "☁\n\nArraste a imagem aqui ou clique para selecionar\n\n" +
                    "Formatos: JPG, PNG | Máx. 5MB";
            }


            // =========================
            // SALVAR
            // =========================

            private void BtnSalvar_Click(
                object sender,
                EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(
                        txtNome.Text))
                {
                    MessageBox.Show(
                        "Informe o nome do prato.",
                        "Atenção",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }


                MessageBox.Show(
                    "Prato pronto para ser salvo!",
                    "Cadastro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);


                // AQUI posteriormente vamos
                // conectar com sua API:
                //
                // PratoService
                // → POST /prato/cadastrar
            }
        }
    }
