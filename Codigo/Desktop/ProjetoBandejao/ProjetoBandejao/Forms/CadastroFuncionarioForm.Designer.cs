namespace ProjetoBandejao.Forms
{
    partial class CadastroFuncionarioForm
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
            lblTitulo = new Label();
            lblNome = new Label();
            txtNome = new TextBox();
            picLogo = new PictureBox();
            lblSubtitulo = new Label();
            pnlLinha = new Panel();
            txtEmail = new TextBox();
            txtSenha = new TextBox();
            lblEmail = new Label();
            lblSenha = new Label();
            lblPermissao = new Label();
            chkAdministrador = new CheckBox();
            btnCancelar = new Button();
            btnCadastrar = new Button();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.ForestGreen;
            lblTitulo.Location = new Point(250, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(329, 37);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Cadastro de Funcionario";
            // 
            // lblNome
            // 
            lblNome.AutoSize = true;
            lblNome.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNome.Location = new Point(50, 120);
            lblNome.Name = "lblNome";
            lblNome.Size = new Size(107, 17);
            lblNome.TabIndex = 1;
            lblNome.Text = "Nome completo";
            // 
            // txtNome
            // 
            txtNome.BackColor = Color.White;
            txtNome.BorderStyle = BorderStyle.FixedSingle;
            txtNome.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNome.ForeColor = Color.Black;
            txtNome.Location = new Point(50, 145);
            txtNome.Name = "txtNome";
            txtNome.PlaceholderText = "Digite o nome completo";
            txtNome.Size = new Size(600, 27);
            txtNome.TabIndex = 4;
            // 
            // picLogo
            // 
            picLogo.BackColor = Color.Transparent;
            picLogo.Image = Properties.Resource.Logo;
            picLogo.Location = new Point(50, 15);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(150, 70);
            picLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            picLogo.TabIndex = 8;
            picLogo.TabStop = false;
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubtitulo.ForeColor = Color.DimGray;
            lblSubtitulo.Location = new Point(300, 55);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(197, 17);
            lblSubtitulo.TabIndex = 9;
            lblSubtitulo.Text = "Preencha as informações abaixo";
            // 
            // pnlLinha
            // 
            pnlLinha.BackColor = Color.ForestGreen;
            pnlLinha.Location = new Point(0, 95);
            pnlLinha.Name = "pnlLinha";
            pnlLinha.Size = new Size(700, 3);
            pnlLinha.TabIndex = 10;
            // 
            // txtEmail
            // 
            txtEmail.BackColor = Color.White;
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtEmail.ForeColor = Color.Black;
            txtEmail.Location = new Point(50, 225);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "Digite o seu usuário";
            txtEmail.Size = new Size(600, 27);
            txtEmail.TabIndex = 13;
            // 
            // txtSenha
            // 
            txtSenha.BackColor = Color.White;
            txtSenha.BorderStyle = BorderStyle.FixedSingle;
            txtSenha.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSenha.ForeColor = Color.Black;
            txtSenha.Location = new Point(50, 305);
            txtSenha.Name = "txtSenha";
            txtSenha.PlaceholderText = "Digite a senha";
            txtSenha.Size = new Size(600, 27);
            txtSenha.TabIndex = 14;
            txtSenha.UseSystemPasswordChar = true;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEmail.Location = new Point(50, 200);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(55, 17);
            lblEmail.TabIndex = 12;
            lblEmail.Text = "Usuário";
            // 
            // lblSenha
            // 
            lblSenha.AutoSize = true;
            lblSenha.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSenha.Location = new Point(50, 280);
            lblSenha.Name = "lblSenha";
            lblSenha.Size = new Size(45, 17);
            lblSenha.TabIndex = 15;
            lblSenha.Text = "Senha";
            // 
            // lblPermissao
            // 
            lblPermissao.AutoSize = true;
            lblPermissao.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPermissao.Location = new Point(50, 365);
            lblPermissao.Name = "lblPermissao";
            lblPermissao.Size = new Size(189, 17);
            lblPermissao.TabIndex = 16;
            lblPermissao.Text = "É funcionário/administrador?";
            // 
            // chkAdministrador
            // 
            chkAdministrador.AutoSize = true;
            chkAdministrador.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkAdministrador.Location = new Point(50, 395);
            chkAdministrador.Name = "chkAdministrador";
            chkAdministrador.Size = new Size(302, 21);
            chkAdministrador.TabIndex = 17;
            chkAdministrador.Text = "Sim, este usuário possui acesso administrativo ";
            chkAdministrador.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.White;
            btnCancelar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancelar.ForeColor = Color.Firebrick;
            btnCancelar.Location = new Point(180, 455);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(130, 45);
            btnCancelar.TabIndex = 18;
            btnCancelar.Text = "❌ Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnCadastrar
            // 
            btnCadastrar.BackColor = Color.ForestGreen;
            btnCadastrar.FlatAppearance.BorderSize = 0;
            btnCadastrar.FlatStyle = FlatStyle.Flat;
            btnCadastrar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCadastrar.ForeColor = Color.White;
            btnCadastrar.Location = new Point(390, 455);
            btnCadastrar.Name = "btnCadastrar";
            btnCadastrar.Size = new Size(130, 45);
            btnCadastrar.TabIndex = 19;
            btnCadastrar.Text = "💾 Cadastrar";
            btnCadastrar.UseVisualStyleBackColor = false;
            btnCadastrar.Click += btnCadastrar_Click;
            // 
            // CadastroFuncionarioForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(684, 526);
            Controls.Add(btnCadastrar);
            Controls.Add(btnCancelar);
            Controls.Add(chkAdministrador);
            Controls.Add(lblPermissao);
            Controls.Add(lblSenha);
            Controls.Add(txtSenha);
            Controls.Add(txtEmail);
            Controls.Add(lblEmail);
            Controls.Add(pnlLinha);
            Controls.Add(lblSubtitulo);
            Controls.Add(picLogo);
            Controls.Add(txtNome);
            Controls.Add(lblNome);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "CadastroFuncionarioForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cadastro de Funcionario";
            Load += CadastroFuncionarioForm_Load;
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblNome;
        private TextBox txtNome;
        private PictureBox picLogo;
        private Label lblSubtitulo;
        private Panel pnlLinha;
        private TextBox txtEmail;
        private TextBox txtSenha;
        private Label lblEmail;
        private Label lblSenha;
        private Label lblPermissao;
        private CheckBox chkAdministrador;
        private Button btnCancelar;
        private Button btnCadastrar;
    }
}