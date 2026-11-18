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

            // Cria/inicializa o banco local com suporte a modo offline completo
            SQLiteDatabase.Inicializar();

            // Sincroniza em segundo plano com a API para abrir o login instantaneamente e manter cache atualizado
            _ = Task.Run(async () =>
            {
                try
                {
                    var api = new ApiService();
                    var usuarios = await api.ObterUsuarios();
                    if (usuarios != null && usuarios.Count > 0)
                    {
                        SQLiteDatabase.SalvarFuncionarios(usuarios);
                        Console.WriteLine($"Sincronização de usuários concluída: {usuarios.Count} usuários.");
                    }

                    var pratos = await api.ObterPratos();
                    if (pratos != null)
                    {
                        SQLiteDatabase.SalvarPratosApi(pratos);
                        Console.WriteLine($"Sincronização de pratos concluída: {pratos.Count} pratos.");
                    }

                    var avisos = await api.ObterAvisos();
                    if (avisos != null)
                    {
                        SQLiteDatabase.SalvarAvisosApi(avisos);
                        Console.WriteLine($"Sincronização de avisos concluída: {avisos.Count} avisos.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Program] Executando em contingência offline: {ex.Message}");
                }
            });

            Application.Run(new LoginForm());
        }
    }
}
