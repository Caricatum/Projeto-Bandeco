using Guna.UI2.WinForms;
using ProjetoBandejao.Data;
using ProjetoBandejao.Forms.Home;
using ProjetoBandejao.Services;

namespace ProjetoBandejao.Forms
{
    public partial class HomeForm : Form
    {
        private static readonly Color Vinho = Color.FromArgb(107, 29, 47);
        private static readonly Color Texto = Color.FromArgb(64, 36, 47);
        private readonly Label conexao = new();
        private readonly Panel dashboard = new();
        private readonly TableLayoutPanel avisos = new();
        private Form? tela;
        private int versaoAvisos;

        public HomeForm()
        {
            InitializeComponent();
            ConfigurarTopo();
            CriarDashboard();
            ConfigurarNavegacao();
            Shown += async (_, _) =>
            {
                AtualizarConexao(!UsuarioSession.IsModoOffline);
                SyncService.OnStatusChanged += ReceberStatus;
                SyncService.OnSyncCompleted += ReceberSincronizacao;
                SyncService.Iniciar(15);
                await AtualizarAvisosAsync();
            };
            FormClosed += (_, _) =>
            {
                SyncService.OnStatusChanged -= ReceberStatus;
                SyncService.OnSyncCompleted -= ReceberSincronizacao;
                SyncService.Parar();
            };
        }

        private void ConfigurarTopo()
        {
            var titulo = Rotulo("Restaurante Universitário", 11, Texto, FontStyle.Bold);
            titulo.Dock = DockStyle.Left;
            titulo.Width = 250;
            titulo.Padding = new Padding(22, 0, 0, 0);
            pnlTop.Controls.Add(titulo);

            conexao.Dock = DockStyle.Right;
            conexao.Width = 154;
            conexao.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            conexao.TextAlign = ContentAlignment.MiddleCenter;
            pnlTop.Controls.Add(conexao);
        }

        private void CriarDashboard()
        {
            var usuario = UsuarioSession.UsuarioLogado;
            string saudacao = string.IsNullOrWhiteSpace(usuario?.Nome) ? "Bem-vindo" : $"Olá, {usuario.Nome.Trim()}";
            sidebar.MostrarPerfil(usuario);
            dashboard.Dock = DockStyle.Fill;
            dashboard.AutoScroll = true;
            dashboard.Padding = new Padding(24);
            pnlMain.Controls.Add(dashboard);
            var layout = new TableLayoutPanel { Dock = DockStyle.Top, Height = 464, ColumnCount = 1, RowCount = 3 };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 126));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            dashboard.Controls.Add(layout);

            var boasVindas = new Guna2Panel
            {
                Dock = DockStyle.Fill, Margin = new Padding(0, 0, 0, 12),
                FillColor = Vinho, BorderRadius = 15, Padding = new Padding(23, 14, 23, 12)
            };
            var data = Rotulo(DateTime.Today.ToString("dddd, dd 'de' MMMM", new System.Globalization.CultureInfo("pt-BR")), 10,
                Color.FromArgb(244, 212, 224));
            data.Dock = DockStyle.Bottom;
            data.Height = 28;
            var nome = Rotulo(saudacao, 24, Color.White, FontStyle.Bold);
            boasVindas.Controls.Add(nome);
            boasVindas.Controls.Add(data);
            layout.Controls.Add(boasVindas, 0, 0);

            var atalhos = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, Margin = Padding.Empty };
            atalhos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            atalhos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            atalhos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34f));
            atalhos.Controls.Add(Atalho("Refeições", () => AbrirTela(new frmRefeicoes())), 0, 0);
            atalhos.Controls.Add(Atalho("Cardápio do dia", () => AbrirTela(new CardapioForm())), 1, 0);
            atalhos.Controls.Add(Atalho("Funcionários", () => AbrirTela(new FuncionariosForm())), 2, 0);
            layout.Controls.Add(atalhos, 0, 1);

            var mural = new Guna2Panel
            {
                Dock = DockStyle.Fill, Margin = new Padding(0, 5, 0, 0), Padding = new Padding(18, 10, 18, 10),
                FillColor = Color.White, BorderRadius = 14, BorderThickness = 1, BorderColor = Color.FromArgb(230, 221, 224)
            };
            var abrirMural = Atalho("Abrir mural", () => AbrirTela(new MuralForm()));
            abrirMural.Dock = DockStyle.Bottom;
            abrirMural.Height = 36;
            var titulo = Rotulo("Mural de avisos", 13, Texto, FontStyle.Bold);
            titulo.Dock = DockStyle.Top;
            titulo.Height = 36;
            avisos.Dock = DockStyle.Fill;
            avisos.ColumnCount = 1;
            avisos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            mural.Controls.Add(avisos);
            mural.Controls.Add(abrirMural);
            mural.Controls.Add(titulo);
            layout.Controls.Add(mural, 0, 2);
        }

        private void ConfigurarNavegacao()
        {
            sidebar.OnHomeClick += (_, _) => AbrirTela(null);
            sidebar.OnRefeicoesClick += (_, _) => AbrirTela(new frmRefeicoes());
            sidebar.OnCardapioClick += (_, _) => AbrirTela(new CardapioForm());
            sidebar.OnFuncionariosClick += (_, _) => AbrirTela(new FuncionariosForm());
            sidebar.OnMuralClick += (_, _) => AbrirTela(new MuralForm());
            sidebar.OnPerfilClick += (_, _) =>
            {
                if (UsuarioSession.UsuarioLogado is not { } usuario) return;
                using var perfil = new PerfilForm(usuario);
                perfil.ShowDialog(this);
                sidebar.MostrarPerfil(UsuarioSession.UsuarioLogado);
            };
        }

        private void AbrirTela(Form? novaTela)
        {
            tela?.Close();
            tela = novaTela;
            dashboard.Visible = tela == null;
            if (tela == null)
            {
                _ = AtualizarAvisosAsync();
                return;
            }

            tela.TopLevel = false;
            tela.FormBorderStyle = FormBorderStyle.None;
            tela.Dock = DockStyle.Fill;
            pnlMain.Controls.Add(tela);
            tela.BringToFront();
            tela.Show();
        }

        private void ReceberStatus(bool online)
        {
            if (IsHandleCreated && !IsDisposed) BeginInvoke(() => AtualizarConexao(online));
        }

        private void ReceberSincronizacao(string mensagem)
        {
            if (IsHandleCreated && !IsDisposed) BeginInvoke(async () => await AtualizarAvisosAsync());
        }

        private void AtualizarConexao(bool online)
        {
            conexao.Text = online ? "● Conectado" : "● Modo offline";
            conexao.ForeColor = online ? Color.FromArgb(31, 105, 64) : Color.FromArgb(138, 88, 21);
        }

        private async Task AtualizarAvisosAsync()
        {
            int versao = ++versaoAvisos;
            var recentes = await Task.Run(() => SQLiteDatabase.ObterAvisos().Take(3).ToList());
            if (IsDisposed || versao != versaoAvisos) return;
            avisos.SuspendLayout();
            try
            {
                foreach (Control aviso in avisos.Controls.Cast<Control>().ToArray()) aviso.Dispose();
                avisos.RowStyles.Clear();
                avisos.RowCount = Math.Max(1, recentes.Count);
                if (recentes.Count == 0)
                {
                    avisos.Controls.Add(Rotulo("Nenhum aviso publicado.", 10, Color.DimGray), 0, 0);
                    return;
                }
                for (int i = 0; i < recentes.Count; i++)
                {
                    avisos.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / recentes.Count));
                    var aviso = recentes[i];
                    var linha = Rotulo($"{aviso.Titulo}  ·  {aviso.Descricao}", 10, Texto);
                    linha.BackColor = Color.FromArgb(248, 244, 246);
                    linha.Padding = new Padding(10, 0, 8, 0);
                    avisos.Controls.Add(linha, 0, i);
                }
            }
            finally { avisos.ResumeLayout(); }
        }

        private static Label Rotulo(string texto, float tamanho, Color cor, FontStyle estilo = FontStyle.Regular) =>
            new()
            {
                Text = texto, ForeColor = cor, Font = new Font("Segoe UI", tamanho, estilo), Dock = DockStyle.Fill,
                AutoEllipsis = true, TextAlign = ContentAlignment.MiddleLeft, BackColor = Color.Transparent
            };

        private static Guna2Button Atalho(string texto, Action abrir)
        {
            var botao = new Guna2Button
            {
                Text = texto, Dock = DockStyle.Fill, BorderRadius = 10, Cursor = Cursors.Hand,
                FillColor = Color.FromArgb(245, 233, 238), ForeColor = Vinho,
                Margin = new Padding(0, 0, 8, 8), Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            botao.Click += (_, _) => abrir();
            return botao;
        }
    }
}
