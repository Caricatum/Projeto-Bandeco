using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace ProjetoBandejao.Forms.Home
{
    public partial class MuralForm : Form
    {
        public MuralForm()
        {
            InitializeComponent();
            CarregarPostItsExemplo();
        }

        private void CarregarPostItsExemplo()
        {
            // Criando exemplos de post-its
            AdicionarPostIt("Mudança no horário", "A partir de 15/08, o almoço será servido das 10h30 às 14h30.", "ALERTA", Color.FromArgb(253, 233, 144), "12/08/2025");
            AdicionarPostIt("Cardápio de Inverno!", "Pratos quentinhos para deixar seu dia mais gostoso.", "INFORMATIVO", Color.FromArgb(218, 232, 252), "11/08/2025");
            AdicionarPostIt("Não esqueça!", "Traga sua garrafinha reutilizável e ajude o meio ambiente.", "LEMBRETE", Color.FromArgb(248, 206, 220), "10/08/2025");
            AdicionarPostIt("Vamos juntos pelo Cotil!", "Desperdício de alimentos faz mal para o planeta. Sirva-se consciente!", "SUSTENTABILIDADE", Color.FromArgb(213, 232, 212), "09/08/2025");
            AdicionarPostIt("Semana do Estudante", "Atividades especiais de 18 a 22/08. Participe!", "EVENTO", Color.FromArgb(225, 213, 231), "08/08/2025");
            AdicionarPostIt("Fila preferencial", "Lembre-se: a fila preferencial é para idosos e gestantes.", "AVISO", Color.FromArgb(255, 242, 204), "07/08/2025");
        }

        private void AdicionarPostIt(string titulo, string mensagem, string tipo, Color cor, string data)
        {
            var pnl = new Guna2Panel
            {
                Width = 240,
                Height = 260,
                FillColor = cor,
                BorderRadius = 4,
                Margin = new Padding(15)
            };

            // Criar o "Chip" de categoria
            var chipPanel = new Guna2Panel
            {
                FillColor = Color.White,
                BorderColor = Color.Gainsboro,
                BorderThickness = 1,
                BorderRadius = 10,
                Size = new Size(110, 25),
                Location = new Point(115, 15)
            };
            
            var lblTipo = new Label
            {
                Text = tipo,
                Font = new Font("Segoe UI", 7F, FontStyle.Bold),
                ForeColor = Color.DimGray,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            chipPanel.Controls.Add(lblTipo);
            pnl.Controls.Add(chipPanel);

            // Alfinete (Pushpin)
            var pin = new Guna2CirclePictureBox
            {
                FillColor = Color.Crimson,
                Size = new Size(16, 16),
                Location = new Point(112, -8),
                BackColor = Color.Transparent
            };
            pnl.Controls.Add(pin);
            pin.BringToFront();

            var lblTitulo = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                Location = new Point(10, 55),
                Size = new Size(220, 60),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.TopCenter,
                ForeColor = Color.Black
            };
            pnl.Controls.Add(lblTitulo);

            var lblMsg = new Label
            {
                Text = mensagem,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Location = new Point(15, 120),
                Size = new Size(210, 100),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.TopCenter,
                ForeColor = Color.FromArgb(60, 60, 60)
            };
            pnl.Controls.Add(lblMsg);

            var lblData = new Label
            {
                Text = "📅 " + data,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Location = new Point(15, 230),
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = Color.DimGray
            };
            pnl.Controls.Add(lblData);

            flpMural.Controls.Add(pnl);
        }

        private void btnPublicar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Aviso publicado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
