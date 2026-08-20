namespace ProjetoBandejao.Forms.Home
{
    partial class MuralForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges19 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges20 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            txtBusca = new Guna.UI2.WinForms.Guna2TextBox();
            cmbCategoriaBusca = new Guna.UI2.WinForms.Guna2ComboBox();
            cmbFiltroRecentes = new Guna.UI2.WinForms.Guna2ComboBox();
            pnlCortica = new Guna.UI2.WinForms.Guna2Panel();
            flpMural = new FlowLayoutPanel();
            pnlCriarAviso = new Guna.UI2.WinForms.Guna2Panel();
            lblCriarAviso = new Label();
            lblTipo = new Label();
            cmbTipo = new Guna.UI2.WinForms.Guna2ComboBox();
            lblNovoTitulo = new Label();
            txtNovoTitulo = new Guna.UI2.WinForms.Guna2TextBox();
            lblMensagem = new Label();
            txtMensagem = new Guna.UI2.WinForms.Guna2TextBox();
            lblCores = new Label();
            btnCancelar = new Guna.UI2.WinForms.Guna2Button();
            btnPublicar = new Guna.UI2.WinForms.Guna2Button();
            lblInfoRodape = new Label();
            pnlCortica.SuspendLayout();
            pnlCriarAviso.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.Black;
            lblTitulo.Location = new Point(30, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(236, 40);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Mural de Avisos";
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 10F);
            lblSubtitulo.ForeColor = Color.Black;
            lblSubtitulo.Location = new Point(34, 65);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(462, 19);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Compartilhe informações importantes com toda a comunidade acadêmica.";
            // 
            // txtBusca
            // 
            txtBusca.BorderRadius = 8;
            txtBusca.CustomizableEdges = customizableEdges1;
            txtBusca.DefaultText = "";
            txtBusca.Font = new Font("Segoe UI", 10F);
            txtBusca.Location = new Point(210, 110);
            txtBusca.Name = "txtBusca";
            txtBusca.PlaceholderText = "Buscar avisos...";
            txtBusca.SelectedText = "";
            txtBusca.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtBusca.Size = new Size(400, 36);
            txtBusca.TabIndex = 3;
            // 
            // cmbCategoriaBusca
            // 
            cmbCategoriaBusca.BackColor = Color.Transparent;
            cmbCategoriaBusca.BorderRadius = 8;
            cmbCategoriaBusca.CustomizableEdges = customizableEdges3;
            cmbCategoriaBusca.DrawMode = DrawMode.OwnerDrawFixed;
            cmbCategoriaBusca.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoriaBusca.FocusedColor = Color.Empty;
            cmbCategoriaBusca.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            cmbCategoriaBusca.ForeColor = Color.ForestGreen;
            cmbCategoriaBusca.ItemHeight = 30;
            cmbCategoriaBusca.Items.AddRange(new object[] { "Todos os avisos", "Alertas", "Eventos", "Informativos" });
            cmbCategoriaBusca.Location = new Point(35, 110);
            cmbCategoriaBusca.Name = "cmbCategoriaBusca";
            cmbCategoriaBusca.ShadowDecoration.CustomizableEdges = customizableEdges4;
            cmbCategoriaBusca.Size = new Size(160, 36);
            cmbCategoriaBusca.StartIndex = 0;
            cmbCategoriaBusca.TabIndex = 2;
            // 
            // cmbFiltroRecentes
            // 
            cmbFiltroRecentes.BackColor = Color.Transparent;
            cmbFiltroRecentes.BorderRadius = 8;
            cmbFiltroRecentes.CustomizableEdges = customizableEdges5;
            cmbFiltroRecentes.DrawMode = DrawMode.OwnerDrawFixed;
            cmbFiltroRecentes.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiltroRecentes.FocusedColor = Color.Empty;
            cmbFiltroRecentes.Font = new Font("Segoe UI", 9.75F);
            cmbFiltroRecentes.ForeColor = Color.FromArgb(68, 88, 112);
            cmbFiltroRecentes.ItemHeight = 30;
            cmbFiltroRecentes.Items.AddRange(new object[] { "Mais recentes", "Mais antigos" });
            cmbFiltroRecentes.Location = new Point(625, 110);
            cmbFiltroRecentes.Name = "cmbFiltroRecentes";
            cmbFiltroRecentes.ShadowDecoration.CustomizableEdges = customizableEdges6;
            cmbFiltroRecentes.Size = new Size(160, 36);
            cmbFiltroRecentes.StartIndex = 0;
            cmbFiltroRecentes.TabIndex = 4;
            // 
            // pnlCortica
            // 
            pnlCortica.BackColor = Color.Transparent;
            pnlCortica.BorderRadius = 12;
            pnlCortica.Controls.Add(flpMural);
            pnlCortica.CustomizableEdges = customizableEdges7;
            pnlCortica.FillColor = Color.Tan;
            pnlCortica.Location = new Point(35, 165);
            pnlCortica.Name = "pnlCortica";
            pnlCortica.ShadowDecoration.CustomizableEdges = customizableEdges8;
            pnlCortica.Size = new Size(750, 500);
            pnlCortica.TabIndex = 5;
            // 
            // flpMural
            // 
            flpMural.AutoScroll = true;
            flpMural.BackColor = Color.Transparent;
            flpMural.Location = new Point(15, 15);
            flpMural.Name = "flpMural";
            flpMural.Size = new Size(720, 470);
            flpMural.TabIndex = 0;
            // 
            // pnlCriarAviso
            // 
            pnlCriarAviso.BorderColor = Color.Gainsboro;
            pnlCriarAviso.BorderRadius = 12;
            pnlCriarAviso.BorderThickness = 1;
            pnlCriarAviso.Controls.Add(lblCriarAviso);
            pnlCriarAviso.Controls.Add(lblTipo);
            pnlCriarAviso.Controls.Add(cmbTipo);
            pnlCriarAviso.Controls.Add(lblNovoTitulo);
            pnlCriarAviso.Controls.Add(txtNovoTitulo);
            pnlCriarAviso.Controls.Add(lblMensagem);
            pnlCriarAviso.Controls.Add(txtMensagem);
            pnlCriarAviso.Controls.Add(lblCores);
            pnlCriarAviso.Controls.Add(btnCancelar);
            pnlCriarAviso.Controls.Add(btnPublicar);
            pnlCriarAviso.CustomizableEdges = customizableEdges19;
            pnlCriarAviso.FillColor = Color.White;
            pnlCriarAviso.Location = new Point(800, 165);
            pnlCriarAviso.Name = "pnlCriarAviso";
            pnlCriarAviso.ShadowDecoration.CustomizableEdges = customizableEdges20;
            pnlCriarAviso.Size = new Size(340, 500);
            pnlCriarAviso.TabIndex = 6;
            // 
            // lblCriarAviso
            // 
            lblCriarAviso.AutoSize = true;
            lblCriarAviso.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            lblCriarAviso.ForeColor = Color.Black;
            lblCriarAviso.Location = new Point(20, 20);
            lblCriarAviso.Name = "lblCriarAviso";
            lblCriarAviso.Size = new Size(162, 25);
            lblCriarAviso.TabIndex = 0;
            lblCriarAviso.Text = "Criar Novo Aviso";
            // 
            // lblTipo
            // 
            lblTipo.AutoSize = true;
            lblTipo.Font = new Font("Segoe UI", 9.75F);
            lblTipo.ForeColor = Color.Black;
            lblTipo.Location = new Point(20, 70);
            lblTipo.Name = "lblTipo";
            lblTipo.Size = new Size(87, 17);
            lblTipo.TabIndex = 1;
            lblTipo.Text = "Tipo de aviso";
            // 
            // cmbTipo
            // 
            cmbTipo.BackColor = Color.Transparent;
            cmbTipo.BorderRadius = 6;
            cmbTipo.CustomizableEdges = customizableEdges9;
            cmbTipo.DrawMode = DrawMode.OwnerDrawFixed;
            cmbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipo.FocusedColor = Color.Empty;
            cmbTipo.Font = new Font("Segoe UI", 9.75F);
            cmbTipo.ForeColor = Color.FromArgb(68, 88, 112);
            cmbTipo.ItemHeight = 30;
            cmbTipo.Items.AddRange(new object[] { "Informativo", "Alerta", "Evento" });
            cmbTipo.Location = new Point(23, 95);
            cmbTipo.Name = "cmbTipo";
            cmbTipo.ShadowDecoration.CustomizableEdges = customizableEdges10;
            cmbTipo.Size = new Size(294, 36);
            cmbTipo.StartIndex = 0;
            cmbTipo.TabIndex = 2;
            // 
            // lblNovoTitulo
            // 
            lblNovoTitulo.AutoSize = true;
            lblNovoTitulo.Font = new Font("Segoe UI", 9.75F);
            lblNovoTitulo.Location = new Point(20, 145);
            lblNovoTitulo.Name = "lblNovoTitulo";
            lblNovoTitulo.Size = new Size(40, 17);
            lblNovoTitulo.TabIndex = 3;
            lblNovoTitulo.Text = "Título";
            // 
            // txtNovoTitulo
            // 
            txtNovoTitulo.BorderRadius = 6;
            txtNovoTitulo.CustomizableEdges = customizableEdges11;
            txtNovoTitulo.DefaultText = "";
            txtNovoTitulo.Font = new Font("Segoe UI", 9.75F);
            txtNovoTitulo.Location = new Point(23, 170);
            txtNovoTitulo.Name = "txtNovoTitulo";
            txtNovoTitulo.PlaceholderText = "Digite um título para o aviso...";
            txtNovoTitulo.SelectedText = "";
            txtNovoTitulo.ShadowDecoration.CustomizableEdges = customizableEdges12;
            txtNovoTitulo.Size = new Size(294, 36);
            txtNovoTitulo.TabIndex = 4;
            // 
            // lblMensagem
            // 
            lblMensagem.AutoSize = true;
            lblMensagem.Font = new Font("Segoe UI", 9.75F);
            lblMensagem.Location = new Point(20, 220);
            lblMensagem.Name = "lblMensagem";
            lblMensagem.Size = new Size(73, 17);
            lblMensagem.TabIndex = 5;
            lblMensagem.Text = "Mensagem";
            // 
            // txtMensagem
            // 
            txtMensagem.BorderRadius = 6;
            txtMensagem.CustomizableEdges = customizableEdges13;
            txtMensagem.DefaultText = "";
            txtMensagem.Font = new Font("Segoe UI", 9.75F);
            txtMensagem.Location = new Point(23, 245);
            txtMensagem.Multiline = true;
            txtMensagem.Name = "txtMensagem";
            txtMensagem.PlaceholderText = "Escreva sua mensagem aqui...";
            txtMensagem.SelectedText = "";
            txtMensagem.ShadowDecoration.CustomizableEdges = customizableEdges14;
            txtMensagem.Size = new Size(294, 110);
            txtMensagem.TabIndex = 6;
            // 
            // lblCores
            // 
            lblCores.AutoSize = true;
            lblCores.Font = new Font("Segoe UI", 9.75F);
            lblCores.Location = new Point(20, 370);
            lblCores.Name = "lblCores";
            lblCores.Size = new Size(91, 17);
            lblCores.TabIndex = 7;
            lblCores.Text = "Cor do post-it";
            // 
            // btnCancelar
            // 
            btnCancelar.BorderColor = Color.LightGray;
            btnCancelar.BorderRadius = 8;
            btnCancelar.BorderThickness = 1;
            btnCancelar.CustomizableEdges = customizableEdges15;
            btnCancelar.FillColor = Color.White;
            btnCancelar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.Black;
            btnCancelar.Location = new Point(23, 440);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.ShadowDecoration.CustomizableEdges = customizableEdges16;
            btnCancelar.Size = new Size(120, 40);
            btnCancelar.TabIndex = 8;
            btnCancelar.Text = "Cancelar";
            // 
            // btnPublicar
            // 
            btnPublicar.BorderRadius = 8;
            btnPublicar.CustomizableEdges = customizableEdges17;
            btnPublicar.FillColor = Color.ForestGreen;
            btnPublicar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnPublicar.ForeColor = Color.White;
            btnPublicar.Location = new Point(157, 440);
            btnPublicar.Name = "btnPublicar";
            btnPublicar.ShadowDecoration.CustomizableEdges = customizableEdges18;
            btnPublicar.Size = new Size(160, 40);
            btnPublicar.TabIndex = 9;
            btnPublicar.Text = "Publicar Aviso";
            btnPublicar.Click += btnPublicar_Click;
            // 
            // lblInfoRodape
            // 
            lblInfoRodape.AutoSize = true;
            lblInfoRodape.Font = new Font("Segoe UI", 9.75F);
            lblInfoRodape.ForeColor = Color.DimGray;
            lblInfoRodape.Location = new Point(34, 675);
            lblInfoRodape.Name = "lblInfoRodape";
            lblInfoRodape.Size = new Size(493, 17);
            lblInfoRodape.TabIndex = 7;
            lblInfoRodape.Text = "Os avisos são exibidos para todos os alunos. Fique atento às datas de publicação!";
            // 
            // MuralForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Salmon;
            ClientSize = new Size(1167, 716);
            Controls.Add(lblTitulo);
            Controls.Add(lblSubtitulo);
            Controls.Add(cmbCategoriaBusca);
            Controls.Add(txtBusca);
            Controls.Add(cmbFiltroRecentes);
            Controls.Add(pnlCortica);
            Controls.Add(pnlCriarAviso);
            Controls.Add(lblInfoRodape);
            FormBorderStyle = FormBorderStyle.None;
            Name = "MuralForm";
            pnlCortica.ResumeLayout(false);
            pnlCriarAviso.ResumeLayout(false);
            pnlCriarAviso.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSubtitulo;
        private Guna.UI2.WinForms.Guna2TextBox txtBusca;
        private Guna.UI2.WinForms.Guna2ComboBox cmbCategoriaBusca;
        private Guna.UI2.WinForms.Guna2ComboBox cmbFiltroRecentes;
        private Guna.UI2.WinForms.Guna2Panel pnlCortica;
        private System.Windows.Forms.FlowLayoutPanel flpMural;
        private Guna.UI2.WinForms.Guna2Panel pnlCriarAviso;
        private System.Windows.Forms.Label lblCriarAviso;
        private System.Windows.Forms.Label lblTipo;
        private Guna.UI2.WinForms.Guna2ComboBox cmbTipo;
        private System.Windows.Forms.Label lblNovoTitulo;
        private Guna.UI2.WinForms.Guna2TextBox txtNovoTitulo;
        private System.Windows.Forms.Label lblMensagem;
        private Guna.UI2.WinForms.Guna2TextBox txtMensagem;
        private System.Windows.Forms.Label lblCores;
        private Guna.UI2.WinForms.Guna2Button btnCancelar;
        private Guna.UI2.WinForms.Guna2Button btnPublicar;
        private System.Windows.Forms.Label lblInfoRodape;
    }
}
