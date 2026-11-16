using ProjetoBandejao.Data;
using ProjetoBandejao.Forms;
using ProjetoBandejao.Forms.Home;
using ProjetoBandejao.Services;

namespace ProjetoBandejao
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Cria/inicializa o banco local
            SQLiteDatabase.Inicializar();

            // Sincroniza em segundo plano com a API para abrir o login instantaneamente
            _ = Task.Run(async () =>
            {
                try
                {
                    var api = new ApiService();
                    var usuarios = await api.ObterUsuarios();
                    if (usuarios != null && usuarios.Count > 0)
                    {
                        SQLiteDatabase.SalvarFuncionarios(usuarios);
                        Console.WriteLine($"Sincronização concluída: {usuarios.Count} usuários.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao sincronizar com a API: {ex.Message}");
                }
            });

            Application.Run(new LoginForm());
        }
    }
}