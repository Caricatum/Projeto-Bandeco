using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjetoBandejao.Models;
using ProjetoBandejao.Services;

namespace ProjetoBandejao.Forms
{
    public partial class CadastroFuncionarioForm : Form
    {
        public CadastroFuncionarioForm()
        {
            InitializeComponent();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (txtNome.Text.Trim() == "" ||
                 txtEmail.Text.Trim() == "" ||
                 txtSenha.Text.Trim() == "")
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
                Funcionario = chkAdministrador.Checked
            };

            UsuarioService service = new UsuarioService();

            bool sucesso = service.Cadastrar(usuario);

            if (sucesso)
            {
                MessageBox.Show(
                          "Funcionário cadastrado com sucesso!\nAgora confirme o e-mail.",
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
                MessageBox.Show(
                    "Erro ao cadastrar funcionário.",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CadastroFuncionarioForm_Load(object sender, EventArgs e)
        {
            this.Shown += (s, e) =>
            {
                ActiveControl = null;
            };
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
