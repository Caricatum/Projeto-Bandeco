using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using ProjetoBandejao.Services;

namespace ProjetoBandejao.Forms
{
    public partial class HomeForm : Form
    {
        private readonly AvisoService avisoService = new AvisoService();
        private readonly PratoService pratoService = new PratoService();

        public HomeForm()
        {
            InitializeComponent();
            SetupCustomUI();
        }

        private void SetupCustomUI()
        {
            // Substitui o painel de gráfico por um customizado responsivo
            var chartPanel = new SimpleChartControl
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            pnlChartContent.Controls.Add(chartPanel);

            // Carrega dados dinâmicos da API
            CarregarDadosDashboard();

        }

        private void CarregarDadosDashboard()
        {
            try
            {
                pnlMuralList.Controls.Clear();
                var avisos = avisoService.Listar().Take(4).ToList();
                if (avisos.Count > 0)
                {
                    foreach (var aviso in avisos)
                    {
                        AddMural(aviso.Titulo, aviso.Descricao);
                    }
                }
                else
                {
                    AddMural("Bem-vindo ao Sistema Bandeco!", "Todas as funcionalidades conectadas.");
                }

                pnlAtividadesList.Controls.Clear();
                var pratos = pratoService.Listar().Take(3).ToList();
                if (pratos.Count > 0)
                {
                    foreach (var prato in pratos)
                    {
                        AddAtividade($"Prato '{prato.Nome}' cadastrado", prato.CategoriaTexto);
                    }
                }
                else
                {
                    AddAtividade("Sistema iniciado com sucesso", "Agora");
                }

                AddEstoqueAlert("Arroz", "Min: 20 kg", "12 kg");
                AddEstoqueAlert("Feijão", "Min: 15 kg", "9 kg");
                AddEstoqueAlert("Óleo", "Min: 10 L", "5 L");
            }
            catch
            {
                // Fallback silencioso em caso de API offline temporariamente
            }
        }

        private void AddAtividade(string text, string time)
        {
            var pnl = new Panel { Height = 35, Dock = DockStyle.Top };
            var dot = new Label { Text = "•", ForeColor = Color.ForestGreen, Font = new Font("Segoe UI", 14, FontStyle.Bold), AutoSize = true, Location = new Point(5, 5) };
            var lblText = new Label { Text = text, Font = new Font("Segoe UI", 9, FontStyle.Regular), AutoSize = true, Location = new Point(25, 10), ForeColor = Color.DimGray };
            var lblTime = new Label { Text = time, Font = new Font("Segoe UI", 9, FontStyle.Regular), AutoSize = true, ForeColor = Color.Silver, Anchor = AnchorStyles.Top | AnchorStyles.Right };

            pnl.Controls.Add(dot);
            pnl.Controls.Add(lblText);
            pnl.Controls.Add(lblTime);
            pnlAtividadesList.Controls.Add(pnl);

            pnl.Resize += (s, e) => { lblTime.Left = pnl.Width - lblTime.Width - 10; lblTime.Top = 10; };
            lblTime.Left = pnl.Width - lblTime.Width - 10;
            pnl.BringToFront();
        }

        private void AddMural(string title, string desc)
        {
            var pnl = new Panel { Height = 50, Dock = DockStyle.Top };
            var icon = new Label { Text = "📢", Font = new Font("Segoe UI", 12), AutoSize = true, Location = new Point(5, 5), ForeColor = Color.CornflowerBlue };
            var lblTitle = new Label { Text = title, Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Location = new Point(35, 5), ForeColor = Color.Black };
            var lblDesc = new Label { Text = desc.Length > 45 ? desc.Substring(0, 42) + "..." : desc, Font = new Font("Segoe UI", 8, FontStyle.Regular), AutoSize = true, Location = new Point(35, 22), ForeColor = Color.Gray };

            pnl.Controls.Add(icon);
            pnl.Controls.Add(lblTitle);
            pnl.Controls.Add(lblDesc);
            pnlMuralList.Controls.Add(pnl);
            pnl.BringToFront();
        }

        private void AddEstoqueAlert(string item, string min, string current)
        {
            var pnl = new Panel { Height = 40, Dock = DockStyle.Top };
            var icon = new Label { Text = "⚠️", Font = new Font("Segoe UI", 11), AutoSize = true, Location = new Point(5, 10) };
            var lblItem = new Label { Text = item, Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Location = new Point(35, 8), ForeColor = Color.Black };
            var lblMin = new Label { Text = min, Font = new Font("Segoe UI", 8, FontStyle.Regular), AutoSize = true, Location = new Point(130, 10), ForeColor = Color.Gray };

            var badge = new Guna2Panel { BorderRadius = 5, FillColor = Color.FromArgb(255, 235, 235), Size = new Size(50, 25), Anchor = AnchorStyles.Top | AnchorStyles.Right };
            var lblCurrent = new Label { Text = current, ForeColor = Color.Crimson, Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = false, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent };
            badge.Controls.Add(lblCurrent);

            pnl.Controls.Add(icon);
            pnl.Controls.Add(lblItem);
            pnl.Controls.Add(lblMin);
            pnl.Controls.Add(badge);
            pnlEstoqueList.Controls.Add(pnl);

            pnl.Resize += (s, e) => { badge.Left = pnl.Width - badge.Width - 10; badge.Top = 7; };
            badge.Left = pnl.Width - badge.Width - 10;
            pnl.BringToFront();
        }

        private Form? activeForm = null;

        private void HomeForm_Load(object sender, EventArgs e)
        {
            sidebar.OnHomeClick += (s, ev) => OpenChildForm(null);
            sidebar.OnRefeicoesClick += (s, ev) => OpenChildForm(new ProjetoBandejao.Forms.Home.frmRefeicoes());
            sidebar.OnCardapioClick += (s, ev) => OpenChildForm(new ProjetoBandejao.Forms.Home.frmRefeicoes());
            sidebar.OnFuncionariosClick += (s, ev) => OpenChildForm(new ProjetoBandejao.Forms.Home.FuncionariosForm());
            sidebar.OnConfiguracoesClick += (s, ev) => OpenChildForm(new ProjetoBandejao.Forms.Home.ConfiguracoesForm());
            sidebar.OnMuralClick += (s, ev) => OpenChildForm(new ProjetoBandejao.Forms.Home.MuralForm());
            sidebar.OnRelatoriosClick += (s, ev) => OpenChildForm(new ProjetoBandejao.Forms.Home.frmRefeicoes());
        }

        private void OpenChildForm(Form? childForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
                activeForm = null;
            }

            // Esconde os controles padrão do dashboard se não for a Home
            bool isHome = childForm == null;
            foreach (Control ctrl in pnlMain.Controls)
            {
                if (ctrl is Form) continue;
                ctrl.Visible = isHome;
            }

            if (!isHome && childForm != null)
            {
                activeForm = childForm;
                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;
                pnlMain.Controls.Add(childForm);
                pnlMain.Tag = childForm;
                childForm.BringToFront();
                childForm.Show();
            }
            else if (isHome)
            {
                CarregarDadosDashboard();
            }
        }

        private void sidebar_Load(object sender, EventArgs e) { }
    }

    public class SimpleChartControl : Control
    {
        public SimpleChartControl()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            var g = e.Graphics;
            int w = Width;
            int h = Height;
            
            int padL = 30;
            int padB = 20;
            int padT = 10;
            int padR = 10;

            if (w <= padL + padR || h <= padB + padT) return;

            Pen gridPen = new Pen(Color.FromArgb(240, 240, 240), 1);
            Font font = new Font("Segoe UI", 8);
            Brush brush = new SolidBrush(Color.Gray);
            
            int numLines = 5;
            for (int i = 0; i <= numLines; i++)
            {
                int y = h - padB - (i * (h - padB - padT) / numLines);
                g.DrawLine(gridPen, padL, y, w - padR, y);
                g.DrawString((i * 100).ToString(), font, brush, 0, y - 6);
            }

            string[] days = { "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb", "Dom" };
            float step = (w - padL - padR) / 6f;
            for (int i = 0; i < days.Length; i++)
            {
                float x = padL + i * step;
                g.DrawString(days[i], font, brush, x - 10, h - padB + 2);
            }

            float[] values = { 150, 250, 350, 280, 220, 150, 50 };
            PointF[] points = new PointF[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                points[i] = new PointF(padL + i * step, h - padB - (values[i] / 500f) * (h - padB - padT));
            }

            PointF[] poly = new PointF[points.Length + 2];
            for (int i = 0; i < points.Length; i++) poly[i] = points[i];
            poly[points.Length] = new PointF(points[points.Length - 1].X, h - padB);
            poly[points.Length + 1] = new PointF(points[0].X, h - padB);

            using (LinearGradientBrush b = new LinearGradientBrush(new Point(0, padT), new Point(0, h - padB), Color.FromArgb(60, 34, 139, 34), Color.FromArgb(0, 34, 139, 34)))
            {
                g.FillPolygon(b, poly);
            }

            Pen linePen = new Pen(Color.ForestGreen, 2);
            g.DrawLines(linePen, points);

            Brush pointBrush = new SolidBrush(Color.ForestGreen);
            foreach (var pt in points)
            {
                g.FillEllipse(pointBrush, pt.X - 3, pt.Y - 3, 6, 6);
            }
        }
    }
}
