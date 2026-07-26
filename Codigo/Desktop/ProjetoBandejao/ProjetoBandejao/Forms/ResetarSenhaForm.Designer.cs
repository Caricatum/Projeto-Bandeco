namespace ProjetoBandejao.Forms
{
    partial class ResetarSenhaForm
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
            pctrChave = new PictureBox();
            lblTitulo = new Label();
            lblDescricao = new Label();
            lblEmail = new Label();
            lblNovaSenha = new Label();
            lblCodigo = new Label();
            lblNaoRecebeu = new Label();
            txtEmail = new TextBox();
            txtCodigo = new MaskedTextBox();
            txtNovaSenha = new TextBox();
            lblConfirmarSenha = new Label();
            txtConfirmarSenha = new TextBox();
            btnAlterarSenha = new Button();
            linkReenviar = new LinkLabel();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)pctrChave).BeginInit();
            SuspendLayout();
            // 
            // pctrChave
            // 
            pctrChave.Image = Properties.Resource.Chave;
            pctrChave.Location = new Point(130, 0);
            pctrChave.Name = "pctrChave";
            pctrChave.Size = new Size(390, 213);
            pctrChave.SizeMode = PictureBoxSizeMode.StretchImage;
            pctrChave.TabIndex = 0;
            pctrChave.TabStop = false;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.Location = new Point(180, 230);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(296, 50);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Redefinir Senha";
            // 
            // lblDescricao
            // 
            lblDescricao.AutoSize = true;
            lblDescricao.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDescricao.ForeColor = Color.DimGray;
            lblDescricao.Location = new Point(130, 300);
            lblDescricao.Name = "lblDescricao";
            lblDescricao.Size = new Size(390, 25);
            lblDescricao.TabIndex = 2;
            lblDescricao.Text = "Informe o código recebido e sua nova senha.";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEmail.Location = new Point(60, 370);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(59, 21);
            lblEmail.TabIndex = 3;
            lblEmail.Text = "E-mail";
            // 
            // lblNovaSenha
            // 
            lblNovaSenha.AutoSize = true;
            lblNovaSenha.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNovaSenha.Location = new Point(60, 590);
            lblNovaSenha.Name = "lblNovaSenha";
            lblNovaSenha.Size = new Size(100, 21);
            lblNovaSenha.TabIndex = 4;
            lblNovaSenha.Text = "Nova senha";
            // 
            // lblCodigo
            // 
            lblCodigo.AutoSize = true;
            lblCodigo.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCodigo.Location = new Point(60, 480);
            lblCodigo.Name = "lblCodigo";
            lblCodigo.Size = new Size(136, 21);
            lblCodigo.TabIndex = 5;
            lblCodigo.Text = "Código recebido";
            // 
            // lblNaoRecebeu
            // 
            lblNaoRecebeu.AutoSize = true;
            lblNaoRecebeu.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNaoRecebeu.ForeColor = Color.DimGray;
            lblNaoRecebeu.Location = new Point(149, 960);
            lblNaoRecebeu.Name = "lblNaoRecebeu";
            lblNaoRecebeu.Size = new Size(169, 21);
            lblNaoRecebeu.TabIndex = 6;
            lblNaoRecebeu.Text = "Não recebeu o código?";
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtEmail.Location = new Point(60, 405);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(540, 33);
            txtEmail.TabIndex = 7;
            // 
            // txtCodigo
            // 
            txtCodigo.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCodigo.Location = new Point(60, 515);
            txtCodigo.Mask = "0 0 0 0 0 0";
            txtCodigo.Name = "txtCodigo";
            txtCodigo.PromptChar = '0';
            txtCodigo.Size = new Size(540, 33);
            txtCodigo.TabIndex = 8;
            txtCodigo.TextAlign = HorizontalAlignment.Center;
            // 
            // txtNovaSenha
            // 
            txtNovaSenha.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNovaSenha.Location = new Point(60, 625);
            txtNovaSenha.Name = "txtNovaSenha";
            txtNovaSenha.Size = new Size(540, 33);
            txtNovaSenha.TabIndex = 9;
            txtNovaSenha.UseSystemPasswordChar = true;
            // 
            // lblConfirmarSenha
            // 
            lblConfirmarSenha.AutoSize = true;
            lblConfirmarSenha.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblConfirmarSenha.Location = new Point(60, 700);
            lblConfirmarSenha.Name = "lblConfirmarSenha";
            lblConfirmarSenha.Size = new Size(178, 21);
            lblConfirmarSenha.TabIndex = 10;
            lblConfirmarSenha.Text = "Confirmar nova senha";
            // 
            // txtConfirmarSenha
            // 
            txtConfirmarSenha.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtConfirmarSenha.Location = new Point(60, 725);
            txtConfirmarSenha.Name = "txtConfirmarSenha";
            txtConfirmarSenha.Size = new Size(540, 33);
            txtConfirmarSenha.TabIndex = 11;
            txtConfirmarSenha.UseSystemPasswordChar = true;
            // 
            // btnAlterarSenha
            // 
            btnAlterarSenha.BackColor = Color.ForestGreen;
            btnAlterarSenha.FlatAppearance.BorderSize = 0;
            btnAlterarSenha.FlatStyle = FlatStyle.Flat;
            btnAlterarSenha.Font = new Font("Segoe UI", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAlterarSenha.ForeColor = Color.White;
            btnAlterarSenha.Location = new Point(60, 820);
            btnAlterarSenha.Name = "btnAlterarSenha";
            btnAlterarSenha.Size = new Size(540, 65);
            btnAlterarSenha.TabIndex = 12;
            btnAlterarSenha.Text = "✅ Alterar senha";
            btnAlterarSenha.UseVisualStyleBackColor = false;
            btnAlterarSenha.Click += btnAlterarSenha_Click;
            // 
            // linkReenviar
            // 
            linkReenviar.ActiveLinkColor = Color.Green;
            linkReenviar.AutoSize = true;
            linkReenviar.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkReenviar.LinkColor = Color.ForestGreen;
            linkReenviar.Location = new Point(315, 960);
            linkReenviar.Name = "linkReenviar";
            linkReenviar.Size = new Size(135, 21);
            linkReenviar.TabIndex = 13;
            linkReenviar.TabStop = true;
            linkReenviar.Text = "Enviar novamente";
            linkReenviar.LinkClicked += linkReenviar_LinkClicked;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Gainsboro;
            panel1.Location = new Point(1, 925);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 1);
            panel1.TabIndex = 14;
            // 
            // ResetarSenhaForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(684, 1011);
            Controls.Add(panel1);
            Controls.Add(linkReenviar);
            Controls.Add(btnAlterarSenha);
            Controls.Add(txtConfirmarSenha);
            Controls.Add(lblConfirmarSenha);
            Controls.Add(txtNovaSenha);
            Controls.Add(txtCodigo);
            Controls.Add(txtEmail);
            Controls.Add(lblNaoRecebeu);
            Controls.Add(lblCodigo);
            Controls.Add(lblNovaSenha);
            Controls.Add(lblEmail);
            Controls.Add(lblDescricao);
            Controls.Add(lblTitulo);
            Controls.Add(pctrChave);
            ForeColor = SystemColors.ControlText;
            Name = "ResetarSenhaForm";
            Text = "Redefinir Senha";
            Load += ResetarSenhaForm_Load;
            ((System.ComponentModel.ISupportInitialize)pctrChave).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pctrChave;
        private Label lblTitulo;
        private Label lblDescricao;
        private Label lblEmail;
        private Label lblNovaSenha;
        private Label lblCodigo;
        private Label lblNaoRecebeu;
        private TextBox txtEmail;
        private MaskedTextBox txtCodigo;
        private TextBox txtNovaSenha;
        private Label lblConfirmarSenha;
        private TextBox txtConfirmarSenha;
        private Button btnAlterarSenha;
        private LinkLabel linkReenviar;
        private Panel panel1;
    }
}