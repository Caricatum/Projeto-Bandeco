using System;
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
                Funcionario = chkAdministrador.Checked
            };

            UsuarioService service = new UsuarioService();

            bool sucesso = service.Cadastrar(usuario, out string mensagemErro);

            if (sucesso)
            {
                MessageBox.Show(
                    "Funcionário cadastrado no banco com sucesso!\nUm código de validação foi gerado para confirmação.",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ConfirmarCodigoForm tela = new ConfirmarCodigoForm(txtEmail.Text.Trim());
                tela.ShowDialog();

                this.Close();
            }
            else
            {
                string detalhe = string.IsNullOrWhiteSpace(mensagemErro) ? "Falha ao comunicar com o servidor da API." : mensagemErro;
                MessageBox.Show(
                    $"Erro ao cadastrar funcionário:\n{detalhe}",
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
