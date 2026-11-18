using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using ProjetoBandejao.Models;

namespace ProjetoBandejao.Services
{
    public partial class CardRefeicaoControl : UserControl
    {
        private static readonly Uri baseApi = new("http://localhost:8080/");
        private static readonly HttpClient imagens = new()
        {
            Timeout = TimeSpan.FromSeconds(4),
            MaxResponseContentBufferSize = 12 * 1024 * 1024
        };
        private static readonly SemaphoreSlim limiteImagens = new(3);
        private static readonly string pastaImagens = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BandejaoCotil", "imagens");
        private static readonly string[] extensoesLegadas = [".png", ".jpg", ".jpeg", ".webp"];
        private static readonly Lazy<string?> uploadsApiLocal = new(LocalizarUploadsDaApi);
        private readonly CancellationTokenSource fechar = new();
        private Prato? pratoAtual;

        public event EventHandler<Prato>? OnExcluirPrato;
        public event EventHandler<Prato>? OnEditarPrato;
        public event EventHandler<Prato>? OnVerAvaliacoes;

        public CardRefeicaoControl()
        {
            InitializeComponent();
            btnDetalhes.Click += (_, _) =>
            {
                if (pratoAtual is { } prato) OnEditarPrato?.Invoke(this, prato);
            };
            btnExcluir.Click += async (_, _) => await ExcluirAsync();
            btnAvaliacoes.Click += (_, _) =>
            {
                if (pratoAtual is { Id: > 0 } prato) OnVerAvaliacoes?.Invoke(this, prato);
            };
        }

        public void PreencherDados(Prato prato)
        {
            pratoAtual = prato;
            btnAvaliacoes.Enabled = prato.Id > 0;
            lblTitle.Text = prato.Nome;
            lblDescription.Text = string.IsNullOrWhiteSpace(prato.Descricao) ? "Sem descrição cadastrada." : prato.Descricao;
            lblCategoria.Text = prato.Vegano ? $"{prato.CategoriaTexto}  ·  Vegano" : prato.CategoriaTexto;

            var valores = prato.ValorNutricional;
            valoresNutricionais[0].Text = FormatarValor(valores?.Kcal ?? prato.Calorias, "0", "kcal");
            valoresNutricionais[1].Text = FormatarValor(valores?.Carboidratos ?? prato.Carboidratos, "0.#", "g");
            valoresNutricionais[2].Text = FormatarValor(valores?.Proteinas ?? prato.Proteinas, "0.#", "g");
            valoresNutricionais[3].Text = FormatarValor(valores?.Lipidios ?? prato.Gorduras, "0.#", "g");
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (pratoAtual == null || DesignMode) return;
            var token = fechar.Token;
            try
            {
                Bitmap? imagem = await ObterImagemAsync(pratoAtual, token);
                if (IsDisposed || token.IsCancellationRequested)
                {
                    imagem?.Dispose();
                    return;
                }

                pctImage.Image = imagem;
                pctImage.Visible = imagem != null;
                lblSemFoto.Visible = imagem == null;
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested) { }
            catch (Exception ex) when (ex is IOException or HttpRequestException or OperationCanceledException or UnauthorizedAccessException)
            {
                Debug.WriteLine($"Foto do prato {pratoAtual.Id}: {ex.Message}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !IsDisposed)
            {
                fechar.Cancel();
                fechar.Dispose();
                pctImage.Image?.Dispose();
            }
            base.Dispose(disposing);
        }

        private static async Task<Bitmap?> ObterImagemAsync(Prato prato, CancellationToken token)
        {
            await limiteImagens.WaitAsync(token).ConfigureAwait(false);
            try
            {
                Uri? url = Uri.TryCreate(baseApi, prato.Imagem, out var endereco)
                    && !string.IsNullOrWhiteSpace(prato.Imagem)
                    && (endereco.Scheme == Uri.UriSchemeHttp || endereco.Scheme == Uri.UriSchemeHttps) ? endereco : null;
                string? arquivo = url == null ? null : Path.Combine(pastaImagens,
                    $"foto_{Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(url.AbsoluteUri)))}.png");

                Bitmap? local = await Task.Run(() =>
                {
                    var imagem = LerImagem(prato.LocalImagePath) ?? LerImagem(arquivo) ?? LerImagem(ObterUploadLocal(url, "pratos"));
                    if (imagem != null) return imagem;

                    foreach (string extensao in extensoesLegadas)
                    {
                        imagem = LerImagem(Path.Combine(pastaImagens, $"prato_{prato.Id}{extensao}"));
                        if (imagem != null) return imagem;
                    }

                    if (string.IsNullOrWhiteSpace(prato.ImagemBase64)) return null;
                    try { return Decodificar(Convert.FromBase64String(prato.ImagemBase64)); }
                    catch (FormatException) { return null; }
                }, token).ConfigureAwait(false);
                if (local != null || url == null || arquivo == null) return local;

                using var resposta = await imagens.GetAsync(url, token).ConfigureAwait(false);
                if (resposta.StatusCode == HttpStatusCode.NotFound || !resposta.IsSuccessStatusCode) return null;
                byte[] bytes = await resposta.Content.ReadAsByteArrayAsync(token).ConfigureAwait(false);
                return await Task.Run(() =>
                {
                    var imagem = Decodificar(bytes);
                    if (imagem == null) return null;
                    try
                    {
                        Directory.CreateDirectory(pastaImagens);
                        string temporario = arquivo + "." + Guid.NewGuid().ToString("N") + ".tmp";
                        try
                        {
                            imagem.Save(temporario, System.Drawing.Imaging.ImageFormat.Png);
                            File.Move(temporario, arquivo, overwrite: true);
                        }
                        finally
                        {
                            File.Delete(temporario);
                        }
                    }
                    catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or System.Runtime.InteropServices.ExternalException)
                    {
                        Debug.WriteLine($"Cache de imagem: {ex.Message}");
                    }
                    return imagem;
                }, token).ConfigureAwait(false);
            }
            finally
            {
                limiteImagens.Release();
            }
        }

        private static Bitmap? LerImagem(string? caminho)
        {
            if (string.IsNullOrWhiteSpace(caminho) || !File.Exists(caminho)) return null;
            try { return Decodificar(File.ReadAllBytes(caminho)); }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException) { return null; }
        }

        internal static string? ObterUploadLocal(Uri? url, string pastaUpload)
        {
            string prefixo = $"/uploads/{pastaUpload}/";
            if (url == null || !url.IsLoopback || url.Port != baseApi.Port
                || !url.AbsolutePath.StartsWith(prefixo, StringComparison.Ordinal))
                return null;

            string nome = Uri.UnescapeDataString(url.AbsolutePath[prefixo.Length..]);
            if (nome.Length == 0 || nome is "." or ".." || nome.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                return null;

            string? pasta = uploadsApiLocal.Value;
            return pasta == null ? null : Path.Combine(pasta, pastaUpload, nome);
        }

        private static string? LocalizarUploadsDaApi()
        {
            foreach (string inicio in new[] { AppContext.BaseDirectory, Environment.CurrentDirectory })
            {
                for (var pasta = new DirectoryInfo(inicio); pasta != null; pasta = pasta.Parent)
                {
                    string uploads = Path.Combine(pasta.FullName, "api", "apiBandeco", "uploads");
                    if (Directory.Exists(uploads)) return uploads;
                }
            }
            return null;
        }

        private static Bitmap? Decodificar(byte[] dados)
        {
            try
            {
                using var stream = new MemoryStream(dados);
                using var original = Image.FromStream(stream);
                double escala = Math.Min(1, 640.0 / Math.Max(original.Width, original.Height));
                return new Bitmap(original, Math.Max(1, (int)(original.Width * escala)), Math.Max(1, (int)(original.Height * escala)));
            }
            catch (Exception ex) when (ex is ArgumentException or OutOfMemoryException or System.Runtime.InteropServices.ExternalException)
            {
                return null;
            }
        }

        private static string FormatarValor(double valor, string formato, string unidade) =>
            valor > 0 ? $"{valor.ToString(formato)} {unidade}" : "—";

        private async Task ExcluirAsync()
        {
            if (pratoAtual is not { } prato || MessageBox.Show(this, $"Excluir o prato '{prato.Nome}'?", "Excluir prato",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            btnExcluir.Enabled = false;
            var servico = new PratoService();
            bool excluido = await Task.Run(() => servico.Deletar(prato.Id));
            if (IsDisposed) return;
            btnExcluir.Enabled = true;
            if (excluido)
                OnExcluirPrato?.Invoke(this, prato);
            else
                MessageBox.Show(this, "Não foi possível excluir o prato.", "Excluir prato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
