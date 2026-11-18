using System.Text.Json;
using ProjetoBandejao.Models;

namespace ProjetoBandejao.Services
{
    internal static class PermissoesDesktop
    {
        private static readonly Lazy<HashSet<string>> administradores = new(CarregarAdministradores);

        public static bool PodeAdministrarContas => EhAdministrador(UsuarioSession.UsuarioLogado);

        public static bool EhAdministrador(Usuario? usuario) =>
            usuario is { Id: > 0, Funcionario: true, EmailConfirmado: true }
            && administradores.Value.Contains(usuario.Login.Trim());

        public static bool EhContaAtual(int id) =>
            id != 0 && UsuarioSession.UsuarioLogado?.Id == id;

        public static string Papel(Usuario usuario) =>
            EhAdministrador(usuario) ? "Administrador" : usuario.Funcionario ? "Funcionário" : "Outra conta";

        private static HashSet<string> CarregarAdministradores()
        {
            try
            {
                string arquivo = Path.Combine(AppContext.BaseDirectory, "administradores.json");
                var emails = JsonSerializer.Deserialize<string[]>(File.ReadAllText(arquivo)) ?? [];
                return new HashSet<string>(emails.Where(email => !string.IsNullOrWhiteSpace(email)).Select(email => email.Trim()),
                    StringComparer.OrdinalIgnoreCase);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
            {
                System.Diagnostics.Debug.WriteLine($"Permissões do desktop: {ex.Message}");
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }
        }
    }
}
