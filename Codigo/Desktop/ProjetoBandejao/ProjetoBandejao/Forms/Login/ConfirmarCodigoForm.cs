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
    public partial class ConfirmarCodigoForm : Form
    {
        public ConfirmarCodigoForm(string email)
        {
            InitializeComponent();

            txtEmail.Text = email;
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show(
                    "Digite o e-mail.",
                    "Atenção",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!mskCodigo.MaskCompleted)
            {
                MessageBox.Show(
                    "Digite o código completo.",
                    "Atenção",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            UsuarioService service =
                new UsuarioService();

            string codigo =
                mskCodigo.Text.Replace(" ", "").Trim();

            string mensagemErro;

            bool sucesso =
                service.ConfirmarEmail(
                    txtEmail.Text.Trim(),
                    codigo,
                    out mensagemErro);

            if (sucesso)
            {
                MessageBox.Show(
                    "E-mail confirmado com sucesso!",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.Close();
            }
            else
            {
                MessageBox.Show(
                    string.IsNullOrWhiteSpace(mensagemErro)
                        ? "Não foi possível confirmar o e-mail."
                        : mensagemErro,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void linkReenviar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Informe o e-mail para reenviar o código.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UsuarioService service = new UsuarioService();
            bool sucesso = service.ReenviarCodigoCadastro(txtEmail.Text.Trim(), out string mensagemRetorno);

            if (sucesso)
            {
                MessageBox.Show(
                    mensagemRetorno + "\n\nVerifique sua caixa de entrada e também a pasta de Spam/Lixo Eletrônico.",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    string.IsNullOrWhiteSpace(mensagemRetorno) ? "Erro ao reenviar código." : mensagemRetorno,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfirmarCodigoForm_Load(object sender, EventArgs e)
        {
            this.Shown += (s, e) =>
            {
                ActiveControl = null;
            };
        }
    }
}
