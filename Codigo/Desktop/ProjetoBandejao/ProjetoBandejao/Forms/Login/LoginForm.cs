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

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            string emailDigitado = txtEmail.Text.Trim();
            string senhaDigitada = txtSenha.Text;

            if (string.IsNullOrWhiteSpace(emailDigitado))
            {
                lblMensagem.Text = "Por favor, informe seu usuário ou e-mail.";
                return;
            }

            Usuario usuario = new Usuario
            {
                Login = emailDigitado,
                Senha = senhaDigitada
            };

            UsuarioService service = new UsuarioService();
            string mensagemErro;
            bool isOffline;

            bool loginValido = service.ValidarLogin(usuario, out mensagemErro, out isOffline);

            if (loginValido)
            {
                UsuarioSession.IsModoOffline = isOffline;
                var usuarioCompleto = service.ObterPorLogin(usuario.Login);
                UsuarioSession.UsuarioLogado = usuarioCompleto ?? usuario;

                if (isOffline)
                {
                    MessageBox.Show(
                        "O servidor da API não respondeu. O acesso foi concedido em Modo Offline pelo banco de dados local SQLite.\n\n" +
                        "Você pode navegar pelo sistema, consultar funcionários e utilizar o Cardápio Semanal.",
                        "Modo Offline Ativado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
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

        private void linkEntrarOffline_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UsuarioService service = new UsuarioService();
            string email = txtEmail.Text.Trim();
            
            Usuario? usuarioParaEntrar = null;
            if (!string.IsNullOrWhiteSpace(email))
            {
                usuarioParaEntrar = service.ObterPorLogin(email);
            }

            if (usuarioParaEntrar == null)
            {
                var todos = SQLiteDatabase.ObterFuncionarios();
                usuarioParaEntrar = todos.FirstOrDefault() ?? new Usuario
                {
                    Id = 1,
                    Login = "admin@cotil.unicamp.br",
                    Nome = "Administrador (Local)",
                    Funcionario = true,
                    EmailConfirmado = true
                };
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

        private void linkEsqueciSenha_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RecuperarSenhaForm tela = new RecuperarSenhaForm();
            tela.ShowDialog();
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
