using Guna.UI2.WinForms;

namespace ProjetoBandejao.Forms
{
    partial class ConfirmarCodigoForm
    {
        private TextBox txtEmail = null!;
        private MaskedTextBox mskCodigo = null!;
        private Guna2Button btnConfirmar = null!;
        private LinkLabel linkReenviar = null!;
        private Label status = null!;

        private void InitializeComponent()
        {
            Text = "Confirmar e-mail";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ClientSize = new Size(448, 430);
            MaximizeBox = MinimizeBox = false;
            BackColor = Color.FromArgb(249, 244, 246);
            Font = new Font("Segoe UI", 10);
            Padding = new Padding(22);

            var card = new Guna2Panel { Dock = DockStyle.Fill, FillColor = Color.White, BorderRadius = 16, Padding = new Padding(20) };
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 9, BackColor = Color.White };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            foreach (int altura in new[] { 41, 48, 26, 38, 26, 45, 44, 28 })
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, altura));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.Controls.Add(Rotulo("Confirme seu e-mail", 19, true), 0, 0);
            layout.Controls.Add(Rotulo("Digite o código de seis dígitos. Se ele não chegar, solicite o reenvio.", 10), 0, 1);
            layout.Controls.Add(Rotulo("E-mail", 9, true), 0, 2);
            txtEmail = new TextBox { Dock = DockStyle.Top, AccessibleName = "E-mail" };
            layout.Controls.Add(txtEmail, 0, 3);
            layout.Controls.Add(Rotulo("Código de confirmação", 9, true), 0, 4);
            mskCodigo = new MaskedTextBox("000000")
            {
                Width = 142, Font = new Font("Segoe UI", 17, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center, AccessibleName = "Código de seis dígitos"
            };
            layout.Controls.Add(mskCodigo, 0, 5);
            btnConfirmar = new Guna2Button
            {
                Dock = DockStyle.Fill, Text = "Confirmar e-mail", ForeColor = Color.White, BorderRadius = 8,
                FillColor = Color.FromArgb(107, 29, 47), Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            layout.Controls.Add(btnConfirmar, 0, 6);
            linkReenviar = new LinkLabel
            {
                Dock = DockStyle.Fill, Text = "Solicitar outro código", LinkColor = Color.FromArgb(107, 29, 47),
                TextAlign = ContentAlignment.MiddleCenter
            };
            layout.Controls.Add(linkReenviar, 0, 7);
            status = Rotulo("", 9);
            layout.Controls.Add(status, 0, 8);
            card.Controls.Add(layout);
            Controls.Add(card);
            AcceptButton = btnConfirmar;
        }

        private static Label Rotulo(string texto, int tamanho, bool destaque = false) => new()
        {
            Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Text = texto,
            ForeColor = Color.FromArgb(77, 40, 54), Font = new Font("Segoe UI", tamanho, destaque ? FontStyle.Bold : FontStyle.Regular)
        };
    }
}
