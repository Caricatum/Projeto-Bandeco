using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using ProjetoBandejao.Models;

namespace ProjetoBandejao.Services
{
    public partial class CardRefeicaoControl : UserControl
    {
        private Prato? _pratoAtual;
        private static readonly HttpClient imageClient = new HttpClient();

        public event EventHandler<Prato>? OnExcluirPrato;

        public CardRefeicaoControl()
        {
            InitializeComponent();

            btnEditar.Click += (s, e) =>
            {
                if (_pratoAtual != null)
                {
                    MessageBox.Show($"Detalhes do prato:\n\nNome: {_pratoAtual.Nome}\nCategoria: {_pratoAtual.CategoriaTexto}\nVegano: {(_pratoAtual.Vegano ? "Sim" : "Não")}\n\n{_pratoAtual.Descricao}", "Informações do Prato", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            btnVerDetalhes.Click += (s, e) =>
            {
                if (_pratoAtual != null)
                {
                    var result = MessageBox.Show($"Deseja excluir o prato '{_pratoAtual.Nome}' do sistema?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        var service = new PratoService();
                        if (service.Deletar(_pratoAtual.Id))
                        {
                            MessageBox.Show("Prato excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            OnExcluirPrato?.Invoke(this, _pratoAtual);
                        }
                    }
                }
            };
        }

        public void PreencherDados(Prato prato)
        {
            _pratoAtual = prato;

            // Preenche dados textuais
            lblTitle.Text = prato.Nome;
            lblDescription.Text = string.IsNullOrWhiteSpace(prato.Descricao) ? "Sem descrição cadastrada." : prato.Descricao;
            
            // Categoria e Tipo
            string catNome = prato.Categoria?.Descricao ?? (prato.Vegano ? "Vegano" : "Tradicional");
            lblCategoriaVal.Text = catNome;
            lblTipoVal.Text = prato.Vegano ? "Vegano" : "Padrão";
            
            // Atualiza os Badges superiores
            lblPratoPrincipalBadge.Text = prato.Vegano ? "🌱 Vegano" : $"🍽️ {catNome}";

            // Informações nutricionais
            float kcal = prato.ValorNutricional?.Kcal ?? (float)prato.Calorias;
            float carbs = prato.ValorNutricional?.Carboidratos ?? (float)prato.Carboidratos;
            float prot = prato.ValorNutricional?.Proteinas ?? (float)prato.Proteinas;
            float gord = prato.ValorNutricional?.Lipidios ?? (float)prato.Gorduras;

            lblCalorias.Text = kcal > 0 ? $"🔥 {kcal:F0} kcal\nCalorias" : "🔥 - kcal\nCalorias";
            lblCarbo.Text = carbs > 0 ? $"🌾 {carbs:F1} g\nCarboidratos" : "🌾 - g\nCarboidratos";
            lblProtein.Text = prot > 0 ? $"🍗 {prot:F1} g\nProteínas" : "🍗 - g\nProteínas";
            lblGordura.Text = gord > 0 ? $"💧 {gord:F1} g\nGorduras" : "💧 - g\nGorduras";

            // Troca o botão "Ver Detalhes" para "Excluir" com estilo visual claro
            btnVerDetalhes.Text = "🗑️ Excluir";
            btnVerDetalhes.FillColor = Color.FromArgb(180, 40, 40);

            // Carrega Imagem
            CarregarImagem(prato);

            // Ingredientes (Separados por vírgula)
            LoadIngredientes(prato.Ingredientes);
        }

        private async void CarregarImagem(Prato prato)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(prato.Imagem))
                {
                    string imageUrl = prato.Imagem;
                    if (imageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    {
                        byte[] bytes = await imageClient.GetByteArrayAsync(imageUrl);
                        using var ms = new MemoryStream(bytes);
                        pctImage.Image = Image.FromStream(ms);
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(prato.ImagemBase64))
                {
                    byte[] imageBytes = Convert.FromBase64String(prato.ImagemBase64);
                    using var ms = new MemoryStream(imageBytes);
                    pctImage.Image = Image.FromStream(ms);
                    return;
                }

                // Imagem padrão caso não tenha imagem cadastrada
                pctImage.Image = Properties.Resource.ImagemBandecco1;
            }
            catch
            {
                pctImage.Image = Properties.Resource.ImagemBandecco1;
            }
        }

        private void LoadIngredientes(string ingredientesStr)
        {
            flpIngredientes.Controls.Clear();

            if (string.IsNullOrWhiteSpace(ingredientesStr))
            {
                lblIngredientesTitle.Visible = false;
                flpIngredientes.Visible = false;
                return;
            }

            lblIngredientesTitle.Visible = true;
            flpIngredientes.Visible = true;

            var ingredientes = ingredientesStr.Split(',')
                                              .Select(i => i.Trim())
                                              .Where(i => !string.IsNullOrEmpty(i))
                                              .ToList();

            foreach (var ing in ingredientes)
            {
                var btn = new Guna2Button
                {
                    Text = "● " + ing,
                    Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                    ForeColor = Color.FromArgb(60, 80, 60),
                    FillColor = Color.FromArgb(245, 250, 245),
                    BorderColor = Color.FromArgb(230, 235, 230),
                    BorderThickness = 1,
                    BorderRadius = 12,
                    AutoSize = true,
                    Height = 28,
                    Padding = new Padding(10, 2, 10, 2),
                    Cursor = Cursors.Default
                };
                btn.HoverState.FillColor = btn.FillColor; 
                btn.HoverState.BorderColor = btn.BorderColor;
                
                flpIngredientes.Controls.Add(btn);
            }
        }
    }
}
