import os

sections = [
    ("Usuarios", "Usuários e Permissões", "Gerencie funcionários e níveis de acesso ao sistema."),
    ("Cardapio", "Cardápio e Refeições", "Configurações do Cardápio, Horários das Refeições, Destaques, Ingredientes."),
    ("Mural", "Mural e Avisos", "Categorias de Avisos, Modelos de Avisos, Prazos Padrão, Configurações de Exibição."),
    ("Notificacoes", "Notificações", "Configurar canais de notificação para eventos do sistema."),
    ("Feedback", "Feedback dos Alunos", "Ativar feedback diário, exibir comentários e relatórios automáticos."),
    ("Estoque", "Estoque e Compras", "Alertas de estoque baixo, quantidade mínima padrão e solicitação de compras."),
    ("Sistema", "Sistema", "Tema do sistema, Idioma, e Atualizações."),
    ("Backup", "Backup e Segurança", "Backup automático, Frequência do backup, e Restaurar backup."),
    ("Sobre", "Sobre o Sistema", "Versão do sistema, Última atualização, e Desenvolvido para o Bandejão do Cotil.")
]

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
"""

for id, title, desc in sections:
    designer_cs += f"            this.btnNav{id} = new Guna.UI2.WinForms.Guna2Button();\n"
    designer_cs += f"            this.pnlSecao{id} = new Guna.UI2.WinForms.Guna2Panel();\n"
    designer_cs += f"            this.lblTitulo{id} = new System.Windows.Forms.Label();\n"
    designer_cs += f"            this.lblDesc{id} = new System.Windows.Forms.Label();\n"

designer_cs += """
            this.pnlSidebar.SuspendLayout();
            this.pnlContent.SuspendLayout();
"""

for id, title, desc in sections:
    designer_cs += f"            this.pnlSecao{id}.SuspendLayout();\n"

designer_cs += """            this.SuspendLayout();

            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.White;
            this.pnlSidebar.Controls.Add(this.lblTituloSidebar);
"""

for id, title, desc in sections:
    designer_cs += f"            this.pnlSidebar.Controls.Add(this.btnNav{id});\n"

designer_cs += """
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
"""

current_y = 20
spacing = 20
panel_height = 200 # approximate for each section

for i, (id, title, desc) in enumerate(sections):
    designer_cs += f"""
            // 
            // btnNav{id}
            // 
            this.btnNav{id}.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnNav{id}.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnNav{id}.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnNav{id}.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnNav{id}.FillColor = System.Drawing.Color.Transparent;
            this.btnNav{id}.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnNav{id}.ForeColor = System.Drawing.Color.Gray;
            this.btnNav{id}.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnNav{id}.HoverState.ForeColor = System.Drawing.Color.Black;
            this.btnNav{id}.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnNav{id}.Location = new System.Drawing.Point(0, {90 + (i * 45)});
            this.btnNav{id}.Name = "btnNav{id}";
            this.btnNav{id}.Size = new System.Drawing.Size(250, 45);
            this.btnNav{id}.TabIndex = {i + 1};
            this.btnNav{id}.Text = "{title}";
            this.btnNav{id}.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnNav{id}.TextOffset = new System.Drawing.Point(20, 0);
            this.btnNav{id}.Click += new System.EventHandler(this.btnNav_Click);
            this.btnNav{id}.Tag = this.pnlSecao{id};
            this.btnNav{id}.Cursor = System.Windows.Forms.Cursors.Hand;

            // 
            // pnlSecao{id}
            // 
            this.pnlSecao{id}.BackColor = System.Drawing.Color.White;
            this.pnlSecao{id}.BorderColor = System.Drawing.Color.Gainsboro;
            this.pnlSecao{id}.BorderRadius = 8;
            this.pnlSecao{id}.BorderThickness = 1;
            this.pnlSecao{id}.Controls.Add(this.lblDesc{id});
            this.pnlSecao{id}.Controls.Add(this.lblTitulo{id});
            this.pnlSecao{id}.Location = new System.Drawing.Point(40, {current_y});
            this.pnlSecao{id}.Name = "pnlSecao{id}";
            this.pnlSecao{id}.Size = new System.Drawing.Size(800, {panel_height});
            this.pnlSecao{id}.TabIndex = {i};
            
            // 
            // lblTitulo{id}
            // 
            this.lblTitulo{id}.AutoSize = true;
            this.lblTitulo{id}.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo{id}.Location = new System.Drawing.Point(20, 20);
            this.lblTitulo{id}.Name = "lblTitulo{id}";
            this.lblTitulo{id}.Size = new System.Drawing.Size(100, 25);
            this.lblTitulo{id}.TabIndex = 0;
            this.lblTitulo{id}.Text = "{title}";

            // 
            // lblDesc{id}
            // 
            this.lblDesc{id}.AutoSize = true;
            this.lblDesc{id}.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDesc{id}.ForeColor = System.Drawing.Color.Gray;
            this.lblDesc{id}.Location = new System.Drawing.Point(20, 50);
            this.lblDesc{id}.Name = "lblDesc{id}";
            this.lblDesc{id}.Size = new System.Drawing.Size(100, 19);
            this.lblDesc{id}.TabIndex = 1;
            this.lblDesc{id}.Text = "{desc}";

"""
    current_y += panel_height + spacing

for id, title, desc in sections:
    designer_cs += f"            this.pnlContent.Controls.Add(this.pnlSecao{id});\n"

designer_cs += """
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
"""
for id, title, desc in sections:
    designer_cs += f"            this.pnlSecao{id}.ResumeLayout(false);\n"
    designer_cs += f"            this.pnlSecao{id}.PerformLayout();\n"

designer_cs += """            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlSidebar;
        private System.Windows.Forms.Label lblTituloSidebar;
        private Guna.UI2.WinForms.Guna2Panel pnlContent;
"""

for id, title, desc in sections:
    designer_cs += f"        private Guna.UI2.WinForms.Guna2Button btnNav{id};\n"
    designer_cs += f"        private Guna.UI2.WinForms.Guna2Panel pnlSecao{id};\n"
    designer_cs += f"        private System.Windows.Forms.Label lblTitulo{id};\n"
    designer_cs += f"        private System.Windows.Forms.Label lblDesc{id};\n"

designer_cs += """    }
}
"""

with open("ConfiguracoesForm.Designer.cs", "w", encoding="utf-8") as f:
    f.write(designer_cs)

print("Saved to ConfiguracoesForm.Designer.cs")
