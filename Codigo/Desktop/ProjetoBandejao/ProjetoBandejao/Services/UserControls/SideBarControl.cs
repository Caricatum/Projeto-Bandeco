using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoBandejao.Services.UserControls
{
    public partial class SideBarControl : UserControl
    {
        public event EventHandler OnHomeClick;
        public event EventHandler OnRefeicoesClick;
        public event EventHandler OnCardapioClick;
        public event EventHandler OnFuncionariosClick;
        public event EventHandler OnMuralClick;
        public event EventHandler OnRelatoriosClick;
        public event EventHandler OnConfiguracoesClick;

        public SideBarControl()
        {
            InitializeComponent();
            
            // Wire up the button click events
            btnHome.Click += (s, e) => OnHomeClick?.Invoke(this, EventArgs.Empty);
            btnRefeicoes.Click += (s, e) => OnRefeicoesClick?.Invoke(this, EventArgs.Empty);
            btnCardapio.Click += (s, e) => OnCardapioClick?.Invoke(this, EventArgs.Empty);
            btnFuncionarios.Click += (s, e) => OnFuncionariosClick?.Invoke(this, EventArgs.Empty);
            btnMural.Click += (s, e) => OnMuralClick?.Invoke(this, EventArgs.Empty);
            btnRelatorios.Click += (s, e) => OnRelatoriosClick?.Invoke(this, EventArgs.Empty);
            btnConfiguracoes.Click += (s, e) => OnConfiguracoesClick?.Invoke(this, EventArgs.Empty);
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e) { }
        private void btnLogo_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel3_Click(object sender, EventArgs e) { }
        private void btnRefeicoes_Click(object sender, EventArgs e) { }
        private void btnCardapio_Click(object sender, EventArgs e) { }
        private void pnlSideBar_Paint(object sender, PaintEventArgs e) { }
    }
}
