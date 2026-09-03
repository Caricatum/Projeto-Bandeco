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

        private void ConfirmarCodigoForm_Load(object sender, EventArgs e)
        {
            this.Shown += (s, e) =>
            {
                ActiveControl = null;
            };
        }
    }
}
