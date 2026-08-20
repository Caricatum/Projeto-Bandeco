import os

theme_color = "System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(75)))), ((int)(((byte)(43)))))"
bg_alert = "System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(235)))), ((int)(((byte)(230)))))"

sections = [
    {
        "id": "Usuarios",
        "title": "Usuários e Permissões",
        "desc": "Gerencie funcionários e níveis de acesso ao sistema.",
        "items": [
            {"type": "action", "title": "Funcionários Cadastrados", "desc": "Visualize, edite ou cadastre funcionários."},
            {"type": "action", "title": "Perfis de Acesso", "desc": "Defina permissões para cada perfil."},
            {"type": "action", "title": "Trocar Minha Senha", "desc": "Altere sua senha de acesso."}
        ]
    },
    {
        "id": "Cardapio",
        "title": "Cardápio e Refeições",
        "desc": "Configurações gerais de alimentação e cardápio.",
        "items": [
            {"type": "action", "title": "Configurações do Cardápio", "desc": "Ative ou desative informações exibidas."},
            {"type": "action", "title": "Horários das Refeições", "desc": "Defina os horários de início e fim."},
            {"type": "action", "title": "Destaques do Dia", "desc": "Gerencie os destaques exibidos na tela inicial."},
            {"type": "action", "title": "Ingredientes e Alergênicos", "desc": "Gerencie lista de ingredientes e alertas."}
        ]
    },
    {
        "id": "Mural",
        "title": "Mural e Avisos",
        "desc": "Configurações do mural de avisos da página inicial.",
        "items": [
            {"type": "action", "title": "Categorias de Avisos", "desc": "Crie e gerencie categorias de avisos."},
            {"type": "action", "title": "Modelos de Avisos", "desc": "Crie modelos para avisos frequentes."},
            {"type": "action", "title": "Prazos Padrão", "desc": "Defina prazos padrão para publicação."},
            {"type": "action", "title": "Configurações de Exibição", "desc": "Defina como os avisos serão exibidos."}
        ]
    },
    {
        "id": "Notificacoes",
        "title": "Notificações",
        "desc": "Alertas e notificações do sistema.",
        "items": [
            {"type": "toggle", "title": "Novos avisos publicados", "desc": "", "checked": "true"},
            {"type": "toggle", "title": "Alterações no cardápio", "desc": "", "checked": "true"},
            {"type": "toggle", "title": "Feedbacks dos alunos", "desc": "", "checked": "true"},
            {"type": "toggle", "title": "Baixo estoque", "desc": "", "checked": "true"},
            {"type": "toggle", "title": "Manutenções do sistema", "desc": "", "checked": "true"},
            {"type": "button_center", "title": "Configurar canais de notificação"}
        ]
    },
    {
        "id": "Feedback",
        "title": "Feedback dos Alunos",
        "desc": "Gerencie as avaliações diárias das refeições.",
        "items": [
            {"type": "toggle", "title": "Ativar feedback diário", "desc": "Permite que alunos avaliem as refeições.", "checked": "true"},
            {"type": "toggle", "title": "Exibir comentários dos alunos", "desc": "Mostra comentários no painel de funcionários.", "checked": "true"},
            {"type": "toggle", "title": "Relatórios automáticos", "desc": "Receba relatórios semanais por e-mail.", "checked": "false"},
            {"type": "button_center", "title": "Configurar perguntas"}
        ]
    },
    {
        "id": "Estoque",
        "title": "Estoque e Compras",
        "desc": "Configurações de inventário e alertas de compras.",
        "items": [
            {"type": "toggle", "title": "Alertas de estoque baixo", "desc": "Receba alertas quando itens estiverem baixos.", "checked": "true"},
            {"type": "input", "title": "Quantidade mínima padrão", "desc": "Defina o valor padrão para alertas.", "val": "10", "suffix": "un."},
            {"type": "toggle", "title": "Solicitação de compras", "desc": "Ative o fluxo de solicitação de compras.", "checked": "true"},
            {"type": "button_center", "title": "Categorias de itens"}
        ]
    },
    {
        "id": "Sistema",
        "title": "Sistema",
        "desc": "Preferências gerais do software.",
        "items": [
            {"type": "combo", "title": "Tema do sistema", "desc": "Escolha o tema de cores da aplicação.", "options": ["Cotil (Laranja)", "Escuro"]},
            {"type": "combo", "title": "Idioma", "desc": "Selecione o idioma do sistema.", "options": ["Português (Brasil)", "English (US)"]},
            {"type": "button", "title": "Atualizações", "desc": "Verifique e aplique atualizações do sistema.", "btn": "Verificar agora"}
        ]
    },
    {
        "id": "Backup",
        "title": "Backup e Segurança",
        "desc": "Proteção de dados e histórico.",
        "items": [
            {"type": "toggle", "title": "Backup automático", "desc": "Realizar backup automático dos dados.", "checked": "true"},
            {"type": "combo", "title": "Frequência do backup", "desc": "Escolha a frequência dos backups.", "options": ["Diário", "Semanal", "Mensal"]},
            {"type": "button", "title": "Restaurar backup", "desc": "Restaure dados a partir de um backup.", "btn": "Restaurar"}
        ]
    },
    {
        "id": "Sobre",
        "title": "Sobre o Sistema",
        "desc": "Informações sobre a licença e versão.",
        "items": [
            {"type": "info", "title": "Versão do sistema", "val": "v1.0.0", "highlight": True},
            {"type": "info", "title": "Última atualização", "val": "10/08/2025 10:45", "highlight": False},
            {"type": "info", "title": "Desenvolvido para o", "val": "Bandejão do Cotil", "highlight": False},
            {"type": "alert", "text": "Sistema interno de uso exclusivo de funcionários do restaurante."}
        ]
    }
]

out = []
out.append("""namespace ProjetoBandejao.Forms.Home
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
""")

# Declare components
out.append("            this.pnlSidebar = new Guna.UI2.WinForms.Guna2Panel();\n")
out.append("            this.lblTituloSidebar = new System.Windows.Forms.Label();\n")
out.append("            this.pnlContent = new Guna.UI2.WinForms.Guna2Panel();\n")

for sec in sections:
    sid = sec['id']
    out.append(f"            this.btnNav{sid} = new Guna.UI2.WinForms.Guna2Button();\n")
    out.append(f"            this.pnlSecao{sid} = new Guna.UI2.WinForms.Guna2Panel();\n")
    out.append(f"            this.lblTitulo{sid} = new System.Windows.Forms.Label();\n")
    out.append(f"            this.lblDesc{sid} = new System.Windows.Forms.Label();\n")
    
    for j, item in enumerate(sec['items']):
        if item['type'] in ['action', 'toggle', 'combo', 'input', 'button', 'info']:
            out.append(f"            this.lblTitle_{sid}_{j} = new System.Windows.Forms.Label();\n")
            if 'desc' in item and item['desc']:
                out.append(f"            this.lblDesc_{sid}_{j} = new System.Windows.Forms.Label();\n")
            
            if item['type'] == 'action':
                out.append(f"            this.lblChev_{sid}_{j} = new System.Windows.Forms.Label();\n")
            elif item['type'] == 'toggle':
                out.append(f"            this.tgl_{sid}_{j} = new Guna.UI2.WinForms.Guna2ToggleSwitch();\n")
            elif item['type'] == 'combo':
                out.append(f"            this.cmb_{sid}_{j} = new Guna.UI2.WinForms.Guna2ComboBox();\n")
            elif item['type'] == 'input':
                out.append(f"            this.txt_{sid}_{j} = new Guna.UI2.WinForms.Guna2TextBox();\n")
                out.append(f"            this.lblSuf_{sid}_{j} = new System.Windows.Forms.Label();\n")
            elif item['type'] == 'button':
                out.append(f"            this.btnAction_{sid}_{j} = new Guna.UI2.WinForms.Guna2Button();\n")
            elif item['type'] == 'info':
                out.append(f"            this.lblVal_{sid}_{j} = new System.Windows.Forms.Label();\n")
                
            out.append(f"            this.div_{sid}_{j} = new Guna.UI2.WinForms.Guna2Panel();\n")
            
        elif item['type'] == 'button_center':
            out.append(f"            this.btnCenter_{sid}_{j} = new Guna.UI2.WinForms.Guna2Button();\n")
        elif item['type'] == 'alert':
            out.append(f"            this.pnlAlert_{sid}_{j} = new Guna.UI2.WinForms.Guna2Panel();\n")
            out.append(f"            this.lblAlert_{sid}_{j} = new System.Windows.Forms.Label();\n")

out.append("""
            this.pnlSidebar.SuspendLayout();
            this.pnlContent.SuspendLayout();
""")
for sec in sections:
    out.append(f"            this.pnlSecao{sec['id']}.SuspendLayout();\n")

out.append("            this.SuspendLayout();\n")

# Settings for Sidebar
out.append("""
            // pnlSidebar
            this.pnlSidebar.BackColor = System.Drawing.Color.White;
            this.pnlSidebar.Controls.Add(this.lblTituloSidebar);
""")
for sec in sections:
    out.append(f"            this.pnlSidebar.Controls.Add(this.btnNav{sec['id']});\n")

out.append("""
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(250, 716);
            this.pnlSidebar.TabIndex = 0;
            this.pnlSidebar.BorderThickness = 0;
            this.pnlSidebar.CustomBorderColor = System.Drawing.Color.Gainsboro;
            this.pnlSidebar.CustomBorderThickness = new System.Windows.Forms.Padding(0, 0, 1, 0);

            // lblTituloSidebar
            this.lblTituloSidebar.AutoSize = true;
            this.lblTituloSidebar.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTituloSidebar.Location = new System.Drawing.Point(20, 30);
            this.lblTituloSidebar.Name = "lblTituloSidebar";
            this.lblTituloSidebar.Size = new System.Drawing.Size(161, 30);
            this.lblTituloSidebar.TabIndex = 0;
            this.lblTituloSidebar.Text = "Configurações";
            
            // pnlContent
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(250, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(917, 716);
            this.pnlContent.TabIndex = 1;
            this.pnlContent.AutoScroll = true;
            this.pnlContent.BackColor = System.Drawing.Color.WhiteSmoke;
""")

content_y = 20

for i, sec in enumerate(sections):
    sid = sec['id']
    # Sidebar button
    out.append(f"""
            // btnNav{sid}
            this.btnNav{sid}.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnNav{sid}.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnNav{sid}.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnNav{sid}.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnNav{sid}.FillColor = System.Drawing.Color.Transparent;
            this.btnNav{sid}.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnNav{sid}.ForeColor = System.Drawing.Color.Gray;
            this.btnNav{sid}.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnNav{sid}.HoverState.ForeColor = System.Drawing.Color.Black;
            this.btnNav{sid}.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnNav{sid}.Location = new System.Drawing.Point(0, {90 + (i * 45)});
            this.btnNav{sid}.Name = "btnNav{sid}";
            this.btnNav{sid}.Size = new System.Drawing.Size(250, 45);
            this.btnNav{sid}.TabIndex = {i + 1};
            this.btnNav{sid}.Text = "{sec['title']}";
            this.btnNav{sid}.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnNav{sid}.TextOffset = new System.Drawing.Point(20, 0);
            this.btnNav{sid}.Click += new System.EventHandler(this.btnNav_Click);
            this.btnNav{sid}.Tag = this.pnlSecao{sid};
            this.btnNav{sid}.Cursor = System.Windows.Forms.Cursors.Hand;
            """)
    
    # Calculate panel height
    y_pos = 90
    for j, item in enumerate(sec['items']):
        if item['type'] in ['action', 'toggle']:
            y_pos += 60 if 'desc' in item and item['desc'] else 50
        elif item['type'] in ['combo', 'input', 'button']:
            y_pos += 70
        elif item['type'] == 'button_center':
            y_pos += 70
        elif item['type'] == 'info':
            y_pos += 45
        elif item['type'] == 'alert':
            y_pos += 80
            
    pnl_height = y_pos

    # Section Panel
    out.append(f"""
            // pnlSecao{sid}
            this.pnlSecao{sid}.BackColor = System.Drawing.Color.White;
            this.pnlSecao{sid}.BorderColor = System.Drawing.Color.Gainsboro;
            this.pnlSecao{sid}.BorderRadius = 8;
            this.pnlSecao{sid}.BorderThickness = 1;
            this.pnlSecao{sid}.Location = new System.Drawing.Point(40, {content_y});
            this.pnlSecao{sid}.Name = "pnlSecao{sid}";
            this.pnlSecao{sid}.Size = new System.Drawing.Size(800, {pnl_height});
            this.pnlSecao{sid}.TabIndex = {i};
            this.pnlSecao{sid}.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.pnlSecao{sid}.Controls.Add(this.lblTitulo{sid});
            this.pnlSecao{sid}.Controls.Add(this.lblDesc{sid});
            """)
    
    # Title and Desc
    out.append(f"""
            // lblTitulo{sid}
            this.lblTitulo{sid}.AutoSize = true;
            this.lblTitulo{sid}.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo{sid}.Location = new System.Drawing.Point(20, 20);
            this.lblTitulo{sid}.Name = "lblTitulo{sid}";
            this.lblTitulo{sid}.Size = new System.Drawing.Size(100, 25);
            this.lblTitulo{sid}.TabIndex = 0;
            this.lblTitulo{sid}.Text = "{sec['title']}";
            
            // lblDesc{sid}
            this.lblDesc{sid}.AutoSize = true;
            this.lblDesc{sid}.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDesc{sid}.ForeColor = System.Drawing.Color.Gray;
            this.lblDesc{sid}.Location = new System.Drawing.Point(20, 50);
            this.lblDesc{sid}.Name = "lblDesc{sid}";
            this.lblDesc{sid}.Size = new System.Drawing.Size(100, 19);
            this.lblDesc{sid}.TabIndex = 1;
            this.lblDesc{sid}.Text = "{sec['desc']}";
            """)

    content_y += pnl_height + 20
    
    y_pos = 90
    for j, item in enumerate(sec['items']):
        t = item['type']
        
        # Add basic titles
        if t in ['action', 'toggle', 'combo', 'input', 'button', 'info']:
            out.append(f"""
            // lblTitle_{sid}_{j}
            this.lblTitle_{sid}_{j}.AutoSize = true;
            this.lblTitle_{sid}_{j}.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblTitle_{sid}_{j}.Location = new System.Drawing.Point(20, {y_pos});
            this.lblTitle_{sid}_{j}.Name = "lblTitle_{sid}_{j}";
            this.lblTitle_{sid}_{j}.Size = new System.Drawing.Size(100, 20);
            this.lblTitle_{sid}_{j}.TabIndex = 2;
            this.lblTitle_{sid}_{j}.Text = "{item['title']}";
            this.pnlSecao{sid}.Controls.Add(this.lblTitle_{sid}_{j});
            """)
            
            if 'desc' in item and item['desc']:
                out.append(f"""
            // lblDesc_{sid}_{j}
            this.lblDesc_{sid}_{j}.AutoSize = true;
            this.lblDesc_{sid}_{j}.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDesc_{sid}_{j}.ForeColor = System.Drawing.Color.Gray;
            this.lblDesc_{sid}_{j}.Location = new System.Drawing.Point(20, {y_pos + 22});
            this.lblDesc_{sid}_{j}.Name = "lblDesc_{sid}_{j}";
            this.lblDesc_{sid}_{j}.Size = new System.Drawing.Size(100, 15);
            this.lblDesc_{sid}_{j}.TabIndex = 3;
            this.lblDesc_{sid}_{j}.Text = "{item['desc']}";
            this.pnlSecao{sid}.Controls.Add(this.lblDesc_{sid}_{j});
            """)

        # Item specific
        if t == 'action':
            out.append(f"""
            this.lblChev_{sid}_{j}.AutoSize = true;
            this.lblChev_{sid}_{j}.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblChev_{sid}_{j}.ForeColor = System.Drawing.Color.Gray;
            this.lblChev_{sid}_{j}.Location = new System.Drawing.Point(740, {y_pos + 5});
            this.lblChev_{sid}_{j}.Name = "lblChev_{sid}_{j}";
            this.lblChev_{sid}_{j}.Size = new System.Drawing.Size(21, 21);
            this.lblChev_{sid}_{j}.TabIndex = 4;
            this.lblChev_{sid}_{j}.Text = ">";
            this.lblChev_{sid}_{j}.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.lblChev_{sid}_{j}.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlSecao{sid}.Controls.Add(this.lblChev_{sid}_{j});
            """)
            div_y = y_pos + 45
            y_pos += 60
            
        elif t == 'toggle':
            out.append(f"""
            this.tgl_{sid}_{j}.Location = new System.Drawing.Point(715, {y_pos + 5});
            this.tgl_{sid}_{j}.Name = "tgl_{sid}_{j}";
            this.tgl_{sid}_{j}.Size = new System.Drawing.Size(45, 22);
            this.tgl_{sid}_{j}.TabIndex = 4;
            this.tgl_{sid}_{j}.Checked = {item['checked']};
            this.tgl_{sid}_{j}.CheckedState.FillColor = {theme_color};
            this.tgl_{sid}_{j}.CheckedState.BorderColor = {theme_color};
            this.tgl_{sid}_{j}.CheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.tgl_{sid}_{j}.CheckedState.InnerColor = System.Drawing.Color.White;
            this.tgl_{sid}_{j}.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.pnlSecao{sid}.Controls.Add(this.tgl_{sid}_{j});
            """)
            div_y = y_pos + (45 if 'desc' in item and item['desc'] else 35)
            y_pos += 60 if 'desc' in item and item['desc'] else 50
            
        elif t == 'combo':
            out.append(f"""
            this.cmb_{sid}_{j}.Location = new System.Drawing.Point(560, {y_pos});
            this.cmb_{sid}_{j}.Name = "cmb_{sid}_{j}";
            this.cmb_{sid}_{j}.Size = new System.Drawing.Size(180, 36);
            this.cmb_{sid}_{j}.TabIndex = 4;
            this.cmb_{sid}_{j}.BackColor = System.Drawing.Color.Transparent;
            this.cmb_{sid}_{j}.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmb_{sid}_{j}.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_{sid}_{j}.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmb_{sid}_{j}.ForeColor = System.Drawing.Color.Black;
            this.cmb_{sid}_{j}.ItemHeight = 30;
            this.cmb_{sid}_{j}.BorderRadius = 4;
            this.cmb_{sid}_{j}.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.cmb_{sid}_{j}.Items.AddRange(new object[] {{ {', '.join([f'"{o}"' for o in item['options']])} }});
            this.cmb_{sid}_{j}.SelectedIndex = 0;
            this.pnlSecao{sid}.Controls.Add(this.cmb_{sid}_{j});
            """)
            div_y = y_pos + 55
            y_pos += 70
            
        elif t == 'input':
            out.append(f"""
            this.txt_{sid}_{j}.Location = new System.Drawing.Point(620, {y_pos});
            this.txt_{sid}_{j}.Name = "txt_{sid}_{j}";
            this.txt_{sid}_{j}.Size = new System.Drawing.Size(80, 30);
            this.txt_{sid}_{j}.TabIndex = 4;
            this.txt_{sid}_{j}.Text = "{item['val']}";
            this.txt_{sid}_{j}.BorderRadius = 4;
            this.txt_{sid}_{j}.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_{sid}_{j}.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.pnlSecao{sid}.Controls.Add(this.txt_{sid}_{j});
            
            this.lblSuf_{sid}_{j}.AutoSize = true;
            this.lblSuf_{sid}_{j}.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSuf_{sid}_{j}.Location = new System.Drawing.Point(710, {y_pos + 5});
            this.lblSuf_{sid}_{j}.Name = "lblSuf_{sid}_{j}";
            this.lblSuf_{sid}_{j}.Size = new System.Drawing.Size(29, 19);
            this.lblSuf_{sid}_{j}.TabIndex = 5;
            this.lblSuf_{sid}_{j}.Text = "{item['suffix']}";
            this.lblSuf_{sid}_{j}.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.pnlSecao{sid}.Controls.Add(this.lblSuf_{sid}_{j});
            """)
            div_y = y_pos + 55
            y_pos += 70
            
        elif t == 'button':
            out.append(f"""
            this.btnAction_{sid}_{j}.Location = new System.Drawing.Point(600, {y_pos});
            this.btnAction_{sid}_{j}.Name = "btnAction_{sid}_{j}";
            this.btnAction_{sid}_{j}.Size = new System.Drawing.Size(140, 36);
            this.btnAction_{sid}_{j}.TabIndex = 4;
            this.btnAction_{sid}_{j}.Text = "{item['btn']}";
            this.btnAction_{sid}_{j}.BorderRadius = 4;
            this.btnAction_{sid}_{j}.FillColor = System.Drawing.Color.White;
            this.btnAction_{sid}_{j}.ForeColor = {theme_color};
            this.btnAction_{sid}_{j}.BorderColor = {theme_color};
            this.btnAction_{sid}_{j}.BorderThickness = 1;
            this.btnAction_{sid}_{j}.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnAction_{sid}_{j}.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.pnlSecao{sid}.Controls.Add(this.btnAction_{sid}_{j});
            """)
            div_y = y_pos + 55
            y_pos += 70
            
        elif t == 'button_center':
            out.append(f"""
            this.btnCenter_{sid}_{j}.Location = new System.Drawing.Point(275, {y_pos + 10});
            this.btnCenter_{sid}_{j}.Name = "btnCenter_{sid}_{j}";
            this.btnCenter_{sid}_{j}.Size = new System.Drawing.Size(250, 40);
            this.btnCenter_{sid}_{j}.TabIndex = 4;
            this.btnCenter_{sid}_{j}.Text = "{item['title']}";
            this.btnCenter_{sid}_{j}.BorderRadius = 4;
            this.btnCenter_{sid}_{j}.FillColor = System.Drawing.Color.White;
            this.btnCenter_{sid}_{j}.ForeColor = {theme_color};
            this.btnCenter_{sid}_{j}.BorderColor = {theme_color};
            this.btnCenter_{sid}_{j}.BorderThickness = 1;
            this.btnCenter_{sid}_{j}.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnCenter_{sid}_{j}.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pnlSecao{sid}.Controls.Add(this.btnCenter_{sid}_{j});
            """)
            y_pos += 70
            continue # No divider for centered button
            
        elif t == 'info':
            hl_color = theme_color if item['highlight'] else "System.Drawing.Color.Gray"
            out.append(f"""
            this.lblTitle_{sid}_{j}.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            
            this.lblVal_{sid}_{j}.Location = new System.Drawing.Point(600, {y_pos});
            this.lblVal_{sid}_{j}.Name = "lblVal_{sid}_{j}";
            this.lblVal_{sid}_{j}.Size = new System.Drawing.Size(140, 20);
            this.lblVal_{sid}_{j}.TabIndex = 4;
            this.lblVal_{sid}_{j}.Text = "{item['val']}";
            this.lblVal_{sid}_{j}.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblVal_{sid}_{j}.ForeColor = {hl_color};
            this.lblVal_{sid}_{j}.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblVal_{sid}_{j}.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.pnlSecao{sid}.Controls.Add(this.lblVal_{sid}_{j});
            """)
            div_y = y_pos + 30
            y_pos += 45
            
        elif t == 'alert':
            out.append(f"""
            this.pnlAlert_{sid}_{j}.Location = new System.Drawing.Point(20, {y_pos + 10});
            this.pnlAlert_{sid}_{j}.Name = "pnlAlert_{sid}_{j}";
            this.pnlAlert_{sid}_{j}.Size = new System.Drawing.Size(760, 50);
            this.pnlAlert_{sid}_{j}.TabIndex = 4;
            this.pnlAlert_{sid}_{j}.BackColor = {bg_alert};
            this.pnlAlert_{sid}_{j}.BorderRadius = 4;
            this.pnlAlert_{sid}_{j}.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.pnlSecao{sid}.Controls.Add(this.pnlAlert_{sid}_{j});
            
            this.lblAlert_{sid}_{j}.AutoSize = true;
            this.lblAlert_{sid}_{j}.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblAlert_{sid}_{j}.ForeColor = System.Drawing.Color.Black;
            this.lblAlert_{sid}_{j}.Location = new System.Drawing.Point(15, 15);
            this.lblAlert_{sid}_{j}.Name = "lblAlert_{sid}_{j}";
            this.lblAlert_{sid}_{j}.Size = new System.Drawing.Size(100, 17);
            this.lblAlert_{sid}_{j}.TabIndex = 5;
            this.lblAlert_{sid}_{j}.Text = "ℹ️ {item['text']}";
            this.pnlAlert_{sid}_{j}.Controls.Add(this.lblAlert_{sid}_{j});
            """)
            y_pos += 80
            continue # No divider for alert

        # Divider
        out.append(f"""
            this.div_{sid}_{j}.Location = new System.Drawing.Point(20, {div_y});
            this.div_{sid}_{j}.Name = "div_{sid}_{j}";
            this.div_{sid}_{j}.Size = new System.Drawing.Size(760, 1);
            this.div_{sid}_{j}.TabIndex = 10;
            this.div_{sid}_{j}.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.div_{sid}_{j}.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.pnlSecao{sid}.Controls.Add(this.div_{sid}_{j});
        """)

for sec in sections:
    out.append(f"            this.pnlContent.Controls.Add(this.pnlSecao{sec['id']});\n")

out.append("""
            // ConfiguracoesForm
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
""")
for sec in sections:
    sid = sec['id']
    out.append(f"            this.pnlSecao{sid}.ResumeLayout(false);\n")
    out.append(f"            this.pnlSecao{sid}.PerformLayout();\n")
    for j, item in enumerate(sec['items']):
        if item['type'] == 'alert':
            out.append(f"            this.pnlAlert_{sid}_{j}.ResumeLayout(false);\n")
            out.append(f"            this.pnlAlert_{sid}_{j}.PerformLayout();\n")

out.append("""            this.ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlSidebar;
        private System.Windows.Forms.Label lblTituloSidebar;
        private Guna.UI2.WinForms.Guna2Panel pnlContent;
""")

for sec in sections:
    sid = sec['id']
    out.append(f"        private Guna.UI2.WinForms.Guna2Button btnNav{sid};\n")
    out.append(f"        private Guna.UI2.WinForms.Guna2Panel pnlSecao{sid};\n")
    out.append(f"        private System.Windows.Forms.Label lblTitulo{sid};\n")
    out.append(f"        private System.Windows.Forms.Label lblDesc{sid};\n")
    for j, item in enumerate(sec['items']):
        if item['type'] in ['action', 'toggle', 'combo', 'input', 'button', 'info']:
            out.append(f"        private System.Windows.Forms.Label lblTitle_{sid}_{j};\n")
            if 'desc' in item and item['desc']:
                out.append(f"        private System.Windows.Forms.Label lblDesc_{sid}_{j};\n")
            
            if item['type'] == 'action':
                out.append(f"        private System.Windows.Forms.Label lblChev_{sid}_{j};\n")
            elif item['type'] == 'toggle':
                out.append(f"        private Guna.UI2.WinForms.Guna2ToggleSwitch tgl_{sid}_{j};\n")
            elif item['type'] == 'combo':
                out.append(f"        private Guna.UI2.WinForms.Guna2ComboBox cmb_{sid}_{j};\n")
            elif item['type'] == 'input':
                out.append(f"        private Guna.UI2.WinForms.Guna2TextBox txt_{sid}_{j};\n")
                out.append(f"        private System.Windows.Forms.Label lblSuf_{sid}_{j};\n")
            elif item['type'] == 'button':
                out.append(f"        private Guna.UI2.WinForms.Guna2Button btnAction_{sid}_{j};\n")
            elif item['type'] == 'info':
                out.append(f"        private System.Windows.Forms.Label lblVal_{sid}_{j};\n")
            
            out.append(f"        private Guna.UI2.WinForms.Guna2Panel div_{sid}_{j};\n")
        elif item['type'] == 'button_center':
            out.append(f"        private Guna.UI2.WinForms.Guna2Button btnCenter_{sid}_{j};\n")
        elif item['type'] == 'alert':
            out.append(f"        private Guna.UI2.WinForms.Guna2Panel pnlAlert_{sid}_{j};\n")
            out.append(f"        private System.Windows.Forms.Label lblAlert_{sid}_{j};\n")

out.append("""    }
}
""")

with open("ConfiguracoesForm.Designer.cs", "w", encoding="utf-8") as f:
    f.write("".join(out))
