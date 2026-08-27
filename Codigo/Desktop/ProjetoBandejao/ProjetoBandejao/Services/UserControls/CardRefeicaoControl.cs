using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using ProjetoBandejao.Models;

namespace ProjetoBandejao.Services
{
    public partial class CardRefeicaoControl : UserControl
    {
        private Prato _pratoAtual;

        public CardRefeicaoControl()
        {
            InitializeComponent();
        }

        public void PreencherDados(Prato prato)
        {
            _pratoAtual = prato;

            // Preenche dados textuais
            lblTitle.Text = prato.Nome;
            lblDescription.Text = prato.Descricao;
            
            // Categoria e Tipo
            lblCategoriaVal.Text = string.IsNullOrWhiteSpace(prato.Categoria) ? "-" : prato.Categoria;
            lblTipoVal.Text = string.IsNullOrWhiteSpace(prato.Tipo) ? "-" : prato.Tipo;
            
            // Atualiza os Badges superiores (Opcional, usando a categoria como badge)
            lblPratoPrincipalBadge.Text = $"🍽️ {prato.Categoria}";

            // Informações nutricionais
            lblCalorias.Text = $"🔥 {prato.Calorias} kcal\nCalorias";
            lblCarbo.Text = $"🌾 {prato.Carboidratos} g\nCarboidratos";
            lblProtein.Text = $"🍗 {prato.Proteinas} g\nProteínas";
            lblGordura.Text = $"💧 {prato.Gorduras} g\nGorduras";

            // Se for baixa caloria, mostra opção saudável, caso contrário oculta
            lblHealthyBadge.Visible = prato.Calorias < 500;

            // Carrega Imagem se houver
            if (!string.IsNullOrEmpty(prato.ImagemBase64))
            {
                try
                {
                    byte[] imageBytes = Convert.FromBase64String(prato.ImagemBase64);
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        pctImage.Image = Image.FromStream(ms);
                    }
                }
                catch
                {
                    // Mantém imagem padrão em caso de erro
                }
            }

            // Ingredientes (Separados por vírgula)
            LoadIngredientes(prato.Ingredientes);
        }

        private void LoadIngredientes(string ingredientesStr)
        {
            flpIngredientes.Controls.Clear();

            if (string.IsNullOrWhiteSpace(ingredientesStr))
                return;

            // Separa por vírgula e remove espaços extras
            var ingredientes = ingredientesStr.Split(',')
                                              .Select(i => i.Trim())
                                              .Where(i => !string.IsNullOrEmpty(i))
                                              .ToList();

            foreach (var ing in ingredientes)
            {
                var btn = new Guna2Button();
                btn.Text = "● " + ing;
                btn.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
                btn.ForeColor = Color.FromArgb(60, 80, 60);
                btn.FillColor = Color.FromArgb(245, 250, 245);
                btn.BorderColor = Color.FromArgb(230, 235, 230);
                btn.BorderThickness = 1;
                btn.BorderRadius = 12;
                btn.AutoSize = true;
                btn.Height = 28;
                btn.Padding = new Padding(10, 2, 10, 2);
                btn.Cursor = Cursors.Default;
                btn.HoverState.FillColor = btn.FillColor; 
                btn.HoverState.BorderColor = btn.BorderColor;
                
                flpIngredientes.Controls.Add(btn);
            }
        }
    }
}
