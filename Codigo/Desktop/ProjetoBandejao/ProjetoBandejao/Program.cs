using ProjetoBandejao.Data;
using ProjetoBandejao.Forms;
using ProjetoBandejao.Services;

namespace ProjetoBandejao
{
    internal static class Program
    {
        [STAThread]
        static async Task Main()
        {
            ApplicationConfiguration.Initialize();

            // Cria/inicializa o banco local
            SQLiteDatabase.Inicializar();

            try
            {
                // Busca os usuários da API
                var api = new ApiService();
                var usuarios = await api.ObterUsuarios();

                // Salva/atualiza no SQLite local
                SQLiteDatabase.SalvarFuncionarios(usuarios);

                Console.WriteLine(
                    $"Sincronização concluída: {usuarios.Count} usuários."
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Erro ao sincronizar com a API: {ex.Message}"
                );
            }

            Application.Run(new LoginForm());
        }
    }
}