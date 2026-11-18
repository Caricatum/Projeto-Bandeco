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

            if (string.IsNullOrWhiteSpace(txtNovaSenha.Text))
            {
                MessageBox.Show(
                    "Digite a nova senha.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (txtNovaSenha.Text != txtConfirmarSenha.Text)
            {
                MessageBox.Show(
                    "As senhas não coincidem.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            UsuarioService service = new UsuarioService();

            bool sucesso = service.ResetarSenha(
                txtEmail.Text.Trim(),
                txtCodigo.Text,
                txtNovaSenha.Text,
                out string mensagemErro);

            if (sucesso)
            {
                MessageBox.Show(
                    "Senha alterada com sucesso! Você já pode realizar o login.",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.Close();
            }
            else
            {
                string detalhe = string.IsNullOrWhiteSpace(mensagemErro)
                    ? "Código inválido ou expirado."
                    : mensagemErro;

                MessageBox.Show(
                    $"Erro ao alterar senha:\n{detalhe}",
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
            UsuarioService service = new UsuarioService();

            bool sucesso = service.SolicitarResetSenha(
                txtEmail.Text.Trim(),
                out string mensagemErro);

            if (sucesso)
            {
                MessageBox.Show(
                    "Novo código enviado com sucesso!\n\nVerifique sua caixa de entrada e também a pasta de Spam/Lixo Eletrônico.",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                string detalhe = string.IsNullOrWhiteSpace(mensagemErro)
                    ? "Erro ao reenviar o código."
                    : mensagemErro;

                MessageBox.Show(
                    $"Não foi possível reenviar o código:\n{detalhe}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
