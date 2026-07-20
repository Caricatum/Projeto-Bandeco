namespace ProjetoBandejao.Services
{
    partial class CardRefeicaoControl
    {
        /// <summary> 
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            pctrImagem = new PictureBox();
            lblNome = new Label();
            lblDescricao = new Label();
            lblIngredientesTitulo = new Label();
            lblIngredientes = new Label();
            btnVisualizar = new Button();
            btnEditar = new Button();
            btnExcluir = new Button();
            lblCalorias = new Label();
            ((System.ComponentModel.ISupportInitialize)pctrImagem).BeginInit();
            SuspendLayout();
            // 
            // pctrImagem
            // 
            pctrImagem.Location = new Point(20, 20);
            pctrImagem.Name = "pctrImagem";
            pctrImagem.Size = new Size(220, 160);
            pctrImagem.SizeMode = PictureBoxSizeMode.StretchImage;
            pctrImagem.TabIndex = 0;
            pctrImagem.TabStop = false;
            // 
            // lblNome
            // 
            lblNome.AutoSize = true;
            lblNome.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNome.ForeColor = Color.Black;
            lblNome.Location = new Point(270, 20);
            lblNome.Name = "lblNome";
            lblNome.Size = new Size(205, 32);
            lblNome.TabIndex = 1;
            lblNome.Text = "Frango Grelhado";
            lblNome.Click += lblNome_Click;
            // 
            // lblDescricao
            // 
            lblDescricao.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDescricao.ForeColor = Color.Black;
            lblDescricao.Location = new Point(270, 55);
            lblDescricao.Name = "lblDescricao";
            lblDescricao.Size = new Size(430, 40);
            lblDescricao.TabIndex = 2;
            lblDescricao.Text = "Peito de frango acompanhado de arroz integral e legumes.";
            // 
            // lblIngredientesTitulo
            // 
            lblIngredientesTitulo.AutoSize = true;
            lblIngredientesTitulo.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblIngredientesTitulo.ForeColor = Color.ForestGreen;
            lblIngredientesTitulo.Location = new Point(270, 100);
            lblIngredientesTitulo.Name = "lblIngredientesTitulo";
            lblIngredientesTitulo.Size = new Size(89, 17);
            lblIngredientesTitulo.TabIndex = 3;
            lblIngredientesTitulo.Text = "Ingredientes:";
            // 
            // lblIngredientes
            // 
            lblIngredientes.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblIngredientes.ForeColor = Color.Black;
            lblIngredientes.Location = new Point(381, 100);
            lblIngredientes.Name = "lblIngredientes";
            lblIngredientes.Size = new Size(330, 40);
            lblIngredientes.TabIndex = 4;
            lblIngredientes.Text = "Frango, arroz integral e cenoura.";
            // 
            // btnVisualizar
            // 
            btnVisualizar.FlatStyle = FlatStyle.Flat;
            btnVisualizar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnVisualizar.ForeColor = Color.Black;
            btnVisualizar.Location = new Point(810, 30);
            btnVisualizar.Name = "btnVisualizar";
            btnVisualizar.Size = new Size(140, 40);
            btnVisualizar.TabIndex = 7;
            btnVisualizar.Text = "👁️  Visualizar";
            btnVisualizar.UseVisualStyleBackColor = true;
            btnVisualizar.Click += button1_Click;
            // 
            // btnEditar
            // 
            btnEditar.BackColor = Color.ForestGreen;
            btnEditar.FlatAppearance.BorderSize = 0;
            btnEditar.FlatStyle = FlatStyle.Flat;
            btnEditar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEditar.ForeColor = Color.White;
            btnEditar.Location = new Point(810, 80);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(140, 40);
            btnEditar.TabIndex = 8;
            btnEditar.Text = "✏️  Editar";
            btnEditar.UseVisualStyleBackColor = false;
            // 
            // btnExcluir
            // 
            btnExcluir.BackColor = Color.Red;
            btnExcluir.FlatStyle = FlatStyle.Flat;
            btnExcluir.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExcluir.ForeColor = Color.White;
            btnExcluir.Location = new Point(810, 130);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.Size = new Size(140, 40);
            btnExcluir.TabIndex = 9;
            btnExcluir.Text = "Excluir";
            btnExcluir.UseVisualStyleBackColor = false;
            btnExcluir.Click += button3_Click;
            // 
            // lblCalorias
            // 
            lblCalorias.BorderStyle = BorderStyle.FixedSingle;
            lblCalorias.Location = new Point(270, 150);
            lblCalorias.Name = "lblCalorias";
            lblCalorias.Size = new Size(100, 20);
            lblCalorias.TabIndex = 10;
            lblCalorias.Text = "🔥 Calorias";
            // 
            // CardRefeicaoControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(lblCalorias);
            Controls.Add(btnExcluir);
            Controls.Add(btnEditar);
            Controls.Add(btnVisualizar);
            Controls.Add(lblIngredientes);
            Controls.Add(lblIngredientesTitulo);
            Controls.Add(lblDescricao);
            Controls.Add(lblNome);
            Controls.Add(pctrImagem);
            Name = "CardRefeicaoControl";
            Size = new Size(980, 200);
            ((System.ComponentModel.ISupportInitialize)pctrImagem).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pctrImagem;
        private Label lblNome;
        private Label lblDescricao;
        private Label lblIngredientesTitulo;
        private Label lblIngredientes;
        private Button btnVisualizar;
        private Button btnEditar;
        private Button btnExcluir;
        private Label lblCalorias;
    }
}
