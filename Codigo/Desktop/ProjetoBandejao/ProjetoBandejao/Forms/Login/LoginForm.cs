using ProjetoBandejao.Data;
using ProjetoBandejao.Models;
using ProjetoBandejao.Services;
using ProjetoBandejao.Forms.Login;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoBandejao.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private async void btnEntrar_Click(object sender, EventArgs e)
        {
            string emailDigitado = txtEmail.Text.Trim();
            string senhaDigitada = txtSenha.Text;

            if (string.IsNullOrWhiteSpace(emailDigitado))
            {
                lblMensagem.Text = "Por favor, informe seu usuário ou e-mail.";
                return;
            }

            btnEntrar.Enabled = false;
            string textoOriginal = btnEntrar.Text;
            btnEntrar.Text = "Entrando...";
            lblMensagem.Text = string.Empty;
            Cursor = Cursors.WaitCursor;

            try
            {
                Usuario usuario = new Usuario
                {
                    Login = emailDigitado,
                    Senha = senhaDigitada
                };

                UsuarioService service = new UsuarioService();
                string mensagemErro = string.Empty;
                bool isOffline = false;
                bool loginValido = false;

                // Executa em segundo plano para não travar a UI Thread
                await Task.Run(() =>
                {
                    loginValido = service.ValidarLogin(usuario, out mensagemErro, out isOffline);
                });

                if (loginValido)
                {
                    UsuarioSession.IsModoOffline = isOffline;
                    Usuario? usuarioCompleto;
                    if (isOffline)
                    {
                        SQLiteDatabase.ValidarSenhaLocal(usuario.Login, senhaDigitada, out usuarioCompleto);
                        if (usuarioCompleto == null)
                        {
                            lblMensagem.Text = "A conta local não está disponível para acesso offline.";
                            return;
                        }
                    }
                    else
                    {
                        usuarioCompleto = await Task.Run(() => service.ObterPorLogin(usuario.Login));
                    }
                    UsuarioSession.UsuarioLogado = usuarioCompleto ?? usuario;

                    if (isOffline)
                    {
                        Console.WriteLine("[LoginForm] Acesso concedido em Modo Offline (SQLite).");
                    }

                    HomeForm home = new HomeForm();
                    home.FormClosed += (s, args) => Application.Exit();
                    home.Show();

                    this.Hide();
                }
                else
                {
                    lblMensagem.Text = string.IsNullOrWhiteSpace(mensagemErro)
                        ? "Acesso negado."
                        : mensagemErro;
                }
            }
            finally
            {
                btnEntrar.Enabled = true;
                btnEntrar.Text = textoOriginal;
                Cursor = Cursors.Default;
            }
        }

        private void linkEntrarOffline_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            if (!SQLiteDatabase.ValidarSenhaLocal(email, txtSenha.Text, out var usuarioParaEntrar)
                || usuarioParaEntrar == null)
            {
                lblMensagem.Text = "Login ou senha inválidos para acesso offline.";
                return;
            }

            UsuarioSession.IsModoOffline = true;
            UsuarioSession.UsuarioLogado = usuarioParaEntrar;

            MessageBox.Show(
                $"Acesso concedido como '{usuarioParaEntrar.Nome}' ({usuarioParaEntrar.Login}) em Modo Offline.\n\n" +
                "Você pode utilizar o sistema com os dados salvos localmente no SQLite.",
                "Modo Offline Ativado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            HomeForm home = new HomeForm();
            home.FormClosed += (s, args) => Application.Exit();
            home.Show();

            this.Hide();
        }

        private void linkCadastro_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CadastroFuncionarioFormNovo cadastro = new CadastroFuncionarioFormNovo();
            cadastro.ShowDialog();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.Shown += (s, e) =>
            {
                ActiveControl = null;
            };
        }
    }
}
