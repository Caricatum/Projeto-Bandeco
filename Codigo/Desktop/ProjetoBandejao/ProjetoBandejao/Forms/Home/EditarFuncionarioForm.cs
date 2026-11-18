using Guna.UI2.WinForms;
using ProjetoBandejao.Models;
using ProjetoBandejao.Services;

namespace ProjetoBandejao.Forms.Home
{
    internal sealed class EditarFuncionarioForm : Form
    {
        private readonly TextBox nome = new();
        private readonly CheckBox acesso = new() { Text = "Funcionário com acesso ao desktop", AutoSize = true };
        private readonly Label status = new() { Dock = DockStyle.Fill, AutoEllipsis = true };
        private readonly Guna2Button salvar = new() { Text = "Salvar", Dock = DockStyle.Fill, BorderRadius = 8, FillColor = Color.FromArgb(107, 29, 47) };
        private string? foto;

        public EditarFuncionarioForm(Usuario conta, UsuarioService service)
        {
            bool propriaConta = PermissoesDesktop.EhContaAtual(conta.Id);
            Text = propriaConta ? "Editar meu perfil" : "Editar funcionário";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = MinimizeBox = false;
            ClientSize = new Size(475, 270);
            Font = new Font("Segoe UI", 10);
            BackColor = Color.FromArgb(249, 244, 246);
            Padding = new Padding(22);
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 6 };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            foreach (int altura in new[] { 29, 33, propriaConta ? 0 : 33, 39, 43, 38 })
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, altura));

            layout.Controls.Add(new Label { Text = $"E-mail: {conta.Login}", Dock = DockStyle.Fill, AutoEllipsis = true }, 0, 0);
            nome.Text = conta.Nome;
            nome.AccessibleName = "Nome";
            nome.Dock = DockStyle.Top;
            layout.Controls.Add(nome, 0, 1);
            acesso.Checked = conta.Funcionario;
            acesso.Enabled = !propriaConta && PermissoesDesktop.PodeAdministrarContas;
            acesso.Visible = !propriaConta;
            layout.Controls.Add(acesso, 0, 2);
            var escolher = new Button { Text = "Escolher foto", Width = 158, Enabled = conta.Id > 0, AutoEllipsis = true };
            escolher.Click += (_, _) =>
            {
                using var dialogo = new OpenFileDialog { Filter = "Imagens JPEG ou PNG|*.jpg;*.jpeg;*.png" };
                if (dialogo.ShowDialog(this) != DialogResult.OK) return;
                if (new FileInfo(dialogo.FileName).Length > 5 * 1024 * 1024)
                {
                    status.Text = "A foto deve ter até 5 MB.";
                    return;
                }
                foto = dialogo.FileName;
                escolher.Text = Path.GetFileName(foto);
                status.Text = "";
            };
            layout.Controls.Add(escolher, 0, 3);
            layout.Controls.Add(status, 0, 4);
            layout.Controls.Add(salvar, 0, 5);
            Controls.Add(layout);
            AcceptButton = salvar;
            salvar.Click += async (_, _) =>
            {
                if (nome.Text.Trim().Length == 0)
                {
                    status.Text = "Informe o nome.";
                    return;
                }
                var atualizado = new Usuario
                {
                    Id = conta.Id, Login = conta.Login, Nome = nome.Text.Trim(), Funcionario = acesso.Checked,
                    MascaraRestricoes = conta.MascaraRestricoes
                };
                salvar.Enabled = false;
                try
                {
                    string? erro = await service.AtualizarAsync(atualizado, foto);
                    if (IsDisposed) return;
                    if (erro != null) { status.Text = erro; return; }
                    DialogResult = DialogResult.OK;
                    Close();
                }
                finally { if (!IsDisposed) salvar.Enabled = true; }
            };
        }
    }
}
