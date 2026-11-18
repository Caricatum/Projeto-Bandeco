using Guna.UI2.WinForms;
using ProjetoBandejao.Data;
using ProjetoBandejao.Models;
using ProjetoBandejao.Services;

namespace ProjetoBandejao.Forms.Home
{
    public partial class MuralForm : Form
    {
        private static readonly string[] tipos = ["Informativo", "Alerta", "Evento"];
        private readonly AvisoService servico = new();
        private readonly DateTimePicker inicio = new() { Format = DateTimePickerFormat.Short };
        private readonly DateTimePicker fim = new() { Format = DateTimePickerFormat.Short };
        private readonly Label status = new() { ForeColor = Color.FromArgb(107, 29, 47) };
        private List<Aviso> avisos = [];
        private Aviso? emEdicao;
        private int versao;

        public MuralForm()
        {
            InitializeComponent();
            MinimumSize = Size.Empty;
            AutoScroll = true;
            BackColor = Color.FromArgb(248, 244, 246);
            ConfigurarLayout();
            cmbCategoriaBusca.Items.Clear();
            cmbCategoriaBusca.Items.AddRange(["Todos os avisos", .. tipos]);
            cmbTipo.Items.Clear();
            cmbTipo.Items.AddRange(tipos);
            cmbTipo.SelectedIndex = cmbCategoriaBusca.SelectedIndex = cmbFiltroRecentes.SelectedIndex = 0;
            txtBusca.TextChanged += (_, _) => Renderizar();
            cmbCategoriaBusca.SelectedIndexChanged += (_, _) => Renderizar();
            cmbFiltroRecentes.SelectedIndexChanged += (_, _) => Renderizar();
            btnCancelar.Click += (_, _) => LimparEditor();
            Load += async (_, _) => await CarregarAsync();
            LimparEditor();
        }

        private void ConfigurarLayout()
        {
            lblTitulo.Text = "Mural de avisos";
            lblTitulo.ForeColor = Color.FromArgb(107, 29, 47);
            lblSubtitulo.Text = "Comunicados do restaurante";
            lblInfoRodape.Visible = false;
            btnPublicar.FillColor = Color.FromArgb(107, 29, 47);
            pnlCortica.FillColor = Color.White;
            pnlCortica.BackgroundImage = null;
            pnlCortica.Padding = new Padding(8);
            flpMural.Dock = DockStyle.Fill;
            flpMural.BackColor = Color.White;
            txtMensagem.Height = 86;
            var lblInicio = new Label { Text = "Início", Location = new Point(23, 333), AutoSize = true };
            var lblFim = new Label { Text = "Fim", Location = new Point(177, 333), AutoSize = true };
            inicio.SetBounds(23, 354, 139, 25);
            fim.SetBounds(177, 354, 140, 25);
            status.SetBounds(23, 386, 294, 48);
            pnlCriarAviso.Controls.AddRange([lblInicio, lblFim, inicio, fim, status]);

            var filtros = new TableLayoutPanel { ColumnCount = 3, RowCount = 1, Location = new Point(24, 94), Height = 43 };
            filtros.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 157));
            filtros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            filtros.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145));
            Control[] campos = [cmbCategoriaBusca, txtBusca, cmbFiltroRecentes];
            for (int i = 0; i < campos.Length; i++)
            {
                campos[i].Dock = DockStyle.Fill;
                campos[i].Margin = new Padding(3);
                filtros.Controls.Add(campos[i], i, 0);
            }
            Controls.Add(filtros);

            void Ajustar()
            {
                int largura = Math.Max(420, ClientSize.Width - 48);
                filtros.Width = largura;
                bool ladoALado = largura >= 670;
                int lista = ladoALado ? largura - 352 : largura;
                pnlCortica.SetBounds(24, 148, lista, ladoALado ? Math.Max(410, ClientSize.Height - 172) : 330);
                pnlCriarAviso.Location = ladoALado ? new Point(24 + lista + 12, 148) : new Point(24, pnlCortica.Bottom + 12);
                AutoScrollMinSize = new Size(0, pnlCriarAviso.Bottom + 20);
            }
            Resize += (_, _) => Ajustar();
            Ajustar();
        }

        private async Task CarregarAsync()
        {
            int leitura = ++versao;
            var cache = await Task.Run(SQLiteDatabase.ObterAvisos);
            if (IsDisposed || leitura != versao) return;
            avisos = cache;
            Renderizar();
            var remotos = await Task.Run(servico.ListarAsync);
            if (IsDisposed || leitura != versao) return;
            avisos = remotos;
            Renderizar();
        }

        private void Renderizar()
        {
            string busca = txtBusca.Text.Trim();
            string tipo = cmbCategoriaBusca.SelectedItem?.ToString() ?? "";
            var filtrados = avisos.Where(aviso =>
                (cmbCategoriaBusca.SelectedIndex <= 0 || tipo.Equals(aviso.Model, StringComparison.OrdinalIgnoreCase))
                && (busca.Length == 0 || aviso.Titulo.Contains(busca, StringComparison.CurrentCultureIgnoreCase)
                    || aviso.Descricao?.Contains(busca, StringComparison.CurrentCultureIgnoreCase) == true));
            var ordenados = cmbFiltroRecentes.SelectedIndex <= 0
                ? filtrados.OrderByDescending(aviso => aviso.DataInicial).ThenByDescending(aviso => aviso.Id)
                : filtrados.OrderBy(aviso => aviso.DataInicial).ThenBy(aviso => aviso.Id);
            flpMural.SuspendLayout();
            try
            {
                foreach (Control item in flpMural.Controls.Cast<Control>().ToArray()) item.Dispose();
                foreach (var aviso in ordenados) flpMural.Controls.Add(CriarCartao(aviso));
                if (flpMural.Controls.Count == 0)
                    flpMural.Controls.Add(new Label { Text = "Nenhum aviso encontrado.", AutoSize = true, Margin = new Padding(14) });
            }
            finally { flpMural.ResumeLayout(); }
        }

        private Control CriarCartao(Aviso aviso)
        {
            var painel = new Guna2Panel
            {
                Size = new Size(251, 173), Margin = new Padding(6), Padding = new Padding(11), BorderRadius = 10,
                FillColor = Color.FromArgb(249, 240, 244), BorderColor = Color.FromArgb(238, 224, 232), BorderThickness = 1
            };
            var titulo = new Label { Dock = DockStyle.Top, Height = 27, Text = aviso.Titulo, AutoEllipsis = true, Font = new Font("Segoe UI", 11, FontStyle.Bold) };
            var contexto = new Label
            {
                Dock = DockStyle.Top, Height = 28, Font = new Font("Segoe UI", 8),
                Text = $"{aviso.Model} · {aviso.DataInicial:dd/MM} a {aviso.DataFinal:dd/MM}" + (aviso.Id < 0 ? " · Pendente" : "")
            };
            var corpo = new Label { Dock = DockStyle.Fill, Text = aviso.Descricao, AutoEllipsis = true };
            var acoes = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 34, FlowDirection = FlowDirection.RightToLeft };
            var excluir = new Button { Text = "Excluir", AutoSize = true };
            excluir.Click += async (_, _) =>
            {
                if (MessageBox.Show(this, $"Excluir o aviso “{aviso.Titulo}”?", "Excluir aviso",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                excluir.Enabled = false;
                string? erro = await servico.DeletarAsync(aviso.Id);
                if (IsDisposed) return;
                if (erro == null) await CarregarAsync();
                else
                {
                    if (!excluir.IsDisposed) excluir.Enabled = true;
                    status.Text = erro;
                }
            };
            var editar = new Button { Text = "Editar", AutoSize = true };
            editar.Click += (_, _) =>
            {
                emEdicao = aviso;
                txtNovoTitulo.Text = aviso.Titulo;
                txtMensagem.Text = aviso.Descricao;
                int indice = cmbTipo.FindStringExact(aviso.Model);
                if (indice < 0 && !string.IsNullOrWhiteSpace(aviso.Model))
                    indice = cmbTipo.Items.Add(aviso.Model);
                cmbTipo.SelectedIndex = Math.Max(0, indice);
                inicio.Value = aviso.DataInicial ?? DateTime.Today;
                fim.Value = aviso.DataFinal ?? DateTime.Today.AddDays(30);
                lblCriarAviso.Text = "Editar aviso";
                btnPublicar.Text = "Salvar";
                status.Text = "";
                txtNovoTitulo.Focus();
            };
            acoes.Controls.AddRange([excluir, editar]);
            painel.Controls.AddRange([corpo, acoes, contexto, titulo]);
            return painel;
        }

        private void LimparEditor()
        {
            emEdicao = null;
            txtNovoTitulo.Clear();
            txtMensagem.Clear();
            cmbTipo.SelectedIndex = 0;
            inicio.Value = DateTime.Today;
            fim.Value = DateTime.Today.AddDays(30);
            lblCriarAviso.Text = "Novo aviso";
            btnPublicar.Text = "Publicar";
            status.Text = "";
        }

        private async void btnPublicar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNovoTitulo.Text) || string.IsNullOrWhiteSpace(txtMensagem.Text))
            {
                status.Text = "Preencha o título e a mensagem.";
                return;
            }
            if (fim.Value.Date < inicio.Value.Date)
            {
                status.Text = "A data final deve ser igual ou posterior à inicial.";
                return;
            }
            var aviso = new Aviso
            {
                Id = emEdicao?.Id ?? 0, Titulo = txtNovoTitulo.Text.Trim(), Descricao = txtMensagem.Text.Trim(),
                Model = cmbTipo.SelectedItem?.ToString() ?? tipos[0], User = emEdicao?.User ?? UsuarioSession.UsuarioLogado,
                DataInicial = inicio.Value.Date, DataFinal = fim.Value.Date, DataCriacao = emEdicao?.DataCriacao ?? DateTime.Now
            };
            btnPublicar.Enabled = false;
            try
            {
                var resultado = await servico.SalvarAsync(aviso);
                if (IsDisposed) return;
                if (resultado.Sucesso)
                {
                    LimparEditor();
                    await CarregarAsync();
                }
                if (!IsDisposed) status.Text = resultado.Mensagem;
            }
            finally { if (!IsDisposed) btnPublicar.Enabled = true; }
        }
    }
}
