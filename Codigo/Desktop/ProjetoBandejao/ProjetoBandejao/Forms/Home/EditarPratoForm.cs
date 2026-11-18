using Guna.UI2.WinForms;
using ProjetoBandejao.Models;
using ProjetoBandejao.Services;

namespace ProjetoBandejao.Forms.Home
{
    internal sealed class EditarPratoForm : Form
    {
        public EditarPratoForm(PratoService service, Prato? atual)
        {
            Text = atual == null ? "Novo prato" : "Editar prato";
            ClientSize = new Size(515, 650);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = MinimizeBox = false;
            Font = new Font("Segoe UI", 10);
            BackColor = Color.FromArgb(249, 244, 246);
            Padding = new Padding(22);

            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1 };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            Controls.Add(layout);
            void Adicionar(Control controle, int altura)
            {
                int linha = layout.RowCount++;
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, altura));
                controle.Dock = DockStyle.Fill;
                layout.Controls.Add(controle, 0, linha);
            }
            void Legenda(string texto) => Adicionar(new Label { Text = texto, TextAlign = ContentAlignment.MiddleLeft }, 28);

            Adicionar(new Label { Text = Text, Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.FromArgb(107, 29, 47) }, 42);
            Legenda("Nome");
            var nome = new TextBox { Text = atual?.Nome ?? "", AccessibleName = "Nome do prato" };
            Adicionar(nome, 33);
            Legenda("Descrição");
            var descricao = new TextBox { Text = atual?.Descricao ?? "", Multiline = true, ScrollBars = ScrollBars.Vertical, AccessibleName = "Descrição do prato" };
            Adicionar(descricao, 65);
            Legenda("Categoria");
            var categoria = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, AccessibleName = "Categoria" };
            Adicionar(categoria, 34);
            var vegano = new CheckBox { Text = "Prato vegano", Checked = atual?.Vegano == true };
            Adicionar(vegano, 32);

            string? foto = null;
            var escolher = new Button { Text = "Escolher foto (opcional)", AutoEllipsis = true };
            var status = new Label { ForeColor = Color.FromArgb(107, 29, 47), TextAlign = ContentAlignment.MiddleLeft };
            escolher.Click += (_, _) =>
            {
                using var dialogo = new OpenFileDialog { Filter = "JPEG ou PNG|*.jpeg;*.jpg;*.png" };
                if (dialogo.ShowDialog(this) != DialogResult.OK) return;
                if (new FileInfo(dialogo.FileName).Length > 10 * 1024 * 1024)
                {
                    status.Text = "Escolha uma foto de até 10 MB.";
                    return;
                }
                foto = dialogo.FileName;
                escolher.Text = Path.GetFileName(foto);
                status.Text = "";
            };
            Adicionar(escolher, 36);

            Legenda("Valores nutricionais por 100 g");
            var grade = new TableLayoutPanel { ColumnCount = 4, RowCount = 2 };
            string[] nomes = ["Calorias (kcal)", "Carboidratos (g)", "Proteínas (g)", "Gorduras (g)"];
            double[] iniciais =
            [
                atual?.ValorNutricional?.Kcal ?? atual?.Calorias ?? 0,
                atual?.ValorNutricional?.Carboidratos ?? atual?.Carboidratos ?? 0,
                atual?.ValorNutricional?.Proteinas ?? atual?.Proteinas ?? 0,
                atual?.ValorNutricional?.Lipidios ?? atual?.Gorduras ?? 0
            ];
            var valores = new NumericUpDown[4];
            for (int i = 0; i < nomes.Length; i++)
            {
                grade.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
                grade.Controls.Add(new Label { Text = nomes[i], AutoSize = true }, i, 0);
                valores[i] = new NumericUpDown
                {
                    DecimalPlaces = 2, Maximum = 9999, Value = (decimal)Math.Clamp(iniciais[i], 0, 9999),
                    Dock = DockStyle.Fill, AccessibleName = nomes[i]
                };
                grade.Controls.Add(valores[i], i, 1);
            }
            Adicionar(grade, 56);

            Legenda("Alergênicos presentes");
            var alergenicos = new CheckedListBox { CheckOnClick = true, MultiColumn = true, ColumnWidth = 136 };
            for (int i = 0; i < Prato.CamposAlergenicos.Count; i++)
                alergenicos.Items.Add(Prato.CamposAlergenicos[i].Rotulo, atual?.TemAlergenico(i) == true);
            Adicionar(alergenicos, 78);
            Adicionar(status, 40);
            var salvar = new Guna2Button { Text = "Salvar prato", FillColor = Color.FromArgb(107, 29, 47), BorderRadius = 8, Enabled = false };
            Adicionar(salvar, 43);
            AcceptButton = salvar;

            Shown += async (_, _) =>
            {
                var categorias = await Task.Run(service.ListarCategorias);
                if (IsDisposed) return;
                if (atual?.Categoria is { } original && categorias.All(item => item.Id != original.Id))
                    categorias.Add(original);
                categoria.Items.AddRange(categorias.Cast<object>().ToArray());
                if (categorias.Count == 0)
                {
                    status.Text = "Nenhuma categoria disponível.";
                    return;
                }
                categoria.SelectedIndex = Math.Max(0, categorias.FindIndex(item => item.Id == atual?.Categoria?.Id));
                salvar.Enabled = true;
            };
            salvar.Click += async (_, _) =>
            {
                if (nome.Text.Trim().Length == 0 || categoria.SelectedItem is not Categoria selecionada)
                {
                    status.Text = "Informe o nome e a categoria.";
                    return;
                }
                var prato = new Prato
                {
                    Id = atual?.Id ?? 0, Nome = nome.Text.Trim(), Descricao = descricao.Text.Trim(), Vegano = vegano.Checked,
                    Categoria = selecionada,
                    Calorias = (double)valores[0].Value,
                    Carboidratos = (double)valores[1].Value,
                    Proteinas = (double)valores[2].Value,
                    Gorduras = (double)valores[3].Value,
                    Alergenicos = alergenicos.CheckedIndices.Cast<int>().Aggregate(0, (mascara, indice) => mascara | (1 << indice))
                };
                if (prato.Descricao.Length == 0) prato.Descricao = "Sem descrição";
                salvar.Enabled = false;
                status.Text = "Salvando…";
                try
                {
                    string? erro = atual == null
                        ? await Task.Run(() => service.Cadastrar(prato, foto)) ? null : "Não foi possível cadastrar o prato."
                        : await service.AtualizarAsync(prato, foto);
                    if (IsDisposed) return;
                    if (erro != null) { status.Text = erro; return; }
                    DialogResult = DialogResult.OK;
                    Close();
                }
                finally { if (!IsDisposed) salvar.Enabled = true; }
            };
        }
    }
}
