using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace ProjetoBandejao.Forms
{
    public partial class HomeForm : Form
    {
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

            // Mock Data
            AddAtividade("Cardápio do dia 23/05 cadastrado", "há 20 min");
            AddAtividade("Estoque de Arroz atualizado", "há 1 hora");
            AddAtividade("Novo Funcionário cadastrado", "há 2 horas");
            AddAtividade("Relatório gerado com sucesso", "há 3 horas");

            AddMural("Reunião da equipe será feita às 16h.", "23/05/2025 - 15:30");
            AddMural("Fornecedor de verduras atrasado.", "23/05/2025 - 11:15");
            AddMural("RU fechado no dia 31/05 e 01/06.", "23/05/2025 - 10:45");

            AddEstoqueAlert("Arroz", "Min: 20 kg", "12 kg");
            AddEstoqueAlert("Feijão", "Min: 15 kg", "9 kg");
            AddEstoqueAlert("Óleo", "Min: 10 L", "5 L");
            
            // Relógio
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += (s, e) => { lblTopTime.Text = "🕒 " + DateTime.Now.ToString("HH:mm"); };
            timer.Start();
            lblTopTime.Text = "🕒 " + DateTime.Now.ToString("HH:mm");
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
            pnl.BringToFront(); // Ensures list stacks correctly from top to bottom
        }

        private void AddMural(string title, string desc)
        {
            var pnl = new Panel { Height = 50, Dock = DockStyle.Top };
            var icon = new Label { Text = "📢", Font = new Font("Segoe UI", 12), AutoSize = true, Location = new Point(5, 5), ForeColor = Color.CornflowerBlue };
            var lblTitle = new Label { Text = title, Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true, Location = new Point(35, 5), ForeColor = Color.Black };
            var lblDesc = new Label { Text = desc, Font = new Font("Segoe UI", 8, FontStyle.Regular), AutoSize = true, Location = new Point(35, 22), ForeColor = Color.Gray };
            
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

        private void HomeForm_Load(object sender, EventArgs e)
        {
        }
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
