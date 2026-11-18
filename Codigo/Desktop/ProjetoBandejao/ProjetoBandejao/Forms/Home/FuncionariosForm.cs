using ProjetoBandejao.Data;
using ProjetoBandejao.Forms.Login;
using ProjetoBandejao.Models;
using ProjetoBandejao.Services;

namespace ProjetoBandejao.Forms.Home
{
    public partial class FuncionariosForm : Form
    {
        private readonly UsuarioService usuarioService = new();
        private List<Usuario> usuarios = [];
        private int versao;
        private bool operacaoEmAndamento;

        public FuncionariosForm()
        {
            InitializeComponent();
            MinimumSize = Size.Empty;
            BackColor = Color.FromArgb(249, 244, 246);
            lblTitle.ForeColor = Color.FromArgb(107, 29, 47);
            iconHeader.Dispose();
            lblTitle.Left = lblSubtitle.Left = 30;
            lblSubtitle.Text = "Consulte as contas e seus tipos de acesso.";
            btnCadastrar.Text = "Cadastrar funcionário";
            btnRefresh.Text = "Atualizar";
            int ajuste = Math.Max(0, 78 - btnRefresh.Width);
            btnRefresh.Left -= ajuste;
            btnRefresh.Width += ajuste;
            cbFilter.Left -= ajuste;
            btnPrev.Visible = btnPage1.Visible = btnNext.Visible = false;
            btnCadastrar.FillColor = btnPage1.FillColor = Color.FromArgb(107, 29, 47);
            colAvatar.HeaderText = "Foto";
            colAvatar.ImageLayout = DataGridViewImageCellLayout.Zoom;
            colAvatar.DefaultCellStyle.NullValue = null;
            colCargo.HeaderText = "Acesso";
            colSetor.Visible = false;
            colTelefone.Visible = false;
            colEdit.HeaderText = "Editar";
            colDelete.HeaderText = "Excluir";
            colEdit.FlatStyle = colDelete.FlatStyle = FlatStyle.Flat;
            colEdit.SortMode = colDelete.SortMode = DataGridViewColumnSortMode.NotSortable;
            colEdit.Visible = colDelete.Visible = btnCadastrar.Visible = PermissoesDesktop.PodeAdministrarContas;
            cbFilter.Items.Clear();
            cbFilter.Items.AddRange(["Todas as contas", "Funcionários", "Administradores", "Outras contas"]);
            cbFilter.SelectedIndex = 0;
            dgvFuncionarios.CellContentClick += async (_, e) => await AcionarLinhaAsync(e);
            txtSearch.TextChanged += (_, _) => AplicarFiltros();
            cbFilter.SelectedIndexChanged += (_, _) => AplicarFiltros();
            btnRefresh.Click += async (_, _) => await CarregarAsync();
            btnCadastrar.Click += btnCadastrar_Click;
            Load += async (_, _) => await CarregarAsync();
            Disposed += (_, _) => LiberarAvatares();
        }

        private async Task CarregarAsync()
        {
            int leitura = ++versao;
            var cache = await Task.Run(SQLiteDatabase.ObterFuncionarios);
            if (IsDisposed || leitura != versao) return;
            usuarios = cache;
            AplicarFiltros();
            var atualizados = await Task.Run(usuarioService.ListarTodosAsync);
            if (IsDisposed || leitura != versao) return;
            usuarios = atualizados;
            AplicarFiltros();
        }

        private void AplicarFiltros()
        {
            string busca = txtSearch.Text.Trim();
            int filtro = cbFilter.SelectedIndex;
            var encontrados = usuarios.Where(usuario =>
                (filtro switch
                {
                    1 => usuario.Funcionario && !PermissoesDesktop.EhAdministrador(usuario),
                    2 => PermissoesDesktop.EhAdministrador(usuario),
                    3 => !usuario.Funcionario,
                    _ => true
                })
                && (busca.Length == 0 || usuario.Nome.Contains(busca, StringComparison.CurrentCultureIgnoreCase)
                    || usuario.Login.Contains(busca, StringComparison.OrdinalIgnoreCase)
                    || PermissoesDesktop.Papel(usuario).Contains(busca, StringComparison.CurrentCultureIgnoreCase))).ToList();
            LiberarAvatares();
            dgvFuncionarios.Rows.Clear();
            lblTotal.Text = $"{encontrados.Count} conta(s)";
            foreach (var usuario in encontrados)
            {
                int indice = dgvFuncionarios.Rows.Add(null, usuario.Nome, usuario.Login, PermissoesDesktop.Papel(usuario), null,
                    usuario.Telefone, usuario.Id < 0 ? "Cadastro local" : usuario.StatusFormatado, "Editar", "Excluir");
                var linha = dgvFuncionarios.Rows[indice];
                linha.Tag = usuario;
                if (!string.IsNullOrWhiteSpace(usuario.Imagem)) _ = PreencherFotoAsync(linha, usuario);
            }
        }

        private async Task PreencherFotoAsync(DataGridViewRow linha, Usuario usuario)
        {
            var foto = await FotoUsuario.CarregarAsync(usuario);
            if (foto == null) return;
            if (IsDisposed || linha.DataGridView != dgvFuncionarios)
            {
                foto.Dispose();
                return;
            }
            linha.Cells[colAvatar.Index].Value = foto;
        }

        private void LiberarAvatares()
        {
            if (dgvFuncionarios.IsDisposed) return;
            foreach (DataGridViewRow linha in dgvFuncionarios.Rows)
                if (linha.Cells[colAvatar.Index].Value is Image foto) foto.Dispose();
        }

        private async Task AcionarLinhaAsync(DataGridViewCellEventArgs e)
        {
            if (!PermissoesDesktop.PodeAdministrarContas || operacaoEmAndamento || e.RowIndex < 0 || e.ColumnIndex < 0
                || dgvFuncionarios.Rows[e.RowIndex].Tag is not Usuario usuario) return;

            if (e.ColumnIndex == colEdit.Index)
            {
                using var edicao = new EditarFuncionarioForm(usuario, usuarioService);
                if (edicao.ShowDialog(this) == DialogResult.OK && !IsDisposed) await CarregarAsync();
            }
            else if (e.ColumnIndex == colDelete.Index)
            {
                if (usuario.Id == UsuarioSession.UsuarioLogado?.Id)
                {
                    MessageBox.Show(this, "A conta em uso não pode ser excluída nesta sessão.", "Excluir conta");
                    return;
                }
                if (MessageBox.Show(this, $"Excluir a conta de {usuario.Nome}?", "Excluir conta",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                operacaoEmAndamento = true;
                try
                {
                    bool excluido = await Task.Run(() => usuarioService.Deletar(usuario.Id));
                    if (excluido && !IsDisposed) await CarregarAsync();
                }
                finally { operacaoEmAndamento = false; }
            }
        }

        private async void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (!PermissoesDesktop.PodeAdministrarContas) return;
            using var cadastro = new CadastroFuncionarioFormNovo();
            if (cadastro.ShowDialog(this) == DialogResult.OK && !IsDisposed) await CarregarAsync();
        }

        private void dgvFuncionarios_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Graphics == null
                || dgvFuncionarios.Rows[e.RowIndex].Tag is not Usuario usuario) return;
            if (e.ColumnIndex == colEdit.Index || e.ColumnIndex == colDelete.Index)
            {
                e.PaintBackground(e.CellBounds, true);
                bool excluir = e.ColumnIndex == colDelete.Index;
                bool propriaConta = excluir && PermissoesDesktop.EhContaAtual(usuario.Id);
                var retangulo = new Rectangle(e.CellBounds.X + 4, e.CellBounds.Y + 11,
                    Math.Max(1, e.CellBounds.Width - 8), Math.Max(1, e.CellBounds.Height - 22));
                using var fundoAcao = new SolidBrush(propriaConta ? Color.FromArgb(231, 229, 230)
                    : excluir ? Color.FromArgb(158, 36, 49) : Color.FromArgb(27, 108, 67));
                e.Graphics.FillRectangle(fundoAcao, retangulo);
                TextRenderer.DrawText(e.Graphics, excluir ? "Excluir" : "Editar", e.CellStyle.Font ?? dgvFuncionarios.Font,
                    retangulo, propriaConta ? Color.DimGray : Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
                e.Paint(e.ClipBounds, DataGridViewPaintParts.Focus);
                e.Handled = true;
                return;
            }
            if (e.ColumnIndex != colAvatar.Index || e.Value is Image) return;
            e.PaintBackground(e.CellBounds, true);
            string iniciais = string.Concat(usuario.Nome.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Take(2).Select(parte => char.ToUpperInvariant(parte[0])));
            var area = new Rectangle(e.CellBounds.X + (e.CellBounds.Width - 34) / 2,
                e.CellBounds.Y + (e.CellBounds.Height - 34) / 2, 34, 34);
            using var fundo = new SolidBrush(Color.FromArgb(245, 230, 237));
            e.Graphics.FillEllipse(fundo, area);
            TextRenderer.DrawText(e.Graphics, iniciais, dgvFuncionarios.Font, area, Color.FromArgb(107, 29, 47),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            e.Handled = true;
        }
    }
}
