using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using ProjetoBandejao.Models;
using ProjetoBandejao.Services;

namespace ProjetoBandejao.Forms.Home
{
    public partial class CardapioForm : Form
    {
        private readonly CardapioService _cardapioService = new CardapioService();
        private DateTime _dataReferencia = DateTime.Today;
        private List<DiaCardapio> _semana = new List<DiaCardapio>();
        private DiaCardapio _diaAtual = new DiaCardapio();
        private bool _isNoturno = false;
        private CancellationTokenSource? _consultaSemana;
        private readonly CultureInfo _ptBr = new CultureInfo("pt-BR");

        public CardapioForm()
        {
            InitializeComponent();
            ConfigurarEventos();
            AtualizarBotoesTurno();
            CarregarCardapio();
            Load += async (_, _) => await AtualizarCardapioDaApiAsync();
            Disposed += (_, _) => _consultaSemana?.Cancel();
        }

        private void ConfigurarEventos()
        {
            btnCardapioDiurno.Text = "Almoço";
            btnCardapioNoturno.Text = "Jantar";
            btnEditarCardapio.Text = "Editar cardápio";
            btnCopiarDia.Text = "Copiar dia";
            btnLimparDia.Text = "Limpar dia";
            pnlCircleIcon.Dispose();
            lblHeaderSubtitle.Visible = false;
            lblHeaderTitle.Left = 12;
            lblHeaderTitle.Top = 14;
            lblHeaderTitle.ForeColor = Color.FromArgb(107, 29, 47);
            lblIconNota.Text = "N";
            lblIconCafe.Text = "i";
            btnCardapioDiurno.Click += (s, e) =>
            {
                if (_isNoturno)
                {
                    _isNoturno = false;
                    AtualizarBotoesTurno();
                    RenderizarCardapioDia();
                }
            };

            btnCardapioNoturno.Click += (s, e) =>
            {
                if (!_isNoturno)
                {
                    _isNoturno = true;
                    AtualizarBotoesTurno();
                    RenderizarCardapioDia();
                }
            };

            btnMontarSemana.Click += (s, e) =>
            {
                string intervalo = _semana.Count > 0 ? $"{_semana.First().Data:dd/MM} a {_semana.Last().Data:dd/MM}" : "atual";
                var result = MessageBox.Show(
                    $"Deseja preencher automaticamente todos os dias úteis da semana ({intervalo}) com as sugestões padrão e opções veganas?",
                    "Montar Cardápio da Semana",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _semana.ForEach(d =>
                    {
                        if (d.DiaSemana != DayOfWeek.Saturday && d.DiaSemana != DayOfWeek.Sunday)
                        {
                            d.Definido = true;
                            _cardapioService.SalvarAlteracaoLocal(d);
                        }
                    });
                    RenderizarBarraSemana();
                    RenderizarCardapioDia();
                    MessageBox.Show("Cardápio semanal configurado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            btnEditarCardapio.Click += (s, e) =>
            {
                AbrirEditorDia();
            };

            var publicarDia = new Guna2Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BorderRadius = 6,
                FillColor = Color.FromArgb(107, 29, 47),
                Font = btnEditarCardapio.Font,
                Location = new Point(btnEditarCardapio.Left - 119, btnEditarCardapio.Top),
                Size = new Size(112, btnEditarCardapio.Height),
                Text = "Publicar dia"
            };
            publicarDia.Click += async (_, _) =>
            {
                var dia = _diaAtual;
                int usuarioId = UsuarioSession.UsuarioLogado?.Id ?? 0;
                if (usuarioId <= 0)
                {
                    MessageBox.Show(this, "Entre com uma conta cadastrada na API para publicar.", "Publicar dia");
                    return;
                }

                if (MessageBox.Show(this,
                    $"Publicar o cardápio de {dia.Data:dd/MM/yyyy} na API? A publicação inclui almoço e jantar. Pão e café ficam no desktop.",
                    "Publicar dia", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                publicarDia.Enabled = false;
                try
                {
                    await _cardapioService.PublicarDiaAsync(dia, usuarioId);
                    if (!IsDisposed)
                        MessageBox.Show(this, "Cardápio publicado.", "Publicar dia");
                }
                catch (Exception ex) when (ex is HttpRequestException or OperationCanceledException
                    or System.Text.Json.JsonException or InvalidOperationException)
                {
                    if (!IsDisposed)
                        MessageBox.Show(this, $"Falha ao publicar. O rascunho continua no desktop.\n{ex.Message}", "Publicar dia");
                }
                finally
                {
                    if (!IsDisposed)
                        publicarDia.Enabled = true;
                }
            };
            pnlDayActionHeader.Controls.Add(publicarDia);

            btnCopiarDia.Click += (s, e) =>
            {
                AbrirDialogCopiar();
            };

            btnLimparDia.Click += (s, e) =>
            {
                var confirm = MessageBox.Show(
                    $"Deseja limpar todos os pratos cadastrados para {_diaAtual.NomeDia} ({_diaAtual.Data:dd/MM/yyyy})?",
                    "Limpar Cardápio do Dia",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    _cardapioService.LimparDia(_diaAtual.Data);
                    RenderizarBarraSemana();
                    RenderizarCardapioDia();
                    MessageBox.Show("Dia limpo com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            // Ajuste responsivo de largura dos cards ao carregar e redimensionar o formulário
            this.Load += (s, e) => AjustarLarguraItens();
            this.Resize += (s, e) => AjustarLarguraItens();
        }

        private void AtualizarBotoesTurno()
        {
            if (!_isNoturno)
            {
                // Diurno ativo
                btnCardapioDiurno.FillColor = Color.FromArgb(245, 186, 66);
                btnCardapioDiurno.ForeColor = Color.FromArgb(48, 20, 26);
                btnCardapioDiurno.BorderThickness = 0;

                btnCardapioNoturno.FillColor = Color.White;
                btnCardapioNoturno.ForeColor = Color.FromArgb(90, 90, 90);
                btnCardapioNoturno.BorderThickness = 1;
                btnCardapioNoturno.BorderColor = Color.FromArgb(220, 220, 220);

                lblHeaderAlmoco.Text = "Almoço";
                pnlHeaderAlmoco.FillColor = Color.FromArgb(245, 186, 66);
                lblHeaderAlmoco.ForeColor = Color.FromArgb(50, 20, 28);

                lblHeaderVegano.Text = "Opções veganas · Almoço";
                pnlHeaderVegano.FillColor = Color.FromArgb(46, 125, 50);
            }
            else
            {
                // Noturno ativo
                btnCardapioNoturno.FillColor = Color.FromArgb(107, 29, 47);
                btnCardapioNoturno.ForeColor = Color.White;
                btnCardapioNoturno.BorderThickness = 0;

                btnCardapioDiurno.FillColor = Color.White;
                btnCardapioDiurno.ForeColor = Color.FromArgb(90, 90, 90);
                btnCardapioDiurno.BorderThickness = 1;
                btnCardapioDiurno.BorderColor = Color.FromArgb(220, 220, 220);

                lblHeaderAlmoco.Text = "Jantar";
                pnlHeaderAlmoco.FillColor = Color.FromArgb(107, 29, 47);
                lblHeaderAlmoco.ForeColor = Color.White;

                lblHeaderVegano.Text = "Opções veganas · Jantar";
                pnlHeaderVegano.FillColor = Color.FromArgb(46, 125, 50);
            }
        }

        private void CarregarCardapio()
        {
            _semana = _cardapioService.ListarSemana(_dataReferencia);

            if (_semana.Count > 0)
            {
                var diaCorrespondente = _semana.FirstOrDefault(d => d.Data.Date == _dataReferencia.Date);
                if (diaCorrespondente != null)
                {
                    _diaAtual = diaCorrespondente;
                }
                else if (_diaAtual != null)
                {
                    var mesmoDiaSemana = _semana.FirstOrDefault(d => d.DiaSemana == _diaAtual.DiaSemana);
                    _diaAtual = mesmoDiaSemana ?? _semana[0];
                }
                else
                {
                    _diaAtual = _semana[0];
                }

                lblHeaderSubtitle.Text = $"Cardápio oficial do Restaurante Universitário • Semana de {_semana.First().Data:dd/MM/yyyy} a {_semana.Last().Data:dd/MM/yyyy}";
            }

            RenderizarBarraSemana();
            RenderizarCardapioDia();
        }

        private async Task AtualizarCardapioDaApiAsync()
        {
            _consultaSemana?.Cancel();
            using var consulta = new CancellationTokenSource();
            _consultaSemana = consulta;
            try
            {
                if (await _cardapioService.AtualizarSemanaDaApiAsync(_dataReferencia, consulta.Token)
                    && !IsDisposed && !consulta.IsCancellationRequested)
                    CarregarCardapio();
            }
            finally
            {
                if (ReferenceEquals(_consultaSemana, consulta))
                    _consultaSemana = null;
            }
        }

        private void RenderizarBarraSemana()
        {
            pnlWeekBar.SuspendLayout();
            pnlWeekBar.Controls.Clear();

            // Botão "◀ Semana Anterior"
            var btnNavAnt = new Guna2Button
            {
                Width = 36,
                Height = 68,
                BorderRadius = 8,
                Cursor = Cursors.Hand,
                FillColor = Color.White,
                BorderColor = Color.FromArgb(220, 224, 230),
                BorderThickness = 1,
                ForeColor = Color.FromArgb(107, 29, 47),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Text = "◀",
                Margin = new Padding(0, 0, 6, 0)
            };
            btnNavAnt.Click += async (s, e) =>
            {
                _dataReferencia = _dataReferencia.AddDays(-7);
                CarregarCardapio();
                await AtualizarCardapioDaApiAsync();
            };
            pnlWeekBar.Controls.Add(btnNavAnt);

            // Dias da semana
            foreach (var dia in _semana)
            {
                bool isSelected = (_diaAtual != null && dia.Data.Date == _diaAtual.Data.Date);

                var card = new Guna2Panel
                {
                    Width = 98,
                    Height = 68,
                    BorderRadius = 8,
                    Cursor = Cursors.Hand,
                    Margin = new Padding(0, 0, 6, 0),
                    FillColor = isSelected ? Color.FromArgb(245, 186, 66) : Color.White,
                    BorderColor = isSelected ? Color.FromArgb(220, 160, 40) : Color.FromArgb(225, 228, 232),
                    BorderThickness = 1,
                    Tag = dia
                };

                var lblDia = new Label
                {
                    Text = dia.NomeDia,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = isSelected ? Color.FromArgb(45, 15, 20) : Color.FromArgb(40, 40, 40),
                    Location = new Point(4, 7),
                    AutoSize = false,
                    Size = new Size(90, 16),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent
                };

                var lblData = new Label
                {
                    Text = dia.Data.ToString("dd/MM/yyyy"),
                    Font = new Font("Segoe UI", 8F, FontStyle.Regular),
                    ForeColor = isSelected ? Color.FromArgb(70, 30, 35) : Color.FromArgb(120, 120, 120),
                    Location = new Point(4, 25),
                    AutoSize = false,
                    Size = new Size(90, 15),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent
                };

                var lblStatus = new Label
                {
                    Text = dia.Definido ? "● Definido" : "● Pendente",
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    ForeColor = dia.Definido ? Color.FromArgb(38, 120, 45) : Color.FromArgb(215, 120, 10),
                    Location = new Point(4, 44),
                    AutoSize = false,
                    Size = new Size(90, 16),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent
                };

                card.Controls.Add(lblDia);
                card.Controls.Add(lblData);
                card.Controls.Add(lblStatus);

                Action selecionar = () =>
                {
                    _diaAtual = dia;
                    _dataReferencia = dia.Data;
                    RenderizarBarraSemana();
                    RenderizarCardapioDia();
                };

                card.Click += (s, e) => selecionar();
                lblDia.Click += (s, e) => selecionar();
                lblData.Click += (s, e) => selecionar();
                lblStatus.Click += (s, e) => selecionar();

                pnlWeekBar.Controls.Add(card);
            }

            // Botão "Semana Seguinte ▶"
            var btnNavProx = new Guna2Button
            {
                Width = 36,
                Height = 68,
                BorderRadius = 8,
                Cursor = Cursors.Hand,
                FillColor = Color.White,
                BorderColor = Color.FromArgb(220, 224, 230),
                BorderThickness = 1,
                ForeColor = Color.FromArgb(107, 29, 47),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Text = "▶",
                Margin = new Padding(0, 0, 6, 0)
            };
            btnNavProx.Click += async (s, e) =>
            {
                _dataReferencia = _dataReferencia.AddDays(7);
                CarregarCardapio();
                await AtualizarCardapioDaApiAsync();
            };
            pnlWeekBar.Controls.Add(btnNavProx);

            // Botão "📅 Selecionar Semana"
            var btnSelSemana = new Guna2Panel
            {
                Width = 105,
                Height = 68,
                BorderRadius = 8,
                Cursor = Cursors.Hand,
                FillColor = Color.White,
                BorderColor = Color.FromArgb(225, 228, 232),
                BorderThickness = 1,
                Margin = new Padding(0, 0, 0, 0)
            };

            var lblTextoCal = new Label
            {
                Text = "Selecionar\nSemana",
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(107, 29, 47),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            btnSelSemana.Controls.Add(lblTextoCal);

            Action abrirCal = () => AbrirCalendarioSemana();
            btnSelSemana.Click += (s, e) => abrirCal();
            lblTextoCal.Click += (s, e) => abrirCal();

            pnlWeekBar.Controls.Add(btnSelSemana);
            pnlWeekBar.ResumeLayout(true);
        }

        private void AbrirCalendarioSemana()
        {
            using var dlg = new Form
            {
                Width = 330,
                Height = 360,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                Text = "Selecionar Semana / Data",
                BackColor = Color.White
            };

            var lblInstrucao = new Label
            {
                Text = "Escolha qualquer dia no calendário para visualizar a respectiva semana:",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(15, 12),
                Size = new Size(290, 32),
                ForeColor = Color.FromArgb(40, 40, 40)
            };

            var monthCalendar = new MonthCalendar
            {
                Location = new Point(45, 50),
                MaxSelectionCount = 1
            };
            monthCalendar.SetDate(_dataReferencia);

            var btnHoje = new Guna2Button
            {
                Text = "Semana Atual",
                Width = 100,
                Height = 36,
                Location = new Point(15, 265),
                BorderRadius = 6,
                FillColor = Color.FromArgb(240, 240, 240),
                ForeColor = Color.FromArgb(50, 50, 50),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold)
            };
            btnHoje.Click += async (s, e) =>
            {
                _dataReferencia = DateTime.Today;
                CarregarCardapio();
                dlg.Close();
                await AtualizarCardapioDaApiAsync();
            };

            var btnOk = new Guna2Button
            {
                Text = "Selecionar",
                Width = 100,
                Height = 36,
                Location = new Point(122, 265),
                BorderRadius = 6,
                FillColor = Color.FromArgb(107, 29, 47),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnOk.Click += async (s, e) =>
            {
                _dataReferencia = monthCalendar.SelectionStart;
                CarregarCardapio();
                dlg.Close();
                await AtualizarCardapioDaApiAsync();
            };

            var btnCancelar = new Guna2Button
            {
                Text = "Cancelar",
                Width = 75,
                Height = 36,
                Location = new Point(228, 265),
                BorderRadius = 6,
                FillColor = Color.White,
                BorderColor = Color.Gainsboro,
                BorderThickness = 1,
                ForeColor = Color.DimGray,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular)
            };
            btnCancelar.Click += (s, e) => dlg.Close();

            dlg.Controls.Add(lblInstrucao);
            dlg.Controls.Add(monthCalendar);
            dlg.Controls.Add(btnHoje);
            dlg.Controls.Add(btnOk);
            dlg.Controls.Add(btnCancelar);

            dlg.ShowDialog(this);
        }

        private void RenderizarCardapioDia()
        {
            if (_diaAtual == null) return;

            string nomeDiaCompleto = _ptBr.TextInfo.ToTitleCase(_diaAtual.Data.ToString("dddd, dd 'de' MMMM 'de' yyyy", _ptBr));
            lblDataSelecionada.Text = nomeDiaCompleto;

            List<ItemRefeicao> itensPadrao = !_isNoturno ? _diaAtual.AlmocoPadrao : _diaAtual.JantarPadrao;
            List<ItemRefeicao> itensVegano = !_isNoturno ? _diaAtual.AlmocoVegano : _diaAtual.JantarVegano;

            RenderizarListaPratos(flpItensAlmoco, itensPadrao, false);
            RenderizarListaPratos(flpItensVegano, itensVegano, true);

            lblDescNota.Text = _diaAtual.NotaTecnica;
            lblDescCafe.Text = _diaAtual.CafeManha;
        }

        private void RenderizarListaPratos(FlowLayoutPanel flp, List<ItemRefeicao> itens, bool isVegano)
        {
            flp.SuspendLayout();
            flp.Controls.Clear();

            int cardWidth = Math.Max(flp.Width - 25, 340);

            foreach (var item in itens)
            {
                var card = CriarCardItemRefeicao(item, cardWidth, isVegano);
                flp.Controls.Add(card);
            }

            flp.ResumeLayout(true);
        }

        private Guna2Panel CriarCardItemRefeicao(ItemRefeicao item, int width, bool isVegano)
        {
            var card = new Guna2Panel
            {
                Width = width,
                Height = 52,
                BorderRadius = 8,
                BorderColor = Color.FromArgb(238, 240, 243),
                BorderThickness = 1,
                FillColor = Color.FromArgb(254, 254, 255),
                Margin = new Padding(0, 0, 0, 6)
            };

            // Thumbnail do prato
            var pic = new Label
            {
                Location = new Point(6, 6),
                Size = new Size(40, 40),
                BackColor = isVegano ? Color.FromArgb(232, 245, 233) : Color.FromArgb(254, 243, 224),
                ForeColor = isVegano ? Color.FromArgb(36, 104, 48) : Color.FromArgb(107, 29, 47),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Text = string.Concat(item.Categoria.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Take(2).Select(parte => char.ToUpperInvariant(parte[0])))
            };

            // Categoria
            var lblCat = new Label
            {
                Text = item.Categoria,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = isVegano ? Color.FromArgb(46, 125, 50) : Color.FromArgb(107, 29, 47),
                Location = new Point(52, 6),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            // Nome do Prato
            var lblNome = new Label
            {
                Text = item.Nome,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = Color.FromArgb(35, 35, 35),
                Location = new Point(52, 23),
                AutoSize = false,
                Size = new Size(width - 185, 24),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                AutoEllipsis = true,
                BackColor = Color.Transparent
            };

            // Botão Editar (Lápis)
            var btnEdit = new Guna2Button
            {
                Size = new Size(58, 28),
                Location = new Point(width - 125, 12),
                BorderRadius = 6,
                Cursor = Cursors.Hand,
                FillColor = Color.FromArgb(254, 243, 220),
                ForeColor = Color.FromArgb(190, 120, 20),
                Font = new Font("Segoe UI", 8F),
                Text = "Editar",
                TextOffset = new Point(0, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            btnEdit.Click += (s, e) =>
            {
                EditarItem(item);
            };

            // Botão Excluir (Lixeira)
            var btnDel = new Guna2Button
            {
                Size = new Size(61, 28),
                Location = new Point(width - 66, 12),
                BorderRadius = 6,
                Cursor = Cursors.Hand,
                FillColor = Color.FromArgb(254, 235, 235),
                ForeColor = Color.FromArgb(210, 45, 45),
                Font = new Font("Segoe UI", 8F),
                Text = "Limpar",
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            btnDel.Click += (s, e) =>
            {
                item.Nome = "Não informado";
                lblNome.Text = item.Nome;
                _cardapioService.SalvarAlteracaoLocal(_diaAtual);
            };

            card.Controls.Add(pic);
            card.Controls.Add(lblCat);
            card.Controls.Add(lblNome);
            card.Controls.Add(btnEdit);
            card.Controls.Add(btnDel);

            return card;
        }

        private void AjustarLarguraItens()
        {
            int larguraAlmoco = Math.Max(flpItensAlmoco.Width - 25, 300);
            foreach (Control ctrl in flpItensAlmoco.Controls)
            {
                ctrl.Width = larguraAlmoco;
            }

            int larguraVegano = Math.Max(flpItensVegano.Width - 25, 300);
            foreach (Control ctrl in flpItensVegano.Controls)
            {
                ctrl.Width = larguraVegano;
            }
        }

        private void EditarItem(ItemRefeicao item)
        {
            string novoNome = PromptDialog.ShowDialog($"Alterar prato para '{item.Categoria}':", "Editar Prato", item.Nome);
            if (!string.IsNullOrWhiteSpace(novoNome))
            {
                item.Nome = novoNome.Trim();
                _diaAtual.Definido = true;
                _cardapioService.SalvarAlteracaoLocal(_diaAtual);
                RenderizarBarraSemana();
                RenderizarCardapioDia();
            }
        }

        private void AbrirEditorDia()
        {
            string msg = $"Selecione o que deseja fazer no cardápio de {_diaAtual.NomeDia} ({_diaAtual.Data:dd/MM/yyyy}):\n\n" +
                         "• Clique em Editar no prato que deseja alterar.\n" +
                         "• Use 'Copiar para outro dia' para replicar o cardápio em outra data.\n" +
                         "• Use 'Limpar Dia' para resetar a seleção.";

            MessageBox.Show(msg, "Gerenciador do Cardápio", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AbrirDialogCopiar()
        {
            using var form = new Form
            {
                Width = 360,
                Height = 220,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Copiar Cardápio",
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            };

            var lbl = new Label
            {
                Left = 20,
                Top = 20,
                Width = 300,
                Text = $"Copiar cardápio de {_diaAtual.NomeDia} ({_diaAtual.Data:dd/MM}) para:",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            var cmb = new ComboBox
            {
                Left = 20,
                Top = 55,
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };

            var outrosDias = _semana.Where(d => d.Data.Date != _diaAtual.Data.Date).ToList();
            foreach (var d in outrosDias)
            {
                cmb.Items.Add($"{d.NomeDia} - {d.Data:dd/MM/yyyy}");
            }
            if (cmb.Items.Count > 0) cmb.SelectedIndex = 0;

            var btnOk = new Guna2Button
            {
                Text = "Copiar",
                Left = 140,
                Top = 115,
                Width = 90,
                Height = 36,
                BorderRadius = 6,
                FillColor = Color.FromArgb(107, 29, 47),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            var btnCancel = new Guna2Button
            {
                Text = "Cancelar",
                Left = 240,
                Top = 115,
                Width = 80,
                Height = 36,
                BorderRadius = 6,
                FillColor = Color.FromArgb(230, 230, 230),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 9F)
            };

            btnOk.Click += (s, e) =>
            {
                if (cmb.SelectedIndex >= 0)
                {
                    var destino = outrosDias[cmb.SelectedIndex];
                    _cardapioService.CopiarDia(_diaAtual.Data, destino.Data);
                    RenderizarBarraSemana();
                    MessageBox.Show($"Cardápio copiado com sucesso para {destino.NomeDia} ({destino.Data:dd/MM})!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    form.Close();
                }
            };

            btnCancel.Click += (s, e) => form.Close();

            form.Controls.Add(lbl);
            form.Controls.Add(cmb);
            form.Controls.Add(btnOk);
            form.Controls.Add(btnCancel);
            form.ShowDialog(this);
        }
    }

    public static class PromptDialog
    {
        public static string ShowDialog(string text, string caption, string defaultValue = "")
        {
            using var prompt = new Form()
            {
                Width = 420,
                Height = 190,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            };

            var textLabel = new Label() { Left = 20, Top = 15, Text = text, AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            var textBox = new TextBox() { Left = 20, Top = 45, Width = 360, Text = defaultValue, Font = new Font("Segoe UI", 10F) };
            
            var confirmation = new Button() { Text = "Salvar", Left = 210, Width = 80, Top = 90, Height = 32, DialogResult = DialogResult.OK, BackColor = Color.FromArgb(107, 29, 47), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            var cancel = new Button() { Text = "Cancelar", Left = 300, Width = 80, Top = 90, Height = 32, DialogResult = DialogResult.Cancel, FlatStyle = FlatStyle.Flat };

            confirmation.Click += (sender, e) => { prompt.Close(); };
            cancel.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancel);
            prompt.AcceptButton = confirmation;
            prompt.CancelButton = cancel;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : defaultValue;
        }
    }
}
