using Guna.UI2.WinForms;
using ProjetoBandejao.Models;
using ProjetoBandejao.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoBandejao.Forms.Login
{
        public partial class CadastroFuncionarioFormNovo : Form
        {
            private Guna2Panel pnlPrincipal;
            private Guna2Panel pnlCabecalho;
            private Guna2Panel pnlFormulario;

            private Label lblTitulo;
            private Label lblSubtitulo;

            private Label lblNome;
            private Label lblEmail;
            private Label lblSenha;
            private Label lblFuncionario;

            private Guna2TextBox txtNome;
            private Guna2TextBox txtEmail;
            private Guna2TextBox txtSenha;

            private Guna2CheckBox chkAdministrador;

            private Guna2Button btnCancelar;
            private Guna2Button btnCadastrar;

            private PictureBox picLogo;

            public CadastroFuncionarioFormNovo()
            {
                InitializeComponent();

                CriarTela();
            }

            private void CriarTela()
            {
                // =========================
                // CONFIGURAÇÃO DA JANELA
                // =========================

                this.Text = "Cadastro de Funcionário";

                this.Size = new Size(950, 650);

                this.StartPosition =
                    FormStartPosition.CenterScreen;

                this.BackColor =
                    Color.FromArgb(250, 245, 246);

                this.FormBorderStyle =
                    FormBorderStyle.Sizable;

                this.MinimumSize =
                    new Size(850, 600);


                // =========================
                // PAINEL PRINCIPAL
                // =========================

                pnlPrincipal = new Guna2Panel();

                pnlPrincipal.Dock =
                    DockStyle.Fill;

                pnlPrincipal.FillColor =
                    Color.FromArgb(250, 245, 246);

                pnlPrincipal.BorderRadius = 0;

                this.Controls.Add(pnlPrincipal);


                // =========================
                // CABEÇALHO
                // =========================

                pnlCabecalho = new Guna2Panel();

                pnlCabecalho.Location =
                    new Point(0, 0);

                pnlCabecalho.Size =
                    new Size(950, 155);

                pnlCabecalho.Anchor =
                    AnchorStyles.Top |
                    AnchorStyles.Left |
                    AnchorStyles.Right;

                pnlCabecalho.FillColor =
                    Color.White;

                pnlCabecalho.BorderRadius = 0;

                pnlPrincipal.Controls.Add(pnlCabecalho);


                // =========================
                // LOGO
                // =========================

                picLogo = new PictureBox();

                picLogo.Location =
                    new Point(45, 35);

                picLogo.Size =
                    new Size(210, 80);

                picLogo.SizeMode =
                    PictureBoxSizeMode.Zoom;

                picLogo.BackColor =
                    Color.Transparent;

                // Depois coloque sua logo aqui:
                // picLogo.Image = Properties.Resources.LogoCotil;

                pnlCabecalho.Controls.Add(picLogo);


                // =========================
                // TÍTULO
                // =========================

                lblTitulo = new Label();

                lblTitulo.Text =
                    "Cadastro de Funcionário";

                lblTitulo.Font =
                    new Font(
                        "Segoe UI",
                        27,
                        FontStyle.Bold);

                lblTitulo.ForeColor =
                    Color.FromArgb(170, 0, 20);

                lblTitulo.AutoSize =
                    true;

                lblTitulo.Location =
                    new Point(310, 38);

                pnlCabecalho.Controls.Add(lblTitulo);


                // =========================
                // SUBTÍTULO
                // =========================

                lblSubtitulo = new Label();

                lblSubtitulo.Text =
                    "Preencha as informações abaixo para cadastrar um novo funcionário.";

                lblSubtitulo.Font =
                    new Font(
                        "Segoe UI",
                        10.5f,
                        FontStyle.Regular);

                lblSubtitulo.ForeColor =
                    Color.FromArgb(100, 100, 100);

                lblSubtitulo.AutoSize =
                    true;

                lblSubtitulo.Location =
                    new Point(313, 82);

                pnlCabecalho.Controls.Add(lblSubtitulo);


                // =========================
                // LINHA VERMELHA
                // =========================

                Panel linha = new Panel();

                linha.Location =
                    new Point(0, 153);

                linha.Size =
                    new Size(950, 3);

                linha.BackColor =
                    Color.FromArgb(190, 0, 25);

                linha.Anchor =
                    AnchorStyles.Top |
                    AnchorStyles.Left |
                    AnchorStyles.Right;

                pnlCabecalho.Controls.Add(linha);


                // =========================
                // CARD DO FORMULÁRIO
                // =========================

                pnlFormulario = new Guna2Panel();

                pnlFormulario.Location =
                    new Point(30, 180);

                pnlFormulario.Size =
                    new Size(890, 320);

                pnlFormulario.Anchor =
                    AnchorStyles.Top |
                    AnchorStyles.Left |
                    AnchorStyles.Right;

                pnlFormulario.FillColor =
                    Color.White;

                pnlFormulario.BorderRadius =
                    20;

                pnlFormulario.ShadowDecoration.Enabled =
                    true;

                pnlFormulario.ShadowDecoration.Depth =
                    8;

                pnlPrincipal.Controls.Add(pnlFormulario);


                // =========================
                // NOME
                // =========================

                lblNome = CriarLabel(
                    "Nome completo",
                    55,
                    25);

                pnlFormulario.Controls.Add(lblNome);


                txtNome = CriarTextBox(
                    "Digite o nome completo",
                    55,
                    55);

                pnlFormulario.Controls.Add(txtNome);


                // =========================
                // E-MAIL
                // =========================

                lblEmail = CriarLabel(
                    "Usuário (e-mail)",
                    55,
                    105);

                pnlFormulario.Controls.Add(lblEmail);


                txtEmail = CriarTextBox(
                    "Digite o seu e-mail",
                    55,
                    135);

                pnlFormulario.Controls.Add(txtEmail);


                // =========================
                // SENHA
                // =========================

                lblSenha = CriarLabel(
                    "Senha",
                    55,
                    185);

                pnlFormulario.Controls.Add(lblSenha);


                txtSenha = CriarTextBox(
                    "Digite a senha",
                    55,
                    215);

                txtSenha.UseSystemPasswordChar =
                    true;

                pnlFormulario.Controls.Add(txtSenha);


                // =========================
                // FUNCIONÁRIO
                // =========================

                lblFuncionario = CriarLabel(
                    "É funcionário/administrador?",
                    510,
                    25);

                pnlFormulario.Controls.Add(lblFuncionario);


                chkAdministrador =
                    new Guna2CheckBox();

                chkAdministrador.Text =
                    "Sim, este usuário possui acesso administrativo";

                chkAdministrador.Font =
                    new Font(
                        "Segoe UI",
                        10);

                chkAdministrador.ForeColor =
                    Color.FromArgb(40, 40, 40);

                chkAdministrador.CheckedState.FillColor =
                    Color.FromArgb(190, 0, 25);

                chkAdministrador.CheckedState.BorderColor =
                    Color.FromArgb(190, 0, 25);

                chkAdministrador.UncheckedState.BorderColor =
                    Color.FromArgb(150, 150, 150);

                chkAdministrador.Location =
                    new Point(510, 62);

                chkAdministrador.Size =
                    new Size(330, 30);

                pnlFormulario.Controls.Add(
                    chkAdministrador);


                // =========================
                // BOTÃO CANCELAR
                // =========================

                btnCancelar =
                    new Guna2Button();

                btnCancelar.Text =
                    "✕  Cancelar";

                btnCancelar.Font =
                    new Font(
                        "Segoe UI",
                        11,
                        FontStyle.Bold);

                btnCancelar.ForeColor =
                    Color.FromArgb(190, 0, 25);

                btnCancelar.FillColor =
                    Color.White;

                btnCancelar.BorderColor =
                    Color.FromArgb(210, 210, 210);

                btnCancelar.BorderThickness =
                    1;

                btnCancelar.BorderRadius =
                    10;

                btnCancelar.Size =
                    new Size(220, 55);

                btnCancelar.Location =
                    new Point(230, 525);

                btnCancelar.Anchor =
                    AnchorStyles.Bottom;

                btnCancelar.Click +=
                    btnCancelar_Click;

                pnlPrincipal.Controls.Add(
                    btnCancelar);


                // =========================
                // BOTÃO CADASTRAR
                // =========================

                btnCadastrar =
                    new Guna2Button();

                btnCadastrar.Text =
                    "▣  Cadastrar";

                btnCadastrar.Font =
                    new Font(
                        "Segoe UI",
                        11,
                        FontStyle.Bold);

                btnCadastrar.ForeColor =
                    Color.White;

                btnCadastrar.FillColor =
                    Color.FromArgb(190, 0, 25);

                btnCadastrar.BorderRadius =
                    10;

                btnCadastrar.HoverState.FillColor =
                    Color.FromArgb(150, 0, 18);

                btnCadastrar.Size =
                    new Size(220, 55);

                btnCadastrar.Location =
                    new Point(500, 525);

                btnCadastrar.Anchor =
                    AnchorStyles.Bottom;

                btnCadastrar.Click +=
                    btnCadastrar_Click;

                pnlPrincipal.Controls.Add(
                    btnCadastrar);
            }


            // ==================================================
            // CRIA LABEL
            // ==================================================

            private Label CriarLabel(
                string texto,
                int x,
                int y)
            {
                Label label = new Label();

                label.Text = texto;

                label.Font =
                    new Font(
                        "Segoe UI",
                        11,
                        FontStyle.Bold);

                label.ForeColor =
                    Color.FromArgb(160, 0, 20);

                label.AutoSize =
                    true;

                label.Location =
                    new Point(x, y);

                return label;
            }


            // ==================================================
            // CRIA TEXTBOX
            // ==================================================

            private Guna2TextBox CriarTextBox(
                string placeholder,
                int x,
                int y)
            {
                Guna2TextBox textbox =
                    new Guna2TextBox();

                textbox.PlaceholderText =
                    placeholder;

                textbox.Font =
                    new Font(
                        "Segoe UI",
                        10.5f);

                textbox.ForeColor =
                    Color.FromArgb(50, 50, 50);

                textbox.BorderColor =
                    Color.FromArgb(210, 210, 210);

                textbox.BorderRadius =
                    10;

                textbox.BorderThickness =
                    1;

                textbox.FillColor =
                    Color.White;

                textbox.Size =
                    new Size(400, 48);

                textbox.Location =
                    new Point(x, y);

                textbox.FocusedState.BorderColor =
                    Color.FromArgb(190, 0, 25);

                return textbox;
            }


            // ==================================================
            // CADASTRAR
            // ==================================================

            private void btnCadastrar_Click(
                object sender,
                EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text) ||
                    string.IsNullOrWhiteSpace(txtSenha.Text))
                {
                    MessageBox.Show(
                        "Preencha todos os campos.",
                        "Atenção",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                Usuario usuario = new Usuario
                {
                    Nome = txtNome.Text.Trim(),
                    Login = txtEmail.Text.Trim(),
                    Senha = txtSenha.Text,
                    Funcionario =
                        chkAdministrador.Checked
                };

                UsuarioService service =
                    new UsuarioService();

                bool sucesso =
                    service.Cadastrar(
                        usuario,
                        out string mensagemErro);

                if (sucesso)
                {
                    MessageBox.Show(
                        "Funcionário cadastrado com sucesso!\n\n" +
                        "Um código de confirmação foi enviado para o e-mail.",
                        "Sucesso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    ConfirmarCodigoForm tela =
                        new ConfirmarCodigoForm(
                            txtEmail.Text.Trim());

                    tela.ShowDialog();

                    this.Close();
                }
                else
                {
                    string detalhe =
                        string.IsNullOrWhiteSpace(
                            mensagemErro)
                            ? "Falha ao comunicar com a API."
                            : mensagemErro;

                    MessageBox.Show(
                        $"Erro ao cadastrar funcionário:\n{detalhe}",
                        "Erro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }


            // ==================================================
            // CANCELAR
            // ==================================================

            private void btnCancelar_Click(
                object sender,
                EventArgs e)
            {
                this.Close();
            }
        }
    }
