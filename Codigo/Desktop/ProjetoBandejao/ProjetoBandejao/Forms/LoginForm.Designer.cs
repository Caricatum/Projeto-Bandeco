namespace ProjetoBandejao.Forms
{
    partial class LoginForm
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
            lblEmail = new Label();
            lblSubtitulo = new Label();
            lblSenha = new Label();
            txtEmail = new TextBox();
            txtSenha = new TextBox();
            btnEntrar = new Button();
            lblMensagem = new Label();
            linkCadastro = new LinkLabel();
            linkEsqueciSenha = new LinkLabel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Left;
            pictureBox1.Image = Properties.Resource.ImagemBandecco1;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(400, 461);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.DarkGreen;
            lblTitulo.Location = new Point(525, 58);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(161, 45);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "RU COTIL";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblEmail.Location = new Point(525, 154);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(59, 20);
            lblEmail.TabIndex = 2;
            lblEmail.Text = "Usuário";
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubtitulo.ForeColor = Color.DimGray;
            lblSubtitulo.Location = new Point(525, 103);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(157, 21);
            lblSubtitulo.TabIndex = 3;
            lblSubtitulo.Text = "Login do Funcionário";
            // 
            // lblSenha
            // 
            lblSenha.AutoSize = true;
            lblSenha.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSenha.Location = new Point(525, 228);
            lblSenha.Name = "lblSenha";
            lblSenha.Size = new Size(49, 20);
            lblSenha.TabIndex = 4;
            lblSenha.Text = "Senha";
            // 
            // txtEmail
            // 
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtEmail.Location = new Point(525, 177);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(220, 27);
            txtEmail.TabIndex = 5;
            // 
            // txtSenha
            // 
            txtSenha.BorderStyle = BorderStyle.FixedSingle;
            txtSenha.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSenha.Location = new Point(525, 251);
            txtSenha.Name = "txtSenha";
            txtSenha.Size = new Size(220, 27);
            txtSenha.TabIndex = 6;
            txtSenha.UseSystemPasswordChar = true;
            // 
            // btnEntrar
            // 
            btnEntrar.Location = new Point(566, 320);
            btnEntrar.Name = "btnEntrar";
            btnEntrar.Size = new Size(140, 45);
            btnEntrar.TabIndex = 7;
            btnEntrar.Text = "Entrar";
            btnEntrar.UseVisualStyleBackColor = true;
            btnEntrar.Click += btnEntrar_Click;
            // 
            // lblMensagem
            // 
            lblMensagem.AutoSize = true;
            lblMensagem.ForeColor = Color.Red;
            lblMensagem.Location = new Point(574, 377);
            lblMensagem.Name = "lblMensagem";
            lblMensagem.Size = new Size(0, 15);
            lblMensagem.TabIndex = 8;
            // 
            // linkCadastro
            // 
            linkCadastro.AutoSize = true;
            linkCadastro.Location = new Point(576, 401);
            linkCadastro.Name = "linkCadastro";
            linkCadastro.Size = new Size(121, 15);
            linkCadastro.TabIndex = 9;
            linkCadastro.TabStop = true;
            linkCadastro.Text = "Cadastrar funcionário";
            linkCadastro.LinkClicked += linkCadastro_LinkClicked;
            // 
            // linkEsqueciSenha
            // 
            linkEsqueciSenha.AutoSize = true;
            linkEsqueciSenha.Location = new Point(624, 285);
            linkEsqueciSenha.Name = "linkEsqueciSenha";
            linkEsqueciSenha.Size = new Size(118, 15);
            linkEsqueciSenha.TabIndex = 10;
            linkEsqueciSenha.TabStop = true;
            linkEsqueciSenha.Text = "Esqueci minha senha";
            linkEsqueciSenha.LinkClicked += linkEsqueciSenha_LinkClicked;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(884, 461);
            Controls.Add(linkEsqueciSenha);
            Controls.Add(linkCadastro);
            Controls.Add(lblMensagem);
            Controls.Add(btnEntrar);
            Controls.Add(txtSenha);
            Controls.Add(txtEmail);
            Controls.Add(lblSenha);
            Controls.Add(lblSubtitulo);
            Controls.Add(lblEmail);
            Controls.Add(lblTitulo);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RU COTIL - LOGIN";
            Load += LoginForm_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label lblTitulo;
        private Label lblEmail;
        private Label lblSubtitulo;
        private Label lblSenha;
        private TextBox txtEmail;
        private TextBox txtSenha;
        private Button btnEntrar;
        private Label lblMensagem;
        private LinkLabel linkCadastro;
        private LinkLabel linkEsqueciSenha;
    }
}