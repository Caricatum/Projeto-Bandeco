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
    public partial class RecuperarSenhaForm : Form
    {
        public RecuperarSenhaForm()
        {
            InitializeComponent();
        }

        private void linkLblVoltar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void btnEnviarCodigo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show(
                    "Digite um e-mail.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            UsuarioService service =
                new UsuarioService();

            bool sucesso =
                service.SolicitarResetSenha(
                    txtEmail.Text.Trim(),
                    out string mensagemErro);

            if (sucesso)
            {
                MessageBox.Show(
                    "Código enviado com sucesso!\n\nVerifique sua caixa de entrada e também a pasta de Spam/Lixo Eletrônico.\nO código é válido por 15 minutos.",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ResetarSenhaForm tela =
                    new ResetarSenhaForm(txtEmail.Text.Trim());

                tela.ShowDialog();

                this.Close();
            }
            else
            {
                string detalhe = string.IsNullOrWhiteSpace(mensagemErro)
                    ? "Não foi possível enviar o código de verificação."
                    : mensagemErro;

                MessageBox.Show(
                    $"Erro ao solicitar redefinição de senha:\n{detalhe}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void RecuperarSenhaForm_Load(object sender, EventArgs e)
        {
            this.Shown += (s, e) =>
            {
                ActiveControl = null;
            };
        }
    }
}
