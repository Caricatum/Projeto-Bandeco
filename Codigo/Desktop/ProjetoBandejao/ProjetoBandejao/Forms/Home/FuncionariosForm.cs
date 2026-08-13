using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using ProjetoBandejao.Models;

namespace ProjetoBandejao.Forms.Home
{
    public partial class FuncionariosForm : Form
    {
        public FuncionariosForm()
        {
            InitializeComponent();
            ConfigurarCoresUnicamp();
            CarregarDadosFalsos();
        }

        private void ConfigurarCoresUnicamp()
        {
            Color unicampRed = Color.FromArgb(179, 0, 0); // Vermelho escuro Unicamp

            // Estilizando botões principais
            btnCadastrar.FillColor = unicampRed;
            btnCadastrar.ForeColor = Color.White;
            
            // Paginação ativa
            btnPage1.FillColor = unicampRed;
            btnPage1.ForeColor = Color.White;

            // Cores do DataGrid (Cabeçalho)
            dgvFuncionarios.ColumnHeadersDefaultCellStyle.ForeColor = unicampRed;
            dgvFuncionarios.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White;
        }

        private void CarregarDadosFalsos()
        {
            dgvFuncionarios.Rows.Clear();
            dgvFuncionarios.Rows.Add(null, "Guilherme Rodrigues", "guilherme", "Administrador", "Administração", "(19) 99999-9999", "Ativo", null, null);
            dgvFuncionarios.Rows.Add(null, "Mariana Silva", "mariana.s", "Nutricionista", "Nutrição", "(19) 98888-8888", "Ativo", null, null);
            dgvFuncionarios.Rows.Add(null, "Lucas Pereira", "lucas.p", "Cozinheiro", "Cozinha", "(19) 97777-7777", "Ativo", null, null);
            dgvFuncionarios.Rows.Add(null, "Juliana Costa", "juliana.c", "Auxiliar de Cozinha", "Cozinha", "(19) 96666-6666", "Ativo", null, null);
            dgvFuncionarios.Rows.Add(null, "Rafael Almeida", "rafael.a", "Atendente", "Atendimento", "(19) 95555-5555", "Ativo", null, null);
        }

        private void dgvFuncionarios_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Customizar a célula de status (badge)
            if (e.ColumnIndex == colStatus.Index && e.RowIndex >= 0 && e.Value != null)
            {
                e.PaintBackground(e.CellBounds, true);
                
                string text = e.Value.ToString();
                Color bgColor = Color.FromArgb(235, 255, 240); // Fundo verde claro (pode trocar se quiser algo da Unicamp)
                Color textColor = Color.FromArgb(40, 167, 69); // Texto verde

                // Se quiser alterar o status ativo pra algo da Unicamp
                // bgColor = Color.FromArgb(255, 230, 230);
                // textColor = Color.FromArgb(179, 0, 0);

                Rectangle badgeRect = new Rectangle(e.CellBounds.X + 10, e.CellBounds.Y + 10, e.CellBounds.Width - 20, e.CellBounds.Height - 20);
                
                using (var brush = new SolidBrush(bgColor))
                {
                    e.Graphics.FillRoundedRectangle(brush, badgeRect, 10);
                }

                TextRenderer.DrawText(e.Graphics, text, dgvFuncionarios.Font, badgeRect, textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                
                e.Handled = true;
            }
        }
    }

    // Extensão para desenhar cantos arredondados (simplificada para o exemplo)
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
