import os

designer_cs = """namespace ProjetoBandejao.Forms.Home
{
    partial class ConfiguracoesForm
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlSidebar = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTituloSidebar = new System.Windows.Forms.Label();
            this.pnlContent = new Guna.UI2.WinForms.Guna2Panel();
            this.lblDescDesign = new System.Windows.Forms.Label();
            
            this.pnlSidebar.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.White;
            this.pnlSidebar.Controls.Add(this.lblTituloSidebar);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(250, 716);
            this.pnlSidebar.TabIndex = 0;
            this.pnlSidebar.BorderThickness = 0;
            this.pnlSidebar.CustomBorderColor = System.Drawing.Color.Gainsboro;
            this.pnlSidebar.CustomBorderThickness = new System.Windows.Forms.Padding(0, 0, 1, 0);

            // 
            // lblTituloSidebar
            // 
            this.lblTituloSidebar.AutoSize = true;
            this.lblTituloSidebar.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTituloSidebar.Location = new System.Drawing.Point(20, 30);
            this.lblTituloSidebar.Name = "lblTituloSidebar";
            this.lblTituloSidebar.Size = new System.Drawing.Size(161, 30);
            this.lblTituloSidebar.TabIndex = 0;
            this.lblTituloSidebar.Text = "Configurações";

            // 
            // pnlContent
            // 
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(250, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(917, 716);
            this.pnlContent.TabIndex = 1;
            this.pnlContent.AutoScroll = true;
            this.pnlContent.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlContent.Controls.Add(this.lblDescDesign);

            // 
            // lblDescDesign
            // 
            this.lblDescDesign.AutoSize = true;
            this.lblDescDesign.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular);
            this.lblDescDesign.ForeColor = System.Drawing.Color.Gray;
            this.lblDescDesign.Location = new System.Drawing.Point(40, 40);
            this.lblDescDesign.Name = "lblDescDesign";
            this.lblDescDesign.Size = new System.Drawing.Size(700, 25);
            this.lblDescDesign.TabIndex = 0;
            this.lblDescDesign.Text = "Os componentes visuais desta tela são gerados dinamicamente ao executar o projeto.";
            
            // 
            // ConfiguracoesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1167, 716);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlSidebar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ConfiguracoesForm";
            this.Text = "Configurações";
            this.Load += new System.EventHandler(this.ConfiguracoesForm_Load);
            
            this.pnlSidebar.ResumeLayout(false);
            this.pnlSidebar.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlSidebar;
        private System.Windows.Forms.Label lblTituloSidebar;
        private Guna.UI2.WinForms.Guna2Panel pnlContent;
        private System.Windows.Forms.Label lblDescDesign;
    }
}
"""
with open("ConfiguracoesForm.Designer.cs", "w", encoding="utf-8") as f:
    f.write(designer_cs)
