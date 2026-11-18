using ProjetoBandejao.Services.UserControls;

namespace ProjetoBandejao.Forms
{
    partial class HomeForm
    {
        private SideBarControl sidebar = null!;
        private Panel pnlTop = null!;
        private Panel pnlMain = null!;

        private void InitializeComponent()
        {
            SuspendLayout();
            sidebar = new SideBarControl { Dock = DockStyle.Left, Width = 250, Name = "sidebar" };
            pnlTop = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.White, Name = "pnlTop" };
            pnlMain = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(248, 244, 246), Name = "pnlMain" };
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 750);
            MinimumSize = new Size(1024, 690);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bandeco · Restaurante Universitário";
            Controls.Add(pnlMain);
            Controls.Add(pnlTop);
            Controls.Add(sidebar);
            ResumeLayout();
        }
    }
}
