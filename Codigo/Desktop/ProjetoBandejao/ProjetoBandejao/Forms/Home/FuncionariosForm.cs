using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using ProjetoBandejao.Models;
using ProjetoBandejao.Services;

namespace ProjetoBandejao.Forms.Home
{
    public partial class FuncionariosForm : Form
    {
        private readonly UsuarioService usuarioService = new UsuarioService();
        private List<Usuario> listaUsuariosAtual = new List<Usuario>();

        public FuncionariosForm()
        {
            InitializeComponent();
            ConfigurarCoresUnicamp();
            ConfigurarEventos();
            CarregarFuncionarios();
        }

        private void ConfigurarCoresUnicamp()
        {
            Color unicampRed = Color.FromArgb(179, 0, 0);

            btnCadastrar.FillColor = unicampRed;
            btnCadastrar.ForeColor = Color.White;

            btnPage1.FillColor = unicampRed;
            btnPage1.ForeColor = Color.White;

            dgvFuncionarios.ColumnHeadersDefaultCellStyle.ForeColor = unicampRed;
            dgvFuncionarios.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White;
        }

        private void ConfigurarEventos()
        {
            btnCadastrar.Click += (s, e) =>
            {
                using var frmCad = new CadastroFuncionarioForm();
                frmCad.ShowDialog();
                CarregarFuncionarios();
            };

            btnRefresh.Click += (s, e) => CarregarFuncionarios();

            txtSearch.TextChanged += (s, e) => AplicarFiltros();
            cbFilter.SelectedIndexChanged += (s, e) => AplicarFiltros();

            dgvFuncionarios.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == colDelete.Index)
                {
                    var row = dgvFuncionarios.Rows[e.RowIndex];
                    string? login = row.Cells[colLogin.Index].Value?.ToString();
                    var user = listaUsuariosAtual.FirstOrDefault(u => u.Login == login);
                    if (user != null)
                    {
                        var confirm = MessageBox.Show($"Deseja realmente excluir o funcionário '{user.Nome}' ({user.Login})?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (confirm == DialogResult.Yes)
                        {
                            if (usuarioService.Deletar(user.Id))
                            {
                                MessageBox.Show("Funcionário excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                CarregarFuncionarios();
                            }
                        }
                    }
                }
            };
        }

        private void CarregarFuncionarios()
        {
            listaUsuariosAtual = usuarioService.ListarTodos();
            AplicarFiltros();
        }

        private void AplicarFiltros()
        {
            dgvFuncionarios.Rows.Clear();

            string busca = txtSearch.Text.Trim();
            IEnumerable<Usuario> filtrados = listaUsuariosAtual;

            if (!string.IsNullOrEmpty(busca))
            {
                filtrados = filtrados.Where(u =>
                    (u.Nome != null && u.Nome.Contains(busca, StringComparison.OrdinalIgnoreCase)) ||
                    (u.Login != null && u.Login.Contains(busca, StringComparison.OrdinalIgnoreCase))
                );
            }

            string filtroCargo = cbFilter.SelectedItem?.ToString() ?? "Todos os Cargos";
            if (filtroCargo != "Todos os Cargos" && !string.IsNullOrWhiteSpace(filtroCargo))
            {
                if (filtroCargo.Contains("Funcionário", StringComparison.OrdinalIgnoreCase) || filtroCargo.Contains("Administrador", StringComparison.OrdinalIgnoreCase))
                {
                    filtrados = filtrados.Where(u => u.Funcionario);
                }
            }

            var listaFinal = filtrados.ToList();
            lblTotal.Text = $"Total de {listaFinal.Count} funcionário{(listaFinal.Count == 1 ? "" : "s")}";

            foreach (var user in listaFinal)
            {
                string cargo = user.Funcionario ? "Funcionário" : "Cliente / Aluno";
                string setor = user.Funcionario ? "Administração" : "Acadêmico";
                string status = "Ativo";

                dgvFuncionarios.Rows.Add(
                    null,
                    user.Nome,
                    user.Login,
                    cargo,
                    setor,
                    "-",
                    status,
                    null,
                    null
                );
            }
        }

        private void dgvFuncionarios_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == colStatus.Index && e.RowIndex >= 0 && e.Value != null)
            {
                e.PaintBackground(e.CellBounds, true);

                string text = e.Value.ToString() ?? "Ativo";
                Color bgColor = Color.FromArgb(235, 255, 240);
                Color textColor = Color.FromArgb(40, 167, 69);

                Rectangle badgeRect = new Rectangle(e.CellBounds.X + 10, e.CellBounds.Y + 10, e.CellBounds.Width - 20, e.CellBounds.Height - 20);

                using (var brush = new SolidBrush(bgColor))
                {
                    e.Graphics?.FillRoundedRectangle(brush, badgeRect, 10);
                }

                TextRenderer.DrawText(e.Graphics, text, dgvFuncionarios.Font, badgeRect, textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                e.Handled = true;
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            CadastroFuncionarioForm cadastro = new CadastroFuncionarioForm();

            cadastro.ShowDialog();
        }
    }

    public static class GraphicsExtension
    {
        public static void FillRoundedRectangle(this Graphics graphics, Brush brush, Rectangle bounds, int cornerRadius)
        {
            if (graphics == null) throw new ArgumentNullException(nameof(graphics));
            if (brush == null) throw new ArgumentNullException(nameof(brush));

            using (System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddArc(bounds.X, bounds.Y, cornerRadius, cornerRadius, 180, 90);
                path.AddArc(bounds.X + bounds.Width - cornerRadius, bounds.Y, cornerRadius, cornerRadius, 270, 90);
                path.AddArc(bounds.X + bounds.Width - cornerRadius, bounds.Y + bounds.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
                path.AddArc(bounds.X, bounds.Y + bounds.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
                path.CloseFigure();
                graphics.FillPath(brush, path);
            }
        }
    }
}
