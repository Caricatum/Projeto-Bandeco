namespace ProjetoBandejao.Forms
{
    partial class ConfirmarCodigoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            lblTitulo = new Label();
            lblDescricao = new Label();
            lblEmail = new Label();
            txtEmail = new TextBox();
            mskCodigo = new MaskedTextBox();
            lblCodigo = new Label();
            btnConfirmar = new Button();
            panel1 = new Panel();
            lblNaoRecebeu = new Label();
            linkReenviar = new LinkLabel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resource.ImagemCodigo;
            pictureBox1.Location = new Point(180, 30);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(240, 180);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // lblTitulo
            // 
            lblTitulo.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.Black;
            lblTitulo.Location = new Point(125, 240);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(350, 50);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Confirmar Código";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDescricao
            // 
            lblDescricao.AutoSize = true;
            lblDescricao.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDescricao.ForeColor = Color.DimGray;
            lblDescricao.Location = new Point(125, 305);
            lblDescricao.Name = "lblDescricao";
            lblDescricao.Size = new Size(353, 25);
            lblDescricao.TabIndex = 2;
            lblDescricao.Text = "Digite o código enviado para seu e-mail.";
            lblDescricao.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEmail.ForeColor = Color.Black;
            lblEmail.Location = new Point(50, 390);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(59, 21);
            lblEmail.TabIndex = 3;
            lblEmail.Text = "E-mail";
            lblEmail.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtEmail
            // 
            txtEmail.BackColor = Color.WhiteSmoke;
            txtEmail.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtEmail.Location = new Point(50, 420);
            txtEmail.Name = "txtEmail";
            txtEmail.ReadOnly = true;
            txtEmail.Size = new Size(500, 27);
            txtEmail.TabIndex = 4;
            // 
            // mskCodigo
            // 
            mskCodigo.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            mskCodigo.Location = new Point(50, 510);
            mskCodigo.Mask = "0 0 0 0 0 0";
            mskCodigo.Name = "mskCodigo";
            mskCodigo.PromptChar = '0';
            mskCodigo.Size = new Size(500, 43);
            mskCodigo.TabIndex = 5;
            mskCodigo.TextAlign = HorizontalAlignment.Center;
            // 
            // lblCodigo
            // 
            lblCodigo.AutoSize = true;
            lblCodigo.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCodigo.ForeColor = Color.Black;
            lblCodigo.Location = new Point(50, 480);
            lblCodigo.Name = "lblCodigo";
            lblCodigo.Size = new Size(188, 21);
            lblCodigo.TabIndex = 6;
            lblCodigo.Text = "Código de confirmação";
            lblCodigo.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnConfirmar
            // 
            btnConfirmar.BackColor = Color.ForestGreen;
            btnConfirmar.FlatAppearance.BorderSize = 0;
            btnConfirmar.FlatStyle = FlatStyle.Flat;
            btnConfirmar.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnConfirmar.ForeColor = Color.White;
            btnConfirmar.Location = new Point(50, 590);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.Size = new Size(500, 55);
            btnConfirmar.TabIndex = 7;
            btnConfirmar.Text = "✓ Confirmar";
            btnConfirmar.UseVisualStyleBackColor = false;
            btnConfirmar.Click += btnConfirmar_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Gainsboro;
            panel1.Location = new Point(50, 680);
            panel1.Name = "panel1";
            panel1.Size = new Size(500, 1);
            panel1.TabIndex = 8;
            // 
            // lblNaoRecebeu
            // 
            lblNaoRecebeu.AutoSize = true;
            lblNaoRecebeu.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNaoRecebeu.ForeColor = Color.DimGray;
            lblNaoRecebeu.Location = new Point(143, 705);
            lblNaoRecebeu.Name = "lblNaoRecebeu";
            lblNaoRecebeu.Size = new Size(165, 20);
            lblNaoRecebeu.TabIndex = 9;
            lblNaoRecebeu.Text = "Não recebeu o código?";
            lblNaoRecebeu.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // linkReenviar
            // 
            linkReenviar.ActiveLinkColor = Color.Green;
            linkReenviar.AutoSize = true;
            linkReenviar.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkReenviar.LinkColor = Color.ForestGreen;
            linkReenviar.Location = new Point(314, 705);
            linkReenviar.Name = "linkReenviar";
            linkReenviar.Size = new Size(66, 20);
            linkReenviar.TabIndex = 10;
            linkReenviar.TabStop = true;
            linkReenviar.Text = "Reenviar";
            // 
            // ConfirmarCodigoForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(584, 761);
            Controls.Add(linkReenviar);
            Controls.Add(lblNaoRecebeu);
            Controls.Add(panel1);
            Controls.Add(btnConfirmar);
            Controls.Add(lblCodigo);
            Controls.Add(mskCodigo);
            Controls.Add(txtEmail);
            Controls.Add(lblEmail);
            Controls.Add(lblDescricao);
            Controls.Add(lblTitulo);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "ConfirmarCodigoForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ConfirmarCodigoForm";
            Load += ConfirmarCodigoForm_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label lblTitulo;
        private Label lblDescricao;
        private Label lblEmail;
        private TextBox txtEmail;
        private MaskedTextBox mskCodigo;
        private Label lblCodigo;
        private Button btnConfirmar;
        private Panel panel1;
        private Label lblNaoRecebeu;
        private LinkLabel linkReenviar;
    }
}