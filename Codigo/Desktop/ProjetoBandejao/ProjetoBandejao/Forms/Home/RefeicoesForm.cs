using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjetoBandejao.Services;
using ProjetoBandejao.Models;

namespace ProjetoBandejao.Forms.Home
{
    public partial class frmRefeicoes : Form
    {
        private FlowLayoutPanel flowLayoutPanelMeals;

        public frmRefeicoes()
        {
            InitializeComponent();
            InitializeMealsList();
        }

        private void InitializeMealsList()
        {
            // Create a FlowLayoutPanel to hold the cards
            flowLayoutPanelMeals = new FlowLayoutPanel();
            flowLayoutPanelMeals.AutoScroll = true;
            flowLayoutPanelMeals.Location = new Point(14, 60);
            flowLayoutPanelMeals.Size = new Size(880, 410); // Adjust as necessary
            flowLayoutPanelMeals.BackColor = Color.White;
            flowLayoutPanelMeals.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelMeals.WrapContents = false;
            
            // Add it to the panel
            guna2Panel2.Controls.Add(flowLayoutPanelMeals);

            CarregarPratos();
        }

        private void CarregarPratos()
        {
            flowLayoutPanelMeals.Controls.Clear();
            var service = new PratoService();
            var pratos = service.Listar();

            if (pratos == null || pratos.Count == 0)
            {
                // Mostra um aviso caso não tenha pratos
                Label lblSemPratos = new Label();
                lblSemPratos.Text = "Nenhum prato cadastrado ainda.";
                lblSemPratos.AutoSize = true;
                lblSemPratos.Font = new Font("Segoe UI", 12F, FontStyle.Italic);
                flowLayoutPanelMeals.Controls.Add(lblSemPratos);
                return;
            }

            foreach (var prato in pratos)
            {
                var card = new CardRefeicaoControl();
                card.PreencherDados(prato);
                // Adiciona margem entre os cards
                card.Margin = new Padding(0, 0, 0, 20);
                flowLayoutPanelMeals.Controls.Add(card);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
