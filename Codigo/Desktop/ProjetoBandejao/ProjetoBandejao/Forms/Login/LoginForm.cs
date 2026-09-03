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
            Usuario usuario = new Usuario
            {
                Login = txtEmail.Text.Trim(),
                Senha = txtSenha.Text
            };

            UsuarioService service =
                new UsuarioService();

            string mensagemErro;

            bool loginValido =
                service.ValidarLogin(usuario, out mensagemErro);

            if (loginValido)
            {
                UsuarioSession.UsuarioLogado =
                    usuario;

                HomeForm home =
                    new HomeForm();

                home.Show();

                this.Hide();
            }
            else
            {
                lblMensagem.Text =
                    string.IsNullOrWhiteSpace(mensagemErro)
                        ? "Acesso negado."
                        : mensagemErro;
            }
        }

        private void linkCadastro_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CadastroFuncionarioForm cadastro = new CadastroFuncionarioForm();

            cadastro.ShowDialog();
        }

        private void linkEsqueciSenha_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RecuperarSenhaForm tela =
                new RecuperarSenhaForm();

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
