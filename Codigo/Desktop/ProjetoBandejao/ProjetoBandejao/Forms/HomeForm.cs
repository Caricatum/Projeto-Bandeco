using ProjetoBandejao.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoBandejao.Forms
{
    public partial class HomeForm : Form
    {
        public HomeForm()
        {
            InitializeComponent();
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            lblUsuario.Text =
             UsuarioSession.UsuarioLogado.Login;

            this.Shown += (s, e) =>
            {
                ActiveControl = null;
            };
        }

        private void pnlFuncionarios_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnCardapio_Click(object sender, EventArgs e)
        {

        }
    }
}
