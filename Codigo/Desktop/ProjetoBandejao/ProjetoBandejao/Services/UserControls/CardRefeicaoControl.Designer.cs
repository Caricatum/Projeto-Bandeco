namespace ProjetoBandejao.Services
{
    partial class CardRefeicaoControl
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
            this.pnlMain = new Guna.UI2.WinForms.Guna2Panel();
            this.pctImage = new Guna.UI2.WinForms.Guna2PictureBox();
            this.lblHealthyBadge = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblPratoPrincipalBadge = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblDescription = new System.Windows.Forms.Label();
            this.pnlNutrition = new Guna.UI2.WinForms.Guna2Panel();
            this.lblCalorias = new System.Windows.Forms.Label();
            this.lblCarbo = new System.Windows.Forms.Label();
            this.lblProtein = new System.Windows.Forms.Label();
            this.lblGordura = new System.Windows.Forms.Label();
            this.lblIngredientesTitle = new System.Windows.Forms.Label();
            this.flpIngredientes = new System.Windows.Forms.FlowLayoutPanel();
            this.lblFooterText = new System.Windows.Forms.Label();
            this.btnEditar = new Guna.UI2.WinForms.Guna2Button();
            this.btnVerDetalhes = new Guna.UI2.WinForms.Guna2Button();
            this.lblCategoriaTitle = new System.Windows.Forms.Label();
            this.lblCategoriaVal = new System.Windows.Forms.Label();
            this.lblTipoTitle = new System.Windows.Forms.Label();
            this.lblTipoVal = new System.Windows.Forms.Label();
            this.lblDisponibilidadeTitle = new System.Windows.Forms.Label();
            this.lblDisponibilidadeVal = new System.Windows.Forms.Label();
            this.lblAvaliacaoTitle = new System.Windows.Forms.Label();
            this.lblAvaliacaoVal = new System.Windows.Forms.Label();
            
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctImage)).BeginInit();
            this.pnlNutrition.SuspendLayout();
            this.SuspendLayout();
            
            // pnlMain
            this.pnlMain.BackColor = System.Drawing.Color.Transparent;
            this.pnlMain.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.pnlMain.BorderRadius = 16;
            this.pnlMain.BorderThickness = 1;
            this.pnlMain.Controls.Add(this.lblAvaliacaoVal);
            this.pnlMain.Controls.Add(this.lblAvaliacaoTitle);
            this.pnlMain.Controls.Add(this.lblDisponibilidadeVal);
            this.pnlMain.Controls.Add(this.lblDisponibilidadeTitle);
            this.pnlMain.Controls.Add(this.lblTipoVal);
            this.pnlMain.Controls.Add(this.lblTipoTitle);
            this.pnlMain.Controls.Add(this.lblCategoriaVal);
            this.pnlMain.Controls.Add(this.lblCategoriaTitle);
            this.pnlMain.Controls.Add(this.btnVerDetalhes);
            this.pnlMain.Controls.Add(this.btnEditar);
            this.pnlMain.Controls.Add(this.lblFooterText);
            this.pnlMain.Controls.Add(this.flpIngredientes);
            this.pnlMain.Controls.Add(this.lblIngredientesTitle);
            this.pnlMain.Controls.Add(this.pnlNutrition);
            this.pnlMain.Controls.Add(this.lblHealthyBadge);
            this.pnlMain.Controls.Add(this.pctImage);
            this.pnlMain.Controls.Add(this.lblPratoPrincipalBadge);
            this.pnlMain.Controls.Add(this.lblDescription);
            this.pnlMain.Controls.Add(this.lblTitle);
            this.pnlMain.FillColor = System.Drawing.Color.White;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(950, 480);
            this.pnlMain.TabIndex = 0;
            
            // pctImage
            this.pctImage.BorderRadius = 12;
            this.pctImage.Image = Properties.Resource.ImagemBandecco1;
            this.pctImage.ImageRotate = 0F;
            this.pctImage.Location = new System.Drawing.Point(20, 20);
            this.pctImage.Name = "pctImage";
            this.pctImage.Size = new System.Drawing.Size(280, 260);
            this.pctImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pctImage.TabIndex = 1;
            this.pctImage.TabStop = false;
            
            // lblHealthyBadge
            this.lblHealthyBadge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(245)))), ((int)(((byte)(235)))));
            this.lblHealthyBadge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(100)))), ((int)(((byte)(30)))));
            this.lblHealthyBadge.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHealthyBadge.Location = new System.Drawing.Point(30, 30);
            this.lblHealthyBadge.Name = "lblHealthyBadge";
            this.lblHealthyBadge.Size = new System.Drawing.Size(100, 22);
            this.lblHealthyBadge.TabIndex = 2;
            this.lblHealthyBadge.Text = "🍃 Opção Saudável";
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(60)))), ((int)(((byte)(20)))));
            this.lblTitle.Location = new System.Drawing.Point(320, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(275, 45);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Frango Grelhado";
            
            // lblPratoPrincipalBadge
            this.lblPratoPrincipalBadge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(245)))), ((int)(((byte)(235)))));
            this.lblPratoPrincipalBadge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(100)))), ((int)(((byte)(30)))));
            this.lblPratoPrincipalBadge.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPratoPrincipalBadge.Location = new System.Drawing.Point(790, 30);
            this.lblPratoPrincipalBadge.Name = "lblPratoPrincipalBadge";
            this.lblPratoPrincipalBadge.Size = new System.Drawing.Size(100, 22);
            this.lblPratoPrincipalBadge.TabIndex = 4;
            this.lblPratoPrincipalBadge.Text = "🍽️ Prato Principal";
            
            // lblDescription
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblDescription.Location = new System.Drawing.Point(324, 75);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(580, 50);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "Peito de frango grelhado temperado com ervas, acompanha arroz branco, feijão carioca e salada fresca da estação.";
            
            // pnlNutrition
            this.pnlNutrition.BackColor = System.Drawing.Color.Transparent;
            this.pnlNutrition.BorderRadius = 12;
            this.pnlNutrition.BorderThickness = 1;
            this.pnlNutrition.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(220)))));
            this.pnlNutrition.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(248)))));
            this.pnlNutrition.Location = new System.Drawing.Point(324, 140);
            this.pnlNutrition.Name = "pnlNutrition";
            this.pnlNutrition.Size = new System.Drawing.Size(596, 100);
            this.pnlNutrition.TabIndex = 6;
            this.pnlNutrition.Controls.Add(this.lblCalorias);
            this.pnlNutrition.Controls.Add(this.lblCarbo);
            this.pnlNutrition.Controls.Add(this.lblProtein);
            this.pnlNutrition.Controls.Add(this.lblGordura);
            
            // lblCalorias
            this.lblCalorias.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCalorias.ForeColor = System.Drawing.Color.Black;
            this.lblCalorias.Location = new System.Drawing.Point(20, 20);
            this.lblCalorias.Name = "lblCalorias";
            this.lblCalorias.Size = new System.Drawing.Size(120, 60);
            this.lblCalorias.TabIndex = 7;
            this.lblCalorias.Text = "🔥 412 kcal\nCalorias";
            this.lblCalorias.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // lblCarbo
            this.lblCarbo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCarbo.ForeColor = System.Drawing.Color.Black;
            this.lblCarbo.Location = new System.Drawing.Point(160, 20);
            this.lblCarbo.Name = "lblCarbo";
            this.lblCarbo.Size = new System.Drawing.Size(120, 60);
            this.lblCarbo.TabIndex = 8;
            this.lblCarbo.Text = "🌾 48 g\nCarboidratos";
            this.lblCarbo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // lblProtein
            this.lblProtein.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProtein.ForeColor = System.Drawing.Color.Black;
            this.lblProtein.Location = new System.Drawing.Point(300, 20);
            this.lblProtein.Name = "lblProtein";
            this.lblProtein.Size = new System.Drawing.Size(120, 60);
            this.lblProtein.TabIndex = 9;
            this.lblProtein.Text = "🍗 32 g\nProteínas";
            this.lblProtein.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // lblGordura
            this.lblGordura.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGordura.ForeColor = System.Drawing.Color.Black;
            this.lblGordura.Location = new System.Drawing.Point(440, 20);
            this.lblGordura.Name = "lblGordura";
            this.lblGordura.Size = new System.Drawing.Size(120, 60);
            this.lblGordura.TabIndex = 10;
            this.lblGordura.Text = "💧 11 g\nGorduras";
            this.lblGordura.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // lblIngredientesTitle
            this.lblIngredientesTitle.AutoSize = true;
            this.lblIngredientesTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIngredientesTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(60)))), ((int)(((byte)(20)))));
            this.lblIngredientesTitle.Location = new System.Drawing.Point(20, 310);
            this.lblIngredientesTitle.Name = "lblIngredientesTitle";
            this.lblIngredientesTitle.Size = new System.Drawing.Size(130, 21);
            this.lblIngredientesTitle.TabIndex = 11;
            this.lblIngredientesTitle.Text = "🍃 Ingredientes";
            
            // flpIngredientes
            this.flpIngredientes.Location = new System.Drawing.Point(20, 340);
            this.flpIngredientes.Name = "flpIngredientes";
            this.flpIngredientes.Size = new System.Drawing.Size(460, 90);
            this.flpIngredientes.TabIndex = 12;
            
            // lblFooterText
            this.lblFooterText.AutoSize = true;
            this.lblFooterText.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFooterText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblFooterText.Location = new System.Drawing.Point(20, 440);
            this.lblFooterText.Name = "lblFooterText";
            this.lblFooterText.Size = new System.Drawing.Size(200, 19);
            this.lblFooterText.TabIndex = 13;
            this.lblFooterText.Text = "🍽️ Disponível no almoço e jantar";
            
            // lblCategoriaTitle
            this.lblCategoriaTitle.AutoSize = true;
            this.lblCategoriaTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCategoriaTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblCategoriaTitle.Location = new System.Drawing.Point(520, 320);
            this.lblCategoriaTitle.Name = "lblCategoriaTitle";
            this.lblCategoriaTitle.Size = new System.Drawing.Size(58, 15);
            this.lblCategoriaTitle.TabIndex = 14;
            this.lblCategoriaTitle.Text = "Categoria";
            
            // lblCategoriaVal
            this.lblCategoriaVal.AutoSize = true;
            this.lblCategoriaVal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCategoriaVal.ForeColor = System.Drawing.Color.Black;
            this.lblCategoriaVal.Location = new System.Drawing.Point(520, 340);
            this.lblCategoriaVal.Name = "lblCategoriaVal";
            this.lblCategoriaVal.Size = new System.Drawing.Size(100, 19);
            this.lblCategoriaVal.TabIndex = 15;
            this.lblCategoriaVal.Text = "Prato Principal";
            
            // lblTipoTitle
            this.lblTipoTitle.AutoSize = true;
            this.lblTipoTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblTipoTitle.Location = new System.Drawing.Point(520, 370);
            this.lblTipoTitle.Name = "lblTipoTitle";
            this.lblTipoTitle.Size = new System.Drawing.Size(30, 15);
            this.lblTipoTitle.TabIndex = 16;
            this.lblTipoTitle.Text = "Tipo";
            
            // lblTipoVal
            this.lblTipoVal.AutoSize = true;
            this.lblTipoVal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoVal.ForeColor = System.Drawing.Color.Black;
            this.lblTipoVal.Location = new System.Drawing.Point(520, 390);
            this.lblTipoVal.Name = "lblTipoVal";
            this.lblTipoVal.Size = new System.Drawing.Size(55, 19);
            this.lblTipoVal.TabIndex = 17;
            this.lblTipoVal.Text = "Comum";
            
            // lblDisponibilidadeTitle
            this.lblDisponibilidadeTitle.AutoSize = true;
            this.lblDisponibilidadeTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisponibilidadeTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblDisponibilidadeTitle.Location = new System.Drawing.Point(700, 320);
            this.lblDisponibilidadeTitle.Name = "lblDisponibilidadeTitle";
            this.lblDisponibilidadeTitle.Size = new System.Drawing.Size(83, 15);
            this.lblDisponibilidadeTitle.TabIndex = 18;
            this.lblDisponibilidadeTitle.Text = "Disponível em";
            
            // lblDisponibilidadeVal
            this.lblDisponibilidadeVal.AutoSize = true;
            this.lblDisponibilidadeVal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisponibilidadeVal.ForeColor = System.Drawing.Color.Black;
            this.lblDisponibilidadeVal.Location = new System.Drawing.Point(700, 340);
            this.lblDisponibilidadeVal.Name = "lblDisponibilidadeVal";
            this.lblDisponibilidadeVal.Size = new System.Drawing.Size(185, 19);
            this.lblDisponibilidadeVal.TabIndex = 19;
            this.lblDisponibilidadeVal.Text = "12/08/2025 a 19/08/2025";
            
            // lblAvaliacaoTitle
            this.lblAvaliacaoTitle.AutoSize = true;
            this.lblAvaliacaoTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvaliacaoTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblAvaliacaoTitle.Location = new System.Drawing.Point(700, 370);
            this.lblAvaliacaoTitle.Name = "lblAvaliacaoTitle";
            this.lblAvaliacaoTitle.Size = new System.Drawing.Size(115, 15);
            this.lblAvaliacaoTitle.TabIndex = 20;
            this.lblAvaliacaoTitle.Text = "Avaliação dos alunos";
            
            // lblAvaliacaoVal
            this.lblAvaliacaoVal.AutoSize = true;
            this.lblAvaliacaoVal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvaliacaoVal.ForeColor = System.Drawing.Color.Black;
            this.lblAvaliacaoVal.Location = new System.Drawing.Point(700, 390);
            this.lblAvaliacaoVal.Name = "lblAvaliacaoVal";
            this.lblAvaliacaoVal.Size = new System.Drawing.Size(120, 19);
            this.lblAvaliacaoVal.TabIndex = 21;
            this.lblAvaliacaoVal.Text = "4.6 ★★★★☆ (128)";
            
            // btnEditar
            this.btnEditar.BorderRadius = 8;
            this.btnEditar.BorderThickness = 1;
            this.btnEditar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(100)))), ((int)(((byte)(40)))));
            this.btnEditar.FillColor = System.Drawing.Color.White;
            this.btnEditar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(100)))), ((int)(((byte)(40)))));
            this.btnEditar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditar.Location = new System.Drawing.Point(520, 430);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(160, 40);
            this.btnEditar.TabIndex = 22;
            this.btnEditar.Text = "✏️ Editar";
            
            // btnVerDetalhes
            this.btnVerDetalhes.BorderRadius = 8;
            this.btnVerDetalhes.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(100)))), ((int)(((byte)(40)))));
            this.btnVerDetalhes.ForeColor = System.Drawing.Color.White;
            this.btnVerDetalhes.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVerDetalhes.Location = new System.Drawing.Point(700, 430);
            this.btnVerDetalhes.Name = "btnVerDetalhes";
            this.btnVerDetalhes.Size = new System.Drawing.Size(220, 40);
            this.btnVerDetalhes.TabIndex = 23;
            this.btnVerDetalhes.Text = "👁️ Ver detalhes";
            
            // CardRefeicaoControl
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pnlMain);
            this.Name = "CardRefeicaoControl";
            this.Size = new System.Drawing.Size(960, 490);
            
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctImage)).EndInit();
            this.pnlNutrition.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        
        private Guna.UI2.WinForms.Guna2Panel pnlMain;
        private Guna.UI2.WinForms.Guna2PictureBox pctImage;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblHealthyBadge;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblPratoPrincipalBadge;
        private System.Windows.Forms.Label lblDescription;
        private Guna.UI2.WinForms.Guna2Panel pnlNutrition;
        private System.Windows.Forms.Label lblCalorias;
        private System.Windows.Forms.Label lblCarbo;
        private System.Windows.Forms.Label lblProtein;
        private System.Windows.Forms.Label lblGordura;
        private System.Windows.Forms.Label lblIngredientesTitle;
        private System.Windows.Forms.FlowLayoutPanel flpIngredientes;
        private System.Windows.Forms.Label lblFooterText;
        private System.Windows.Forms.Label lblCategoriaTitle;
        private System.Windows.Forms.Label lblCategoriaVal;
        private System.Windows.Forms.Label lblTipoTitle;
        private System.Windows.Forms.Label lblTipoVal;
        private System.Windows.Forms.Label lblDisponibilidadeTitle;
        private System.Windows.Forms.Label lblDisponibilidadeVal;
        private System.Windows.Forms.Label lblAvaliacaoTitle;
        private System.Windows.Forms.Label lblAvaliacaoVal;
        private Guna.UI2.WinForms.Guna2Button btnEditar;
        private Guna.UI2.WinForms.Guna2Button btnVerDetalhes;
    }
}
