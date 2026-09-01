using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using ProjetoBandejao.Models;
using ProjetoBandejao.Services;

namespace ProjetoBandejao.Forms.Home
{
    public partial class MuralForm : Form
    {
        private readonly AvisoService avisoService = new AvisoService();
        private List<Aviso> listaAvisosAtual = new List<Aviso>();

        public MuralForm()
        {
            InitializeComponent();
            ConfigurarEventos();
            CarregarAvisos();
        }

        private void ConfigurarEventos()
        {
            btnCancelar.Click += (s, e) =>
            {
                txtNovoTitulo.Clear();
                txtMensagem.Clear();
                if (cmbTipo.Items.Count > 0) cmbTipo.SelectedIndex = 0;
            };

            txtBusca.TextChanged += (s, e) => AplicarFiltros();
            cmbCategoriaBusca.SelectedIndexChanged += (s, e) => AplicarFiltros();
            cmbFiltroRecentes.SelectedIndexChanged += (s, e) => AplicarFiltros();
        }

        private void CarregarAvisos()
        {
            listaAvisosAtual = avisoService.Listar();
            AplicarFiltros();
        }

        private void AplicarFiltros()
        {
            flpMural.Controls.Clear();

            string busca = txtBusca.Text.Trim();
            IEnumerable<Aviso> filtrados = listaAvisosAtual;

            if (!string.IsNullOrEmpty(busca))
            {
                filtrados = filtrados.Where(a => 
                    (a.Titulo != null && a.Titulo.Contains(busca, StringComparison.OrdinalIgnoreCase)) ||
                    (a.Descricao != null && a.Descricao.Contains(busca, StringComparison.OrdinalIgnoreCase))
                );
            }

            // Ordenação por data
            bool maisRecente = cmbFiltroRecentes.SelectedIndex <= 0;
            if (maisRecente)
            {
                filtrados = filtrados.OrderByDescending(a => a.DataCriacao ?? DateTime.MinValue);
            }
            else
            {
                filtrados = filtrados.OrderBy(a => a.DataCriacao ?? DateTime.MinValue);
            }

            var listaFinal = filtrados.ToList();

            if (listaFinal.Count == 0)
            {
                var lblVazio = new Label
                {
                    Text = "Nenhum aviso publicado no mural.\nUse o formulário ao lado para criar o primeiro aviso!",
                    Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                    ForeColor = Color.DarkSlateGray,
                    AutoSize = false,
                    Size = new Size(680, 100),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(20)
                };
                flpMural.Controls.Add(lblVazio);
                return;
            }

            Color[] coresPostIt = new Color[]
            {
                Color.FromArgb(253, 233, 144), // Amarelo
                Color.FromArgb(218, 232, 252), // Azul
                Color.FromArgb(248, 206, 220), // Rosa
                Color.FromArgb(213, 232, 212), // Verde
                Color.FromArgb(225, 213, 231)  // Roxo
            };

            int i = 0;
            foreach (var aviso in listaFinal)
            {
                Color cor = coresPostIt[i % coresPostIt.Length];
                AdicionarPostIt(aviso, cor);
                i++;
            }
        }

        private void AdicionarPostIt(Aviso aviso, Color cor)
        {
            var pnl = new Guna2Panel
            {
                Width = 220,
                Height = 230,
                FillColor = cor,
                BorderRadius = 4,
                Margin = new Padding(10)
            };

            // Alfinete (Pushpin)
            var pin = new Guna2CirclePictureBox
            {
                FillColor = Color.Crimson,
                Size = new Size(14, 14),
                Location = new Point(103, -7),
                BackColor = Color.Transparent
            };
            pnl.Controls.Add(pin);
            pin.BringToFront();

            // Botão Excluir pequeno
            var btnDel = new Guna2Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                FillColor = Color.Transparent,
                ForeColor = Color.FromArgb(150, 40, 40),
                Size = new Size(24, 24),
                Location = new Point(190, 5),
                Cursor = Cursors.Hand
            };
            btnDel.Click += (s, e) =>
            {
                var confirm = MessageBox.Show($"Deseja excluir o aviso '{aviso.Titulo}'?", "Excluir Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    if (avisoService.Deletar(aviso.Id))
                    {
                        CarregarAvisos();
                    }
                }
            };
            pnl.Controls.Add(btnDel);

            var lblTitulo = new Label
            {
                Text = aviso.Titulo,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(10, 25),
                Size = new Size(200, 45),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.TopCenter,
                ForeColor = Color.Black
            };
            pnl.Controls.Add(lblTitulo);

            var lblMsg = new Label
            {
                Text = aviso.Descricao,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Location = new Point(10, 75),
                Size = new Size(200, 110),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.TopCenter,
                ForeColor = Color.FromArgb(50, 50, 50)
            };
            pnl.Controls.Add(lblMsg);

            var lblData = new Label
            {
                Text = "📅 " + aviso.DataFormatada,
                Font = new Font("Segoe UI", 8, FontStyle.Regular),
                Location = new Point(10, 205),
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = Color.DimGray
            };
            pnl.Controls.Add(lblData);

            flpMural.Controls.Add(pnl);
        }

        private void btnPublicar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNovoTitulo.Text))
            {
                MessageBox.Show("Por favor, informe o título do aviso.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMensagem.Text))
            {
                MessageBox.Show("Por favor, escreva a mensagem do aviso.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Aviso novoAviso = new Aviso
            {
                Titulo = txtNovoTitulo.Text.Trim(),
                Descricao = txtMensagem.Text.Trim(),
                User = UsuarioSession.UsuarioLogado
            };

            bool sucesso = avisoService.Cadastrar(novoAviso);
            if (sucesso)
            {
                MessageBox.Show("Aviso publicado com sucesso no mural!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNovoTitulo.Clear();
                txtMensagem.Clear();
                CarregarAvisos();
            }
        }
    }
}
