using ProjetoBandejao.Services;

namespace ProjetoBandejao.Forms
{
    public partial class ConfirmarCodigoForm : Form
    {
        private readonly UsuarioService service = new();

        public ConfirmarCodigoForm(string email)
        {
            InitializeComponent();
            txtEmail.Text = email.Trim();
            txtEmail.ReadOnly = !string.IsNullOrWhiteSpace(email);
            btnConfirmar.Click += async (_, _) => await ConfirmarAsync();
            linkReenviar.LinkClicked += async (_, _) => await ReenviarAsync();
            Shown += (_, _) => mskCodigo.Focus();
        }

        private async Task ConfirmarAsync()
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !mskCodigo.MaskCompleted)
            {
                status.Text = "Informe o e-mail e os seis dígitos do código.";
                return;
            }
            string email = txtEmail.Text.Trim();
            string codigo = mskCodigo.Text;
            await ExecutarAsync(() =>
            {
                bool sucesso = service.ConfirmarEmail(email, codigo, out string mensagem);
                return (sucesso, mensagem);
            }, fecharAoConfirmar: true);
        }

        private async Task ReenviarAsync()
        {
            string email = txtEmail.Text.Trim();
            if (email.Length == 0)
            {
                status.Text = "Informe seu e-mail para pedir outro código.";
                return;
            }
            await ExecutarAsync(() =>
            {
                bool sucesso = service.ReenviarCodigoCadastro(email, out string mensagem);
                return (sucesso, mensagem);
            }, fecharAoConfirmar: false);
        }

        private async Task ExecutarAsync(Func<(bool Sucesso, string Mensagem)> enviar, bool fecharAoConfirmar)
        {
            btnConfirmar.Enabled = linkReenviar.Enabled = false;
            status.Text = "Aguardando a API…";
            try
            {
                var resultado = await Task.Run(enviar);
                if (IsDisposed) return;
                if (resultado.Sucesso && fecharAoConfirmar)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }
                status.Text = string.IsNullOrWhiteSpace(resultado.Mensagem)
                    ? "A API não confirmou a operação." : resultado.Mensagem;
            }
            finally
            {
                if (!IsDisposed) btnConfirmar.Enabled = linkReenviar.Enabled = true;
            }
        }
    }
}
