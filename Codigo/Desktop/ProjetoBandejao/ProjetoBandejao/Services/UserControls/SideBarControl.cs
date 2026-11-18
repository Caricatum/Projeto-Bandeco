using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using ProjetoBandejao.Models;

namespace ProjetoBandejao.Services.UserControls
{
    public partial class SideBarControl : UserControl
    {
        public event EventHandler? OnHomeClick;
        public event EventHandler? OnRefeicoesClick;
        public event EventHandler? OnCardapioClick;
        public event EventHandler? OnFuncionariosClick;
        public event EventHandler? OnMuralClick;
        public event EventHandler? OnPerfilClick;
        private readonly Guna2Button perfil;
        private int versaoPerfil;

        public SideBarControl()
        {
            InitializeComponent();
            
            // Wire up the button click events
            btnHome.Click += (s, e) => OnHomeClick?.Invoke(this, EventArgs.Empty);
            btnRefeicoes.Click += (s, e) => OnRefeicoesClick?.Invoke(this, EventArgs.Empty);
            btnCardapio.Click += (s, e) => OnCardapioClick?.Invoke(this, EventArgs.Empty);
            btnFuncionarios.Click += (s, e) => OnFuncionariosClick?.Invoke(this, EventArgs.Empty);
            btnMural.Click += (s, e) => OnMuralClick?.Invoke(this, EventArgs.Empty);
            btnRelatorios.Dispose();
            btnConfiguracoes.Dispose();
            pnlInfo.Dispose();
            lblNome.Dispose();
            guna2HtmlLabel6.Dispose();
            guna2CirclePictureBox1.Dispose();
            guna2Panel3.Dispose();

            perfil = new Guna2Button
            {
                Text = "Meu perfil", AccessibleName = "Abrir meu perfil", TextAlign = HorizontalAlignment.Left,
                TextOffset = new Point(8, 0), Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(107, 29, 47), FillColor = Color.FromArgb(248, 238, 242),
                BorderRadius = 10, Cursor = Cursors.Hand, Size = new Size(210, 46),
                Location = new Point(20, pnlSideBar.Height - 120), Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            perfil.Click += (_, _) => OnPerfilClick?.Invoke(this, EventArgs.Empty);
            pnlSideBar.Controls.Add(perfil);
            Disposed += (_, _) => perfil.Image?.Dispose();
            guna2Button1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            guna2Button1.Click += (_, _) => FindForm()?.Close();
        }

        public async void MostrarPerfil(Usuario? usuario)
        {
            int versao = ++versaoPerfil;
            string nome = string.IsNullOrWhiteSpace(usuario?.Nome) ? "Meu perfil" : usuario.Nome.Trim();
            perfil.Text = nome.Length > 25 ? nome[..24] + "…" : nome;
            perfil.AccessibleName = $"Abrir perfil de {nome}";
            if (usuario == null) return;
            var foto = await FotoUsuario.CarregarAsync(usuario);
            if (IsDisposed || versao != versaoPerfil) { foto?.Dispose(); return; }
            var anterior = perfil.Image;
            perfil.Image = foto;
            perfil.ImageSize = new Size(30, 30);
            perfil.ImageAlign = HorizontalAlignment.Left;
            perfil.TextOffset = new Point(foto == null ? 8 : 14, 0);
            anterior?.Dispose();
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e) { }
        private void btnLogo_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel3_Click(object sender, EventArgs e) { }
        private void btnRefeicoes_Click(object sender, EventArgs e) { }
        private void btnCardapio_Click(object sender, EventArgs e) { }
        private void pnlSideBar_Paint(object sender, PaintEventArgs e) { }
    }
}
