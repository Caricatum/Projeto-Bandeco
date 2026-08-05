using Google.Protobuf.WellKnownTypes;
using System.ComponentModel;

namespace ProjetoBandejao.Forms
{
    partial class RecuperarSenhaForm
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
            components = new Container();
            lblTitulo = new Label();
            lblDescricao = new Label();
            contextMenuStrip1 = new ContextMenuStrip(components);
            txtEmail = new TextBox();
            panel1 = new Panel();
            btnEnviarCodigo = new Button();
            linkLblVoltar = new LinkLabel();
            lblLembrou = new Label();
            printDocument1 = new System.Drawing.Printing.PrintDocument();
            pctrCadeado = new PictureBox();
            lblEmail = new Label();
            ((ISupportInitialize)pctrCadeado).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.Black;
            lblTitulo.Location = new Point(115, 250);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(355, 45);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Recuperação de Senha";
            // 
            // lblDescricao
            // 
            lblDescricao.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDescricao.ForeColor = Color.DimGray;
            lblDescricao.Location = new Point(90, 320);
            lblDescricao.Name = "lblDescricao";
            lblDescricao.Size = new Size(420, 70);
            lblDescricao.TabIndex = 3;
            lblDescricao.Text = "Digite seu e-mail para receber\r\num código de recuperação.";
            lblDescricao.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // txtEmail
            // 
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtEmail.Location = new Point(50, 460);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "✉️ seu@email.com";
            txtEmail.Size = new Size(500, 27);
            txtEmail.TabIndex = 6;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Gainsboro;
            panel1.Location = new Point(50, 660);
            panel1.Name = "panel1";
            panel1.Size = new Size(500, 1);
            panel1.TabIndex = 7;
            // 
            // btnEnviarCodigo
            // 
            btnEnviarCodigo.BackColor = Color.ForestGreen;
            btnEnviarCodigo.FlatAppearance.BorderSize = 0;
            btnEnviarCodigo.FlatStyle = FlatStyle.Flat;
            btnEnviarCodigo.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEnviarCodigo.ForeColor = Color.White;
            btnEnviarCodigo.Location = new Point(50, 555);
            btnEnviarCodigo.Name = "btnEnviarCodigo";
            btnEnviarCodigo.Size = new Size(500, 55);
            btnEnviarCodigo.TabIndex = 8;
            btnEnviarCodigo.Text = "📤 Enviar Código";
            btnEnviarCodigo.UseVisualStyleBackColor = false;
            btnEnviarCodigo.Click += btnEnviarCodigo_Click;
            // 
            // linkLblVoltar
            // 
            linkLblVoltar.ActiveLinkColor = Color.Green;
            linkLblVoltar.AutoSize = true;
            linkLblVoltar.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkLblVoltar.LinkColor = Color.ForestGreen;
            linkLblVoltar.Location = new Point(268, 700);
            linkLblVoltar.Name = "linkLblVoltar";
            linkLblVoltar.Size = new Size(133, 20);
            linkLblVoltar.TabIndex = 9;
            linkLblVoltar.TabStop = true;
            linkLblVoltar.Text = "Voltar para o login";
            linkLblVoltar.LinkClicked += linkLblVoltar_LinkClicked;
            // 
            // lblLembrou
            // 
            lblLembrou.AutoSize = true;
            lblLembrou.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblLembrou.ForeColor = Color.DimGray;
            lblLembrou.Location = new Point(129, 700);
            lblLembrou.Name = "lblLembrou";
            lblLembrou.Size = new Size(143, 20);
            lblLembrou.TabIndex = 10;
            lblLembrou.Text = "Lembrou sua senha?";
            // 
            // pctrCadeado
            // 
            pctrCadeado.Image = Properties.Resource.Cadeado;
            pctrCadeado.Location = new Point(90, 12);
            pctrCadeado.Name = "pctrCadeado";
            pctrCadeado.Size = new Size(420, 235);
            pctrCadeado.SizeMode = PictureBoxSizeMode.StretchImage;
            pctrCadeado.TabIndex = 11;
            pctrCadeado.TabStop = false;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEmail.ForeColor = Color.Black;
            lblEmail.Location = new Point(50, 425);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(59, 21);
            lblEmail.TabIndex = 12;
            lblEmail.Text = "E-mail";
            // 
            // RecuperarSenhaForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(584, 761);
            Controls.Add(lblEmail);
            Controls.Add(pctrCadeado);
            Controls.Add(lblLembrou);
            Controls.Add(linkLblVoltar);
            Controls.Add(btnEnviarCodigo);
            Controls.Add(panel1);
            Controls.Add(txtEmail);
            Controls.Add(lblDescricao);
            Controls.Add(lblTitulo);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "RecuperarSenhaForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RecuperarSenha";
            Load += RecuperarSenhaForm_Load;
            ((ISupportInitialize)pctrCadeado).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lblTitulo;
        private Label lblDescricao;
        private ContextMenuStrip contextMenuStrip1;
        private TextBox txtEmail;
        private Panel panel1;
        private Button btnEnviarCodigo;
        private LinkLabel linkLblVoltar;
        private Label lblLembrou;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private PictureBox pctrCadeado;
        private Label lblEmail;
    }
}