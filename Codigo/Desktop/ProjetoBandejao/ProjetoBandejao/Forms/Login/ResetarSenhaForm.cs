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
    public partial class ResetarSenhaForm : Form
    {
        private string email;

        public ResetarSenhaForm(string email)
        {
            InitializeComponent();

            this.email = email;
        }

        private void btnAlterarSenha_Click(object sender, EventArgs e)
        {
            if (!txtCodigo.MaskCompleted)
            {
                MessageBox.Show(
                    "Digite o código completo.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(
                txtNovaSenha.Text))
            {
                MessageBox.Show(
                    "Digite a nova senha.");

                return;
            }

            if (txtNovaSenha.Text !=
                txtConfirmarSenha.Text)
            {
                MessageBox.Show(
                    "As senhas não coincidem.");

                return;
            }

            UsuarioService service =
                new UsuarioService();

            bool sucesso =
                service.ResetarSenha(
                    txtEmail.Text,
                    txtCodigo.Text,
                    txtNovaSenha.Text);

            if (sucesso)
            {
                MessageBox.Show(
                    "Senha alterada com sucesso.",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.Close();
            }
            else
            {
                MessageBox.Show(
                    "Código inválido ou expirado.",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ResetarSenhaForm_Load(object sender, EventArgs e)
        {
            txtEmail.Text = email;
            this.Shown += (s, e) =>
            {
                ActiveControl = null;
            };
        }

        private void linkReenviar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UsuarioService service =
             new UsuarioService();

            bool sucesso =
                service.SolicitarResetSenha(
                    txtEmail.Text);

            if (sucesso)
            {
                MessageBox.Show(
                    "Novo código enviado.");
            }
            else
            {
                MessageBox.Show(
                    "Erro ao reenviar.");
            }
        }
    }
}
