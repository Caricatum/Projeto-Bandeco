namespace ProjetoBandejao.Forms
{
    partial class HomeForm
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
            pnlSideBar = new Panel();
            btnDashboard = new Button();
            btnMural = new Button();
            btnConfiguracoes = new Button();
            btnRelatorios = new Button();
            btnFuncionarios = new Button();
            btnEstoque = new Button();
            btnCardapio = new Button();
            btnRefeicoesdoDia = new Button();
            panel1 = new Panel();
            lblTitulo = new Label();
            lblUsuario = new Label();
            lblHorario = new Label();
            lblBemVindo = new Label();
            lblSubtitulo = new Label();
            pnlCardapios = new Panel();
            lblCardapiosDesc = new Label();
            lblCardapiosTitulo = new Label();
            lblCardapiosNumero = new Label();
            pnlRefeicoes = new Panel();
            lblRefeicoesDesc = new Label();
            lblRefeioesNumero = new Label();
            lblRefeicoesTitulo = new Label();
            pnlEstoque = new Panel();
            lblEstoqueDesc = new Label();
            lblEstoqueTitulo = new Label();
            lblEstoqueNumero = new Label();
            pnlFuncionarios = new Panel();
            lblFuncionariosDesc = new Label();
            lblFuncionariosNumero = new Label();
            lblFuncionariosTitulo = new Label();
            panel2 = new Panel();
            panel3 = new Panel();
            panel4 = new Panel();
            panel5 = new Panel();
            grpBoxRefeicoes = new GroupBox();
            grpBoxAtividades = new GroupBox();
            grpBoxMural = new GroupBox();
            grpBoxEstoque = new GroupBox();
            pictureBox1 = new PictureBox();
            pnlSideBar.SuspendLayout();
            panel1.SuspendLayout();
            pnlCardapios.SuspendLayout();
            pnlRefeicoes.SuspendLayout();
            pnlEstoque.SuspendLayout();
            pnlFuncionarios.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pnlSideBar
            // 
            pnlSideBar.BackColor = Color.White;
            pnlSideBar.Controls.Add(pictureBox1);
            pnlSideBar.Location = new Point(0, 0);
            pnlSideBar.Name = "pnlSideBar";
            pnlSideBar.Size = new Size(200, 700);
            pnlSideBar.TabIndex = 0;
            // 
            // btnDashboard
            // 
            btnDashboard.BackColor = Color.ForestGreen;
            btnDashboard.FlatStyle = FlatStyle.Flat;
            btnDashboard.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDashboard.ForeColor = Color.White;
            btnDashboard.Location = new Point(15, 95);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Size = new Size(170, 38);
            btnDashboard.TabIndex = 3;
            btnDashboard.Text = "Dashboard";
            btnDashboard.TextAlign = ContentAlignment.MiddleLeft;
            btnDashboard.UseVisualStyleBackColor = false;
            // 
            // btnMural
            // 
            btnMural.FlatStyle = FlatStyle.Flat;
            btnMural.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnMural.Location = new Point(15, 138);
            btnMural.Name = "btnMural";
            btnMural.Size = new Size(170, 38);
            btnMural.TabIndex = 4;
            btnMural.Text = "Mural";
            btnMural.TextAlign = ContentAlignment.MiddleLeft;
            btnMural.UseVisualStyleBackColor = true;
            // 
            // btnConfiguracoes
            // 
            btnConfiguracoes.FlatStyle = FlatStyle.Flat;
            btnConfiguracoes.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnConfiguracoes.Location = new Point(15, 396);
            btnConfiguracoes.Name = "btnConfiguracoes";
            btnConfiguracoes.Size = new Size(170, 38);
            btnConfiguracoes.TabIndex = 5;
            btnConfiguracoes.Text = "Configurações";
            btnConfiguracoes.TextAlign = ContentAlignment.MiddleLeft;
            btnConfiguracoes.UseVisualStyleBackColor = true;
            // 
            // btnRelatorios
            // 
            btnRelatorios.FlatStyle = FlatStyle.Flat;
            btnRelatorios.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRelatorios.Location = new Point(15, 353);
            btnRelatorios.Name = "btnRelatorios";
            btnRelatorios.Size = new Size(170, 38);
            btnRelatorios.TabIndex = 6;
            btnRelatorios.Text = "Relatórios";
            btnRelatorios.TextAlign = ContentAlignment.MiddleLeft;
            btnRelatorios.UseVisualStyleBackColor = true;
            // 
            // btnFuncionarios
            // 
            btnFuncionarios.FlatStyle = FlatStyle.Flat;
            btnFuncionarios.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnFuncionarios.Location = new Point(15, 310);
            btnFuncionarios.Name = "btnFuncionarios";
            btnFuncionarios.Size = new Size(170, 38);
            btnFuncionarios.TabIndex = 7;
            btnFuncionarios.Text = "Funcionários";
            btnFuncionarios.TextAlign = ContentAlignment.MiddleLeft;
            btnFuncionarios.UseVisualStyleBackColor = true;
            // 
            // btnEstoque
            // 
            btnEstoque.FlatStyle = FlatStyle.Flat;
            btnEstoque.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEstoque.Location = new Point(15, 267);
            btnEstoque.Name = "btnEstoque";
            btnEstoque.Size = new Size(170, 38);
            btnEstoque.TabIndex = 8;
            btnEstoque.Text = "Estoque";
            btnEstoque.TextAlign = ContentAlignment.MiddleLeft;
            btnEstoque.UseVisualStyleBackColor = true;
            // 
            // btnCardapio
            // 
            btnCardapio.FlatStyle = FlatStyle.Flat;
            btnCardapio.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCardapio.Location = new Point(15, 224);
            btnCardapio.Name = "btnCardapio";
            btnCardapio.Size = new Size(170, 38);
            btnCardapio.TabIndex = 9;
            btnCardapio.Text = "Cardápio";
            btnCardapio.TextAlign = ContentAlignment.MiddleLeft;
            btnCardapio.UseVisualStyleBackColor = true;
            btnCardapio.Click += btnCardapio_Click;
            // 
            // btnRefeicoesdoDia
            // 
            btnRefeicoesdoDia.FlatStyle = FlatStyle.Flat;
            btnRefeicoesdoDia.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRefeicoesdoDia.Location = new Point(15, 181);
            btnRefeicoesdoDia.Name = "btnRefeicoesdoDia";
            btnRefeicoesdoDia.Size = new Size(170, 38);
            btnRefeicoesdoDia.TabIndex = 10;
            btnRefeicoesdoDia.Text = "Refeições do dia";
            btnRefeicoesdoDia.TextAlign = ContentAlignment.MiddleLeft;
            btnRefeicoesdoDia.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(lblTitulo);
            panel1.Controls.Add(lblUsuario);
            panel1.Controls.Add(lblHorario);
            panel1.Location = new Point(200, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(750, 50);
            panel1.TabIndex = 11;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.Location = new Point(62, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(93, 21);
            lblTitulo.TabIndex = 12;
            lblTitulo.Text = "Dashboard";
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.BackColor = Color.White;
            lblUsuario.Location = new Point(652, 20);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(58, 15);
            lblUsuario.TabIndex = 14;
            lblUsuario.Text = "🙍 Admin";
            // 
            // lblHorario
            // 
            lblHorario.AutoSize = true;
            lblHorario.BackColor = Color.White;
            lblHorario.Location = new Point(592, 20);
            lblHorario.Name = "lblHorario";
            lblHorario.Size = new Size(43, 15);
            lblHorario.TabIndex = 13;
            lblHorario.Text = "🕜 9:42";
            // 
            // lblBemVindo
            // 
            lblBemVindo.AutoSize = true;
            lblBemVindo.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBemVindo.Location = new Point(220, 70);
            lblBemVindo.Name = "lblBemVindo";
            lblBemVindo.Size = new Size(254, 21);
            lblBemVindo.TabIndex = 15;
            lblBemVindo.Text = "Bem-vindo ao Sistema RU COTIL";
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubtitulo.ForeColor = Color.Gray;
            lblSubtitulo.Location = new Point(220, 95);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(290, 13);
            lblSubtitulo.TabIndex = 16;
            lblSubtitulo.Text = "Aqui está um resumo geral do restaurante universitário";
            // 
            // pnlCardapios
            // 
            pnlCardapios.BackColor = Color.White;
            pnlCardapios.Controls.Add(lblCardapiosDesc);
            pnlCardapios.Controls.Add(lblCardapiosTitulo);
            pnlCardapios.Controls.Add(lblCardapiosNumero);
            pnlCardapios.Location = new Point(392, 122);
            pnlCardapios.Name = "pnlCardapios";
            pnlCardapios.Size = new Size(165, 90);
            pnlCardapios.TabIndex = 17;
            // 
            // lblCardapiosDesc
            // 
            lblCardapiosDesc.AutoSize = true;
            lblCardapiosDesc.Font = new Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCardapiosDesc.ForeColor = Color.Gray;
            lblCardapiosDesc.Location = new Point(12, 58);
            lblCardapiosDesc.Name = "lblCardapiosDesc";
            lblCardapiosDesc.Size = new Size(95, 12);
            lblCardapiosDesc.TabIndex = 23;
            lblCardapiosDesc.Text = "Cardápios cadastrados";
            // 
            // lblCardapiosTitulo
            // 
            lblCardapiosTitulo.AutoSize = true;
            lblCardapiosTitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCardapiosTitulo.ForeColor = Color.DimGray;
            lblCardapiosTitulo.Location = new Point(12, 40);
            lblCardapiosTitulo.Name = "lblCardapiosTitulo";
            lblCardapiosTitulo.Size = new Size(95, 13);
            lblCardapiosTitulo.TabIndex = 22;
            lblCardapiosTitulo.Text = "Cardápios Ativos";
            // 
            // lblCardapiosNumero
            // 
            lblCardapiosNumero.AutoSize = true;
            lblCardapiosNumero.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCardapiosNumero.ForeColor = Color.Black;
            lblCardapiosNumero.Location = new Point(12, 10);
            lblCardapiosNumero.Name = "lblCardapiosNumero";
            lblCardapiosNumero.Size = new Size(23, 25);
            lblCardapiosNumero.TabIndex = 21;
            lblCardapiosNumero.Text = "3";
            // 
            // pnlRefeicoes
            // 
            pnlRefeicoes.BackColor = Color.White;
            pnlRefeicoes.Controls.Add(lblRefeicoesDesc);
            pnlRefeicoes.Controls.Add(lblRefeioesNumero);
            pnlRefeicoes.Controls.Add(lblRefeicoesTitulo);
            pnlRefeicoes.Location = new Point(216, 122);
            pnlRefeicoes.Name = "pnlRefeicoes";
            pnlRefeicoes.Size = new Size(165, 90);
            pnlRefeicoes.TabIndex = 18;
            pnlRefeicoes.Tag = "320";
            // 
            // lblRefeicoesDesc
            // 
            lblRefeicoesDesc.AutoSize = true;
            lblRefeicoesDesc.Font = new Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblRefeicoesDesc.ForeColor = Color.Gray;
            lblRefeicoesDesc.Location = new Point(12, 58);
            lblRefeicoesDesc.Name = "lblRefeicoesDesc";
            lblRefeicoesDesc.Size = new Size(108, 12);
            lblRefeicoesDesc.TabIndex = 21;
            lblRefeicoesDesc.Text = "Total de refeições servidas";
            // 
            // lblRefeioesNumero
            // 
            lblRefeioesNumero.AutoSize = true;
            lblRefeioesNumero.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRefeioesNumero.ForeColor = Color.Black;
            lblRefeioesNumero.Location = new Point(12, 10);
            lblRefeioesNumero.Name = "lblRefeioesNumero";
            lblRefeioesNumero.Size = new Size(45, 25);
            lblRefeioesNumero.TabIndex = 20;
            lblRefeioesNumero.Text = "320";
            // 
            // lblRefeicoesTitulo
            // 
            lblRefeicoesTitulo.AutoSize = true;
            lblRefeicoesTitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRefeicoesTitulo.ForeColor = Color.DimGray;
            lblRefeicoesTitulo.Location = new Point(12, 40);
            lblRefeicoesTitulo.Name = "lblRefeicoesTitulo";
            lblRefeicoesTitulo.Size = new Size(83, 13);
            lblRefeicoesTitulo.TabIndex = 19;
            lblRefeicoesTitulo.Text = "Refeições Hoje";
            // 
            // pnlEstoque
            // 
            pnlEstoque.BackColor = Color.White;
            pnlEstoque.Controls.Add(lblEstoqueDesc);
            pnlEstoque.Controls.Add(lblEstoqueTitulo);
            pnlEstoque.Controls.Add(lblEstoqueNumero);
            pnlEstoque.Location = new Point(570, 122);
            pnlEstoque.Name = "pnlEstoque";
            pnlEstoque.Size = new Size(165, 90);
            pnlEstoque.TabIndex = 19;
            // 
            // lblEstoqueDesc
            // 
            lblEstoqueDesc.AutoSize = true;
            lblEstoqueDesc.Font = new Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblEstoqueDesc.ForeColor = Color.Gray;
            lblEstoqueDesc.Location = new Point(12, 58);
            lblEstoqueDesc.Name = "lblEstoqueDesc";
            lblEstoqueDesc.Size = new Size(96, 12);
            lblEstoqueDesc.TabIndex = 23;
            lblEstoqueDesc.Text = "Itens abaixo do mínimo";
            // 
            // lblEstoqueTitulo
            // 
            lblEstoqueTitulo.AutoSize = true;
            lblEstoqueTitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEstoqueTitulo.ForeColor = Color.DimGray;
            lblEstoqueTitulo.Location = new Point(12, 40);
            lblEstoqueTitulo.Name = "lblEstoqueTitulo";
            lblEstoqueTitulo.Size = new Size(81, 13);
            lblEstoqueTitulo.TabIndex = 22;
            lblEstoqueTitulo.Text = "Estoque Baixo";
            // 
            // lblEstoqueNumero
            // 
            lblEstoqueNumero.AutoSize = true;
            lblEstoqueNumero.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEstoqueNumero.ForeColor = Color.Black;
            lblEstoqueNumero.Location = new Point(12, 10);
            lblEstoqueNumero.Name = "lblEstoqueNumero";
            lblEstoqueNumero.Size = new Size(34, 25);
            lblEstoqueNumero.TabIndex = 21;
            lblEstoqueNumero.Text = "15";
            // 
            // pnlFuncionarios
            // 
            pnlFuncionarios.BackColor = Color.White;
            pnlFuncionarios.Controls.Add(lblFuncionariosDesc);
            pnlFuncionarios.Controls.Add(lblFuncionariosNumero);
            pnlFuncionarios.Controls.Add(lblFuncionariosTitulo);
            pnlFuncionarios.Location = new Point(744, 122);
            pnlFuncionarios.Name = "pnlFuncionarios";
            pnlFuncionarios.Size = new Size(165, 90);
            pnlFuncionarios.TabIndex = 20;
            pnlFuncionarios.Tag = "320";
            pnlFuncionarios.Paint += pnlFuncionarios_Paint;
            // 
            // lblFuncionariosDesc
            // 
            lblFuncionariosDesc.AutoSize = true;
            lblFuncionariosDesc.Font = new Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblFuncionariosDesc.ForeColor = Color.Gray;
            lblFuncionariosDesc.Location = new Point(12, 58);
            lblFuncionariosDesc.Name = "lblFuncionariosDesc";
            lblFuncionariosDesc.Size = new Size(104, 12);
            lblFuncionariosDesc.TabIndex = 21;
            lblFuncionariosDesc.Text = "Funcionários cadastrados";
            // 
            // lblFuncionariosNumero
            // 
            lblFuncionariosNumero.AutoSize = true;
            lblFuncionariosNumero.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFuncionariosNumero.ForeColor = Color.Black;
            lblFuncionariosNumero.Location = new Point(12, 10);
            lblFuncionariosNumero.Name = "lblFuncionariosNumero";
            lblFuncionariosNumero.Size = new Size(34, 25);
            lblFuncionariosNumero.TabIndex = 20;
            lblFuncionariosNumero.Text = "12";
            // 
            // lblFuncionariosTitulo
            // 
            lblFuncionariosTitulo.AutoSize = true;
            lblFuncionariosTitulo.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFuncionariosTitulo.ForeColor = Color.DimGray;
            lblFuncionariosTitulo.Location = new Point(12, 40);
            lblFuncionariosTitulo.Name = "lblFuncionariosTitulo";
            lblFuncionariosTitulo.Size = new Size(110, 13);
            lblFuncionariosTitulo.TabIndex = 19;
            lblFuncionariosTitulo.Text = "Funcionários Ativos";
            // 
            // panel2
            // 
            panel2.BackColor = Color.Gainsboro;
            panel2.Location = new Point(219, 125);
            panel2.Name = "panel2";
            panel2.Size = new Size(165, 90);
            panel2.TabIndex = 22;
            // 
            // panel3
            // 
            panel3.BackColor = Color.Gainsboro;
            panel3.Location = new Point(395, 125);
            panel3.Name = "panel3";
            panel3.Size = new Size(165, 90);
            panel3.TabIndex = 23;
            // 
            // panel4
            // 
            panel4.BackColor = Color.Gainsboro;
            panel4.Location = new Point(573, 125);
            panel4.Name = "panel4";
            panel4.Size = new Size(165, 90);
            panel4.TabIndex = 24;
            // 
            // panel5
            // 
            panel5.BackColor = Color.Gainsboro;
            panel5.Location = new Point(747, 125);
            panel5.Name = "panel5";
            panel5.Size = new Size(165, 90);
            panel5.TabIndex = 25;
            // 
            // grpBoxRefeicoes
            // 
            grpBoxRefeicoes.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpBoxRefeicoes.Location = new Point(220, 240);
            grpBoxRefeicoes.Name = "grpBoxRefeicoes";
            grpBoxRefeicoes.Size = new Size(400, 170);
            grpBoxRefeicoes.TabIndex = 26;
            grpBoxRefeicoes.TabStop = false;
            grpBoxRefeicoes.Text = "Refeições do dia";
            // 
            // grpBoxAtividades
            // 
            grpBoxAtividades.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpBoxAtividades.Location = new Point(635, 240);
            grpBoxAtividades.Name = "grpBoxAtividades";
            grpBoxAtividades.Size = new Size(295, 170);
            grpBoxAtividades.TabIndex = 27;
            grpBoxAtividades.TabStop = false;
            grpBoxAtividades.Text = "Últimas Atividades";
            // 
            // grpBoxMural
            // 
            grpBoxMural.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpBoxMural.Location = new Point(220, 425);
            grpBoxMural.Name = "grpBoxMural";
            grpBoxMural.Size = new Size(400, 200);
            grpBoxMural.TabIndex = 28;
            grpBoxMural.TabStop = false;
            grpBoxMural.Text = "Mural";
            // 
            // grpBoxEstoque
            // 
            grpBoxEstoque.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpBoxEstoque.Location = new Point(635, 425);
            grpBoxEstoque.Name = "grpBoxEstoque";
            grpBoxEstoque.Size = new Size(295, 200);
            grpBoxEstoque.TabIndex = 29;
            grpBoxEstoque.TabStop = false;
            grpBoxEstoque.Text = "Estoque com Atenção";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resource.Logo;
            pictureBox1.Location = new Point(15, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(170, 65);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // HomeForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1066, 661);
            Controls.Add(grpBoxEstoque);
            Controls.Add(grpBoxMural);
            Controls.Add(grpBoxAtividades);
            Controls.Add(grpBoxRefeicoes);
            Controls.Add(pnlFuncionarios);
            Controls.Add(panel5);
            Controls.Add(pnlEstoque);
            Controls.Add(panel4);
            Controls.Add(pnlCardapios);
            Controls.Add(panel3);
            Controls.Add(pnlRefeicoes);
            Controls.Add(panel2);
            Controls.Add(lblSubtitulo);
            Controls.Add(lblBemVindo);
            Controls.Add(panel1);
            Controls.Add(btnRefeicoesdoDia);
            Controls.Add(btnCardapio);
            Controls.Add(btnEstoque);
            Controls.Add(btnFuncionarios);
            Controls.Add(btnRelatorios);
            Controls.Add(btnConfiguracoes);
            Controls.Add(btnMural);
            Controls.Add(btnDashboard);
            Controls.Add(pnlSideBar);
            Name = "HomeForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "HomeForm";
            Load += HomeForm_Load;
            pnlSideBar.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            pnlCardapios.ResumeLayout(false);
            pnlCardapios.PerformLayout();
            pnlRefeicoes.ResumeLayout(false);
            pnlRefeicoes.PerformLayout();
            pnlEstoque.ResumeLayout(false);
            pnlEstoque.PerformLayout();
            pnlFuncionarios.ResumeLayout(false);
            pnlFuncionarios.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlSideBar;
        private Button btnDashboard;
        private Button btnMural;
        private Button btnConfiguracoes;
        private Button btnRelatorios;
        private Button btnFuncionarios;
        private Button btnEstoque;
        private Button btnCardapio;
        private Button btnRefeicoesdoDia;
        private Panel panel1;
        private Label lblTitulo;
        private Label lblHorario;
        private Label lblUsuario;
        private Label lblBemVindo;
        private Label lblSubtitulo;
        private Panel pnlCardapios;
        private Panel pnlRefeicoes;
        private Label lblRefeicoesTitulo;
        private Label lblRefeioesNumero;
        private Label lblRefeicoesDesc;
        private Label lblCardapiosDesc;
        private Label lblCardapiosTitulo;
        private Label lblCardapiosNumero;
        private Panel pnlEstoque;
        private Label lblEstoqueDesc;
        private Label lblEstoqueTitulo;
        private Label lblEstoqueNumero;
        private Panel pnlFuncionarios;
        private Label lblFuncionariosDesc;
        private Label lblFuncionariosNumero;
        private Label lblFuncionariosTitulo;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        private GroupBox grpBoxRefeicoes;
        private GroupBox grpBoxAtividades;
        private GroupBox grpBoxMural;
        private GroupBox grpBoxEstoque;
        private PictureBox pictureBox1;
    }
}