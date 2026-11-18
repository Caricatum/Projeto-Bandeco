using Guna.UI2.WinForms;

namespace ProjetoBandejao.Services
{
    partial class CardRefeicaoControl
    {
        private PictureBox pctImage = null!;
        private Label lblSemFoto = null!;
        private Label lblTitle = null!;
        private Label lblDescription = null!;
        private Label lblCategoria = null!;
        private Label[] valoresNutricionais = null!;
        private Guna2Button btnDetalhes = null!;
        private Guna2Button btnExcluir = null!;
        private Guna2Button btnAvaliacoes = null!;

        private void InitializeComponent()
        {
            SuspendLayout();
            var card = new Guna2Panel
            {
                Dock = DockStyle.Fill, FillColor = Color.White, BorderColor = Color.FromArgb(230, 221, 224),
                BorderThickness = 1, BorderRadius = 14, Padding = new Padding(14)
            };
            var colunas = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            colunas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27F));
            colunas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 73F));
            colunas.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var imagem = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0, 0, 14, 0), BackColor = Color.FromArgb(247, 240, 242) };
            pctImage = new PictureBox { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom, Visible = false, TabStop = false };
            lblSemFoto = CriarRotulo("Sem foto", 10, Color.FromArgb(116, 85, 98));
            lblSemFoto.TextAlign = ContentAlignment.MiddleCenter;
            imagem.Controls.Add(pctImage);
            imagem.Controls.Add(lblSemFoto);

            var conteudo = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 5, Margin = Padding.Empty };
            conteudo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            conteudo.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            conteudo.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));
            conteudo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            conteudo.RowStyles.Add(new RowStyle(SizeType.Absolute, 49));
            conteudo.RowStyles.Add(new RowStyle(SizeType.Absolute, 39));

            lblTitle = CriarRotulo("", 15, Color.FromArgb(58, 29, 40), FontStyle.Bold);
            lblCategoria = CriarRotulo("", 9, Color.FromArgb(121, 38, 64), FontStyle.Bold);
            lblDescription = CriarRotulo("", 9.5f, Color.FromArgb(89, 80, 84));
            conteudo.Controls.Add(lblTitle, 0, 0);
            conteudo.Controls.Add(lblCategoria, 0, 1);
            conteudo.Controls.Add(lblDescription, 0, 2);

            var nutricao = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 1, Margin = Padding.Empty };
            string[] nomes = ["Calorias", "Carboidratos", "Proteínas", "Gorduras"];
            valoresNutricionais = new Label[nomes.Length];
            for (int i = 0; i < nomes.Length; i++)
            {
                nutricao.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
                var indicador = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0, 0, 5, 0), BackColor = Color.FromArgb(248, 245, 246) };
                var legenda = CriarRotulo(nomes[i], 8, Color.FromArgb(94, 80, 87));
                legenda.Dock = DockStyle.Bottom;
                legenda.Height = 20;
                legenda.TextAlign = ContentAlignment.MiddleCenter;
                valoresNutricionais[i] = CriarRotulo("", 10, Color.FromArgb(58, 29, 40), FontStyle.Bold);
                valoresNutricionais[i].TextAlign = ContentAlignment.MiddleCenter;
                indicador.Controls.Add(valoresNutricionais[i]);
                indicador.Controls.Add(legenda);
                nutricao.Controls.Add(indicador, i, 0);
            }
            conteudo.Controls.Add(nutricao, 0, 3);

            var acoes = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, Margin = Padding.Empty };
            btnExcluir = CriarBotao("Excluir", Color.FromArgb(252, 237, 239), Color.FromArgb(134, 36, 55));
            btnDetalhes = CriarBotao("Editar", Color.FromArgb(107, 29, 47), Color.White);
            btnAvaliacoes = CriarBotao("Avaliações", Color.FromArgb(245, 233, 238), Color.FromArgb(107, 29, 47));
            btnAvaliacoes.Width = 108;
            acoes.Controls.Add(btnExcluir);
            acoes.Controls.Add(btnDetalhes);
            acoes.Controls.Add(btnAvaliacoes);
            conteudo.Controls.Add(acoes, 0, 4);

            colunas.Controls.Add(imagem, 0, 0);
            colunas.Controls.Add(conteudo, 1, 0);
            card.Controls.Add(colunas);
            Controls.Add(card);
            AutoScaleMode = AutoScaleMode.Font;
            Size = new Size(760, 230);
            ResumeLayout();
        }

        private static Label CriarRotulo(string texto, float tamanho, Color cor, FontStyle estilo = FontStyle.Regular) =>
            new() { Text = texto, Dock = DockStyle.Fill, AutoEllipsis = true, ForeColor = cor, Font = new Font("Segoe UI", tamanho, estilo) };

        private static Guna2Button CriarBotao(string texto, Color fundo, Color frente) =>
            new()
            {
                Text = texto, Size = new Size(92, 30), Margin = new Padding(4, 7, 0, 0), BorderRadius = 7,
                FillColor = fundo, ForeColor = frente, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand
            };
    }
}
