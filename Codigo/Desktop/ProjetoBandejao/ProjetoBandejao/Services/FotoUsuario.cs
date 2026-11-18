using System.Security.Cryptography;
using System.Text;
using ProjetoBandejao.Models;

namespace ProjetoBandejao.Services
{
    internal static class FotoUsuario
    {
        private static readonly Uri api = new("http://localhost:8080/");
        private static readonly HttpClient cliente = new() { Timeout = TimeSpan.FromSeconds(4), MaxResponseContentBufferSize = 8 * 1024 * 1024 };
        private static readonly SemaphoreSlim limite = new(3);

        public static async Task<Bitmap?> CarregarAsync(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Imagem) || !Uri.TryCreate(api, usuario.Imagem, out var url)
                || (url.Scheme != Uri.UriSchemeHttp && url.Scheme != Uri.UriSchemeHttps))
                return null;

            string pasta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "BandejaoCotil", "avatares");
            string arquivo = Path.Combine(pasta, Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(url.AbsoluteUri))) + ".png");
            await limite.WaitAsync().ConfigureAwait(false);
            try
            {
                var existente = await Task.Run(() => Ler(arquivo) ?? Ler(CardRefeicaoControl.ObterUploadLocal(url, "user"))).ConfigureAwait(false);
                if (existente != null) return existente;
                byte[] dados = await cliente.GetByteArrayAsync(url).ConfigureAwait(false);
                return await Task.Run(() =>
                {
                    var imagem = Decodificar(dados);
                    if (imagem == null) return null;
                    try
                    {
                        Directory.CreateDirectory(pasta);
                        imagem.Save(arquivo, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or System.Runtime.InteropServices.ExternalException) { }
                    return imagem;
                }).ConfigureAwait(false);
            }
            catch (Exception ex) when (ex is HttpRequestException or OperationCanceledException or IOException or UnauthorizedAccessException)
            {
                return null;
            }
            finally { limite.Release(); }
        }

        private static Bitmap? Ler(string? arquivo) =>
            arquivo != null && File.Exists(arquivo) ? Decodificar(File.ReadAllBytes(arquivo)) : null;

        private static Bitmap? Decodificar(byte[] dados)
        {
            try
            {
                using var stream = new MemoryStream(dados);
                using var origem = Image.FromStream(stream);
                int lado = Math.Min(origem.Width, origem.Height);
                var avatar = new Bitmap(96, 96);
                using var desenho = Graphics.FromImage(avatar);
                desenho.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                desenho.DrawImage(origem, new Rectangle(0, 0, 96, 96),
                    new Rectangle((origem.Width - lado) / 2, (origem.Height - lado) / 2, lado, lado), GraphicsUnit.Pixel);
                return avatar;
            }
            catch (Exception ex) when (ex is ArgumentException or OutOfMemoryException or System.Runtime.InteropServices.ExternalException) { return null; }
        }
    }
}
