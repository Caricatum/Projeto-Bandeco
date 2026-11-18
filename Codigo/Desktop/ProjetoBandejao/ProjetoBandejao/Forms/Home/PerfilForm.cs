using Guna.UI2.WinForms;
using ProjetoBandejao.Models;
using ProjetoBandejao.Services;

namespace ProjetoBandejao.Forms.Home
{
    public sealed class PerfilForm : Form
    {
        public PerfilForm(Usuario usuario)
        {
            string nome = string.IsNullOrWhiteSpace(usuario.Nome) ? usuario.Login : usuario.Nome.Trim();
            string iniciais = string.Concat(nome.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Take(2).Select(parte => char.ToUpperInvariant(parte[0])));
            Text = "Meu perfil";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = MinimizeBox = false;
            ClientSize = new Size(460, 355);
            BackColor = Color.FromArgb(248, 244, 246);
            Padding = new Padding(20);

            var cartao = new Guna2Panel
            {
                Dock = DockStyle.Fill, FillColor = Color.White, BorderRadius = 16, Padding = new Padding(20)
            };
            var corpo = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 7, BackColor = Color.White };
            foreach (int altura in new[] { 54, 42, 29, 36, 36, 36 })
                corpo.RowStyles.Add(new RowStyle(SizeType.Absolute, altura));
            corpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var avatar = new Guna2Panel
            {
                Size = new Size(50, 50), BorderRadius = 25, FillColor = Color.FromArgb(107, 29, 47),
                Margin = Padding.Empty
            };
            var rotuloIniciais = new Label
            {
                Text = iniciais, Font = new Font("Segoe UI", 15, FontStyle.Bold), ForeColor = Color.White,
                Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent
            };
            avatar.Controls.Add(rotuloIniciais);
            var imagem = new Guna2CirclePictureBox { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom, Visible = false };
            avatar.Controls.Add(imagem);
            corpo.Controls.Add(avatar, 0, 0);
            var rotuloNome = Rotulo(nome, 19, true);
            corpo.Controls.Add(rotuloNome, 0, 1);
            corpo.Controls.Add(Rotulo(PermissoesDesktop.Papel(usuario), 10, true), 0, 2);
            corpo.Controls.Add(Rotulo($"E-mail  •  {usuario.Login}", 10), 0, 3);
            corpo.Controls.Add(Rotulo($"Setor  •  {usuario.Setor}", 10), 0, 4);
            corpo.Controls.Add(Rotulo($"Conta  •  {usuario.StatusFormatado}", 10), 0, 5);
            var fechar = new Guna2Button
            {
                Text = "Fechar", DialogResult = DialogResult.OK, Anchor = AnchorStyles.Right,
                Size = new Size(96, 32), BorderRadius = 8, FillColor = Color.FromArgb(107, 29, 47),
                ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            var acoes = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            var editar = new Guna2Button
            {
                Text = "Editar perfil", Size = new Size(116, 32), BorderRadius = 8,
                FillColor = Color.FromArgb(230, 242, 233), ForeColor = Color.FromArgb(27, 90, 56),
                Font = new Font("Segoe UI", 9, FontStyle.Bold), Enabled = PermissoesDesktop.EhContaAtual(usuario.Id)
            };
            acoes.Controls.AddRange([fechar, editar]);
            corpo.Controls.Add(acoes, 0, 6);
            cartao.Controls.Add(corpo);
            Controls.Add(cartao);
            AcceptButton = CancelButton = fechar;
            Image? foto = null;
            int versaoFoto = 0;
            async Task AtualizarFotoAsync()
            {
                int versao = ++versaoFoto;
                var carregada = await FotoUsuario.CarregarAsync(usuario);
                if (IsDisposed || versao != versaoFoto) { carregada?.Dispose(); return; }
                var anterior = foto;
                foto = carregada;
                imagem.Image = foto;
                imagem.Visible = foto != null;
                if (foto != null) imagem.BringToFront();
                anterior?.Dispose();
            }
            Shown += async (_, _) => await AtualizarFotoAsync();
            editar.Click += async (_, _) =>
            {
                using var editor = new EditarFuncionarioForm(usuario, new UsuarioService());
                if (editor.ShowDialog(this) != DialogResult.OK || UsuarioSession.UsuarioLogado is not { } atualizado) return;
                usuario = atualizado;
                rotuloNome.Text = atualizado.Nome;
                rotuloIniciais.Text = string.Concat(atualizado.Nome.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Take(2).Select(parte => char.ToUpperInvariant(parte[0])));
                await AtualizarFotoAsync();
            };
            Disposed += (_, _) => foto?.Dispose();
        }

        private static Label Rotulo(string texto, float tamanho, bool destaque = false) =>
            new()
            {
                Text = texto, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, AutoEllipsis = true,
                ForeColor = destaque ? Color.FromArgb(74, 31, 48) : Color.FromArgb(94, 75, 83),
                Font = new Font("Segoe UI", tamanho, destaque ? FontStyle.Bold : FontStyle.Regular)
            };
    }
}
