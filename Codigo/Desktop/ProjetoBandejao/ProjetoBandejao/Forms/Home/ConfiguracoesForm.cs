using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using System.Collections.Generic;

namespace ProjetoBandejao.Forms.Home
{
    public partial class ConfiguracoesForm : Form
    {
        private List<Guna2Button> navButtons = new List<Guna2Button>();
        private List<Guna2Panel> sections = new List<Guna2Panel>();
        private bool isScrollingProgrammatically = false;

        public ConfiguracoesForm()
        {
            InitializeComponent();
            
            // Build references for ScrollSpy and Navigation
            navButtons.Add(btnNavUsuarios);
            navButtons.Add(btnNavCardapio);
            navButtons.Add(btnNavMural);
            navButtons.Add(btnNavNotificacoes);
            navButtons.Add(btnNavFeedback);
            navButtons.Add(btnNavEstoque);
            navButtons.Add(btnNavSistema);
            navButtons.Add(btnNavBackup);
            navButtons.Add(btnNavSobre);

            sections.Add(pnlSecaoUsuarios);
            sections.Add(pnlSecaoCardapio);
            sections.Add(pnlSecaoMural);
            sections.Add(pnlSecaoNotificacoes);
            sections.Add(pnlSecaoFeedback);
            sections.Add(pnlSecaoEstoque);
            sections.Add(pnlSecaoSistema);
            sections.Add(pnlSecaoBackup);
            sections.Add(pnlSecaoSobre);
            
            pnlContent.Scroll += PnlContent_Scroll;
            pnlContent.MouseWheel += PnlContent_Scroll;
            
            if (navButtons.Count > 0)
                SetActiveButton(navButtons[0]);
        }

        private void ConfiguracoesForm_Load(object? sender, EventArgs e)
        {
            pnlContent.AutoScrollPosition = new Point(0, 0);
        }

        private void btnNav_Click(object? sender, EventArgs e)
        {
            if (sender is Guna2Button btn && btn.Tag is Guna2Panel section)
            {
                isScrollingProgrammatically = true;
                SetActiveButton(btn);
                
                Point targetPoint = new Point(0, section.Top);
                pnlContent.AutoScrollPosition = targetPoint;
                
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = 100 };
                timer.Tick += (s, ev) => 
                {
                    isScrollingProgrammatically = false;
                    timer.Stop();
                    timer.Dispose();
                };
                timer.Start();
            }
        }

        private void PnlContent_Scroll(object? sender, EventArgs e)
        {
            if (isScrollingProgrammatically) return;

            int scrollY = -pnlContent.AutoScrollPosition.Y;
            int middleScreen = scrollY + (pnlContent.Height / 3);

            for (int i = sections.Count - 1; i >= 0; i--)
            {
                var section = sections[i];
                if (section.Top <= middleScreen)
                {
                    SetActiveButton(navButtons[i]);
                    break;
                }
            }
        }

        private void SetActiveButton(Guna2Button activeBtn)
        {
            foreach (var btn in navButtons)
            {
                if (btn == activeBtn)
                {
                    btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                    btn.ForeColor = Color.Black;
                    btn.FillColor = Color.FromArgb(235, 235, 235);
                }
                else
                {
                    btn.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
                    btn.ForeColor = Color.Gray;
                    btn.FillColor = Color.Transparent;
                }
            }
        }
    }
}
