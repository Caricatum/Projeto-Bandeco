using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using ProjetoBandejao.Models;
using ProjetoBandejao.Services;

namespace ProjetoBandejao.Data
{
    public static class SQLiteDatabase
    {
        private static readonly string pasta =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData
                ),
                "BandejaoCotil"
            );

        private static readonly string caminhoBanco =
            Path.Combine(pasta, "bandeco.db");

        private static readonly string connectionString =
            $"Data Source={caminhoBanco}";

        public static SqliteConnection GetConnection()
        {
            Directory.CreateDirectory(pasta);
            return new SqliteConnection(connectionString);
        }

        public static string GerarHashSenha(string senha)
        {
            if (string.IsNullOrEmpty(senha)) return string.Empty;
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(senha, salt, 600_000, HashAlgorithmName.SHA256, 32);
            return $"pbkdf2:{Convert.ToHexString(salt)}:{Convert.ToHexString(hash)}";
        }

        private static string GerarHashLegado(string senha) =>
            Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(senha)));

        private static bool ConferirSenha(string senha, string hash)
        {
            if (hash.StartsWith("pbkdf2:", StringComparison.Ordinal))
            {
                string[] partes = hash.Split(':');
                if (partes.Length != 3 || partes[1].Length != 32 || partes[2].Length != 64)
                    return false;

                byte[] esperado = Convert.FromHexString(partes[2]);
                byte[] calculado = Rfc2898DeriveBytes.Pbkdf2(senha, Convert.FromHexString(partes[1]), 600_000, HashAlgorithmName.SHA256, 32);
                return CryptographicOperations.FixedTimeEquals(esperado, calculado);
            }

            return string.Equals(hash, GerarHashLegado(senha), StringComparison.OrdinalIgnoreCase);
        }

        private static string ProtegerSenhaPendente(string senha) =>
            "dpapi:" + Convert.ToBase64String(ProtectedData.Protect(Encoding.UTF8.GetBytes(senha), null, DataProtectionScope.CurrentUser));

        private static string DesprotegerSenhaPendente(string senha) =>
            senha.StartsWith("dpapi:", StringComparison.Ordinal)
                ? Encoding.UTF8.GetString(ProtectedData.Unprotect(Convert.FromBase64String(senha[6..]), null, DataProtectionScope.CurrentUser))
                : senha;

        public static void Inicializar()
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                // 1. Tabela Funcionarios
                string sqlFuncionarios = @"
                    CREATE TABLE IF NOT EXISTS Funcionarios (
                        Id INTEGER PRIMARY KEY,
                        Login TEXT NOT NULL UNIQUE,
                        Nome TEXT NOT NULL,
                        Funcionario INTEGER NOT NULL,
                        EmailConfirmado INTEGER NOT NULL DEFAULT 1,
                        SenhaHash TEXT,
                        SenhaPendente TEXT,
                        PendenteEnvio INTEGER NOT NULL DEFAULT 0,
                        Setor TEXT DEFAULT 'Administração',
                        Cargo TEXT DEFAULT 'Funcionário',
                        Telefone TEXT DEFAULT '-'
                    );
                ";
                using (var cmd = new SqliteCommand(sqlFuncionarios, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // Migração de colunas
                ExecutarComandoSilencioso(connection, "ALTER TABLE Funcionarios ADD COLUMN EmailConfirmado INTEGER NOT NULL DEFAULT 1;");
                ExecutarComandoSilencioso(connection, "ALTER TABLE Funcionarios ADD COLUMN SenhaHash TEXT;");
                ExecutarComandoSilencioso(connection, "ALTER TABLE Funcionarios ADD COLUMN SenhaPendente TEXT;");
                ExecutarComandoSilencioso(connection, "ALTER TABLE Funcionarios ADD COLUMN PendenteEnvio INTEGER NOT NULL DEFAULT 0;");
                ExecutarComandoSilencioso(connection, "ALTER TABLE Funcionarios ADD COLUMN Imagem TEXT;");
                ExecutarComandoSilencioso(connection, "ALTER TABLE Funcionarios ADD COLUMN Restricoes INTEGER NOT NULL DEFAULT 0;");
                ExecutarComandoSilencioso(connection, "ALTER TABLE Funcionarios ADD COLUMN Setor TEXT DEFAULT 'Administração';");
                ExecutarComandoSilencioso(connection, "ALTER TABLE Funcionarios ADD COLUMN Cargo TEXT DEFAULT 'Funcionário';");
                ExecutarComandoSilencioso(connection, "ALTER TABLE Funcionarios ADD COLUMN Telefone TEXT DEFAULT '-';");

                // 2. Tabela Pratos
                string sqlPratos = @"
                    CREATE TABLE IF NOT EXISTS Pratos (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ApiId INTEGER,
                        Nome TEXT NOT NULL,
                        Descricao TEXT,
                        Vegano INTEGER NOT NULL DEFAULT 0,
                        ImagemUrl TEXT,
                        NotaTecnica TEXT,
                        DescricaoIA TEXT,
                        CategoriaId INTEGER,
                        CategoriaDescricao TEXT,
                        Calorias REAL DEFAULT 0,
                        Proteinas REAL DEFAULT 0,
                        Carboidratos REAL DEFAULT 0,
                        Gorduras REAL DEFAULT 0,
                        LocalImagePath TEXT,
                        PendenteEnvio INTEGER DEFAULT 0
                    );
                ";
                using (var cmd = new SqliteCommand(sqlPratos, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // 3. Tabela Avisos
                ExecutarComandoSilencioso(connection, "ALTER TABLE Pratos ADD COLUMN Alergenicos INTEGER NOT NULL DEFAULT 0;");
                string sqlAvisos = @"
                    CREATE TABLE IF NOT EXISTS Avisos (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ApiId INTEGER,
                        Titulo TEXT NOT NULL,
                        Descricao TEXT NOT NULL,
                        Model TEXT DEFAULT 'Mural',
                        DataInicial TEXT,
                        DataFinal TEXT,
                        DataCriacao TEXT,
                        UserId INTEGER,
                        PendenteEnvio INTEGER DEFAULT 0
                    );
                ";
                using (var cmd = new SqliteCommand(sqlAvisos, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // 4. Tabela CardapioSemana
                string sqlCardapio = @"
                    CREATE TABLE IF NOT EXISTS CardapioSemana (
                        SegundaFeira TEXT PRIMARY KEY,
                        JsonData TEXT NOT NULL
                    );
                ";
                using (var cmd = new SqliteCommand(sqlCardapio, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                using (var removerAdminPadrao = new SqliteCommand(
                    "UPDATE Funcionarios SET SenhaHash = NULL WHERE Login = 'admin@cotil.unicamp.br' AND UPPER(SenhaHash) = $hash;", connection))
                {
                    removerAdminPadrao.Parameters.AddWithValue("$hash", GerarHashLegado("admin123"));
                    removerAdminPadrao.ExecuteNonQuery();
                }

                MigrarSenhasPendentes(connection);

                // Limpeza automática de dados mockados/fictícios residuais (garante que apenas dados reais da API e cadastros offline existam)
                try
                {
                    using var cleanMockPratos = new SqliteCommand("DELETE FROM Pratos WHERE (ApiId IS NULL OR ApiId <= 0) AND PendenteEnvio = 0;", connection);
                    cleanMockPratos.ExecuteNonQuery();

                    using var cleanMockAvisos = new SqliteCommand("DELETE FROM Avisos WHERE (ApiId IS NULL OR ApiId <= 0) AND PendenteEnvio = 0;", connection);
                    cleanMockAvisos.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[SQLiteDatabase] Erro ao limpar dados mockados: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.Inicializar] Erro: {ex.Message}");
            }
        }

        private static void ExecutarComandoSilencioso(SqliteConnection connection, string sql)
        {
            try
            {
                using var cmd = new SqliteCommand(sql, connection);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                // Coluna já existe ou erro ignorável
            }
        }

        private static void MigrarSenhasPendentes(SqliteConnection connection)
        {
            using var transaction = connection.BeginTransaction();
            using (var consultarIds = new SqliteCommand("SELECT MIN(0, COALESCE(MIN(Id), 0)) FROM Funcionarios;", connection, transaction))
            {
                using var separarIds = new SqliteCommand(
                    "UPDATE Funcionarios SET Id = $menor - Id WHERE PendenteEnvio = 1 AND Id > 0;", connection, transaction);
                separarIds.Parameters.AddWithValue("$menor", consultarIds.ExecuteScalar() ?? 0L);
                separarIds.ExecuteNonQuery();
            }
            var pendentes = new List<(int Id, string Senha)>();
            using (var consulta = new SqliteCommand(
                "SELECT Id, SenhaPendente FROM Funcionarios WHERE SenhaPendente IS NOT NULL AND SenhaPendente NOT LIKE 'dpapi:%';",
                connection, transaction))
            using (var reader = consulta.ExecuteReader())
            {
                while (reader.Read())
                    pendentes.Add((reader.GetInt32(0), reader.GetString(1)));
            }

            foreach (var (id, senha) in pendentes)
            {
                using var atualizar = new SqliteCommand("UPDATE Funcionarios SET SenhaPendente = $senha WHERE Id = $id;", connection, transaction);
                atualizar.Parameters.AddWithValue("$senha", ProtegerSenhaPendente(senha));
                atualizar.Parameters.AddWithValue("$id", id);
                atualizar.ExecuteNonQuery();
            }

            transaction.Commit();
        }

        // ==========================================
        // FUNCIONARIOS / AUTENTICAÇÃO
        // ==========================================
        public static void SalvarFuncionarios(List<Usuario> usuarios)
        {
            if (usuarios == null || usuarios.Count == 0) return;

            try
            {
                using var connection = GetConnection();
                connection.Open();

                foreach (var u in usuarios)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(u.Login)) continue;

                        string loginLimpo = u.Login.Trim();

                        // Verifica se o usuário já existe por Login (case-insensitive)
                        using var checkCmd = new SqliteCommand("SELECT Id FROM Funcionarios WHERE LOWER(TRIM(Login)) = LOWER($login);", connection);
                        checkCmd.Parameters.AddWithValue("$login", loginLimpo);

                        var existingId = checkCmd.ExecuteScalar();

                        if (existingId != null)
                        {
                            string updSql = @"
                                UPDATE Funcionarios SET
                                    Id = CASE WHEN Id < 0 THEN $apiId ELSE Id END,
                                    Nome = $nome,
                                    Funcionario = $funcionario,
                                    EmailConfirmado = $emailConfirmado,
                                    Imagem = $imagem,
                                    Restricoes = $restricoes,
                                    PendenteEnvio = CASE WHEN Id < 0 THEN 0 ELSE PendenteEnvio END,
                                    SenhaPendente = CASE WHEN Id < 0 THEN NULL ELSE SenhaPendente END,
                                    SenhaHash = CASE WHEN Id < 0 THEN NULL ELSE SenhaHash END,
                                    Setor = COALESCE($setor, Setor),
                                    Cargo = COALESCE($cargo, Cargo),
                                    Telefone = COALESCE($telefone, Telefone)
                                WHERE Id = $existingId;
                            ";
                            using var updCmd = new SqliteCommand(updSql, connection);
                            updCmd.Parameters.AddWithValue("$existingId", existingId);
                            updCmd.Parameters.AddWithValue("$apiId", u.Id);
                            updCmd.Parameters.AddWithValue("$nome", string.IsNullOrWhiteSpace(u.Nome) ? loginLimpo : u.Nome.Trim());
                            updCmd.Parameters.AddWithValue("$funcionario", u.Funcionario ? 1 : 0);
                            updCmd.Parameters.AddWithValue("$emailConfirmado", u.EmailConfirmado ? 1 : 0);
                            updCmd.Parameters.AddWithValue("$imagem", (object?)u.Imagem ?? DBNull.Value);
                            updCmd.Parameters.AddWithValue("$restricoes", u.MascaraRestricoes);
                            updCmd.Parameters.AddWithValue("$setor", string.IsNullOrWhiteSpace(u.Setor) ? (u.Funcionario ? "Administração" : "Acadêmico") : u.Setor);
                            updCmd.Parameters.AddWithValue("$cargo", string.IsNullOrWhiteSpace(u.CargoEspecifico) ? u.Cargo : u.CargoEspecifico);
                            updCmd.Parameters.AddWithValue("$telefone", string.IsNullOrWhiteSpace(u.Telefone) ? "-" : u.Telefone);
                            updCmd.ExecuteNonQuery();
                        }
                        else
                        {
                            string insSql = @"
                                INSERT OR REPLACE INTO Funcionarios (Id, Login, Nome, Funcionario, EmailConfirmado, Setor, Cargo, Telefone, PendenteEnvio, Imagem, Restricoes)
                                VALUES ($id, $login, $nome, $funcionario, $emailConfirmado, $setor, $cargo, $telefone, 0, $imagem, $restricoes);
                            ";
                            using var insCmd = new SqliteCommand(insSql, connection);
                            insCmd.Parameters.AddWithValue("$id", u.Id);
                            insCmd.Parameters.AddWithValue("$login", loginLimpo);
                            insCmd.Parameters.AddWithValue("$nome", string.IsNullOrWhiteSpace(u.Nome) ? loginLimpo : u.Nome.Trim());
                            insCmd.Parameters.AddWithValue("$funcionario", u.Funcionario ? 1 : 0);
                            insCmd.Parameters.AddWithValue("$emailConfirmado", u.EmailConfirmado ? 1 : 0);
                            insCmd.Parameters.AddWithValue("$imagem", (object?)u.Imagem ?? DBNull.Value);
                            insCmd.Parameters.AddWithValue("$restricoes", u.MascaraRestricoes);
                            insCmd.Parameters.AddWithValue("$setor", string.IsNullOrWhiteSpace(u.Setor) ? (u.Funcionario ? "Administração" : "Acadêmico") : u.Setor);
                            insCmd.Parameters.AddWithValue("$cargo", string.IsNullOrWhiteSpace(u.CargoEspecifico) ? u.Cargo : u.CargoEspecifico);
                            insCmd.Parameters.AddWithValue("$telefone", string.IsNullOrWhiteSpace(u.Telefone) ? "-" : u.Telefone);
                            insCmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception exUser)
                    {
                        Console.WriteLine($"[SQLiteDatabase.SalvarFuncionarios] Erro ao salvar funcionário '{u.Login}': {exUser.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.SalvarFuncionarios] Erro geral: {ex.Message}");
            }
        }

        public static void AtualizarSenhaHash(string login, string senhaClara)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrEmpty(senhaClara)) return;

            try
            {
                using var connection = GetConnection();
                connection.Open();

                string hash = GerarHashSenha(senhaClara);
                string sql = "UPDATE Funcionarios SET SenhaHash = $hash WHERE LOWER(Login) = LOWER($login);";
                using var cmd = new SqliteCommand(sql, connection);
                cmd.Parameters.AddWithValue("$hash", hash);
                cmd.Parameters.AddWithValue("$login", login.Trim());
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.AtualizarSenhaHash] Erro: {ex.Message}");
            }
        }

        public static bool ValidarSenhaLocal(string login, string senhaDigitada, out Usuario? usuarioLogado)
        {
            usuarioLogado = null;
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrEmpty(senhaDigitada)) return false;

            try
            {
                using var connection = GetConnection();
                connection.Open();

                string loginLimpo = login.Trim();

                // Um login sem domínio pode identificar o usuário pelo prefixo do e-mail.
                string sql = @"
                    SELECT Id, Login, Nome, Funcionario, EmailConfirmado, SenhaHash, Imagem, Restricoes
                    FROM Funcionarios
                    WHERE Funcionario = 1 AND EmailConfirmado = 1
                      AND (LOWER(Login) = LOWER($login)
                           OR (INSTR($login, '@') = 0 AND LOWER(SUBSTR(Login, 1, INSTR(Login || '@', '@') - 1)) = LOWER($login)));
                ";

                using var cmd = new SqliteCommand(sql, connection);
                cmd.Parameters.AddWithValue("$login", loginLimpo);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string hashGravado = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                    if (!string.IsNullOrEmpty(hashGravado) && ConferirSenha(senhaDigitada, hashGravado))
                    {
                        usuarioLogado = new Usuario
                        {
                            Id = reader.GetInt32(0),
                            Login = reader.GetString(1),
                            Nome = reader.GetString(2),
                            Funcionario = reader.GetInt32(3) == 1,
                            EmailConfirmado = reader.GetInt32(4) == 1,
                            Imagem = reader.IsDBNull(6) ? null : reader.GetString(6),
                            MascaraRestricoes = reader.GetInt32(7)
                        };
                        if (!hashGravado.StartsWith("pbkdf2:", StringComparison.Ordinal))
                        {
                            reader.Close();
                            AtualizarSenhaHash(usuarioLogado.Login, senhaDigitada);
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.ValidarSenhaLocal] Erro: {ex.Message}");
            }

            return false;
        }

        public static bool SalvarFuncionarioLocal(Usuario usuario, string senhaClara, bool pendenteEnvio = true)
        {
            if (usuario == null || string.IsNullOrWhiteSpace(usuario.Login)) return false;

            try
            {
                using var connection = GetConnection();
                connection.Open();

                using var transaction = connection.BeginTransaction();
                int localId = usuario.Id;
                if (pendenteEnvio)
                {
                    using var duplicado = new SqliteCommand(
                        "SELECT 1 FROM Funcionarios WHERE LOWER(TRIM(Login)) = LOWER(TRIM($login)) LIMIT 1;",
                        connection, transaction);
                    duplicado.Parameters.AddWithValue("$login", usuario.Login);
                    if (duplicado.ExecuteScalar() != null) return false;

                    using var minCmd = new SqliteCommand("SELECT MIN(0, COALESCE(MIN(Id), 0)) - 1 FROM Funcionarios;", connection, transaction);
                    localId = Convert.ToInt32(minCmd.ExecuteScalar());
                }
                else if (localId <= 0)
                    return false;

                string hash = GerarHashSenha(senhaClara);
                string sql = @"
                    INSERT INTO Funcionarios (Id, Login, Nome, Funcionario, EmailConfirmado, SenhaHash, SenhaPendente, PendenteEnvio, Setor, Cargo, Telefone)
                    VALUES ($id, $login, $nome, $funcionario, $emailConfirmado, $hash, $senhaPendente, $pendente, $setor, $cargo, $telefone)
                    ON CONFLICT(Id) DO UPDATE SET
                        Login = excluded.Login,
                        Nome = excluded.Nome,
                        Funcionario = excluded.Funcionario,
                        EmailConfirmado = excluded.EmailConfirmado,
                        SenhaHash = excluded.SenhaHash,
                        SenhaPendente = excluded.SenhaPendente,
                        PendenteEnvio = excluded.PendenteEnvio,
                        Setor = excluded.Setor,
                        Cargo = excluded.Cargo,
                        Telefone = excluded.Telefone;
                ";

                using var cmd = new SqliteCommand(sql, connection, transaction);
                cmd.Parameters.AddWithValue("$id", localId);
                cmd.Parameters.AddWithValue("$login", usuario.Login.Trim());
                cmd.Parameters.AddWithValue("$nome", usuario.Nome.Trim());
                cmd.Parameters.AddWithValue("$funcionario", usuario.Funcionario ? 1 : 0);
                cmd.Parameters.AddWithValue("$emailConfirmado", 1);
                cmd.Parameters.AddWithValue("$hash", hash);
                cmd.Parameters.AddWithValue("$senhaPendente", pendenteEnvio ? ProtegerSenhaPendente(senhaClara) : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("$pendente", pendenteEnvio ? 1 : 0);
                cmd.Parameters.AddWithValue("$setor", string.IsNullOrWhiteSpace(usuario.Setor) ? (usuario.Funcionario ? "Administração" : "Acadêmico") : usuario.Setor);
                cmd.Parameters.AddWithValue("$cargo", string.IsNullOrWhiteSpace(usuario.CargoEspecifico) ? usuario.Cargo : usuario.CargoEspecifico);
                cmd.Parameters.AddWithValue("$telefone", string.IsNullOrWhiteSpace(usuario.Telefone) ? "-" : usuario.Telefone);

                cmd.ExecuteNonQuery();
                transaction.Commit();
                usuario.Id = localId;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.SalvarFuncionarioLocal] Erro: {ex.Message}");
                return false;
            }
        }

        public static bool ExisteEmailLocal(string email) =>
            ObterFuncionarios().Any(usuario => string.Equals(usuario.Login.Trim(), email.Trim(), StringComparison.OrdinalIgnoreCase));

        public static bool AtualizarFuncionarioPendente(Usuario usuario)
        {
            using var connection = GetConnection();
            connection.Open();
            using var comando = new SqliteCommand(
                "UPDATE Funcionarios SET Nome = $nome, Funcionario = $funcionario, Restricoes = $restricoes WHERE Id = $id AND PendenteEnvio = 1;",
                connection);
            comando.Parameters.AddWithValue("$nome", usuario.Nome);
            comando.Parameters.AddWithValue("$funcionario", usuario.Funcionario ? 1 : 0);
            comando.Parameters.AddWithValue("$restricoes", usuario.MascaraRestricoes);
            comando.Parameters.AddWithValue("$id", usuario.Id);
            return comando.ExecuteNonQuery() == 1;
        }

        public class FuncionarioPendenteInfo
        {
            public int LocalId { get; set; }
            public string Login { get; set; } = string.Empty;
            public string Nome { get; set; } = string.Empty;
            public string Senha { get; set; } = string.Empty;
            public bool Funcionario { get; set; }
        }

        public static List<FuncionarioPendenteInfo> ObterFuncionariosPendentes()
        {
            var pendentes = new List<FuncionarioPendenteInfo>();

            try
            {
                using var connection = GetConnection();
                connection.Open();

                string sql = "SELECT Id, Login, Nome, SenhaPendente, Funcionario FROM Funcionarios WHERE PendenteEnvio = 1 AND SenhaPendente IS NOT NULL;";
                using var cmd = new SqliteCommand(sql, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    pendentes.Add(new FuncionarioPendenteInfo
                    {
                        LocalId = reader.GetInt32(0),
                        Login = reader.GetString(1),
                        Nome = reader.GetString(2),
                        Senha = DesprotegerSenhaPendente(reader.GetString(3)),
                        Funcionario = reader.GetInt32(4) == 1
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.ObterFuncionariosPendentes] Erro: {ex.Message}");
            }

            return pendentes;
        }

        public static void MarcarFuncionarioSincronizado(string login, int apiId)
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                string sql = "UPDATE Funcionarios SET Id = $apiId, PendenteEnvio = 0, SenhaPendente = NULL WHERE LOWER(Login) = LOWER($login);";
                using var cmd = new SqliteCommand(sql, connection);
                cmd.Parameters.AddWithValue("$apiId", apiId);
                cmd.Parameters.AddWithValue("$login", login.Trim());
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.MarcarFuncionarioSincronizado] Erro: {ex.Message}");
            }
        }

        public static List<Usuario> ObterFuncionarios()
        {
            var lista = new List<Usuario>();

            try
            {
                using var connection = GetConnection();
                connection.Open();

                string sql = "SELECT Id, Login, Nome, Funcionario, EmailConfirmado, Setor, Cargo, Telefone, Imagem, Restricoes FROM Funcionarios ORDER BY Nome ASC";
                using var cmd = new SqliteCommand(sql, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Usuario
                    {
                        Id = reader.GetInt32(0),
                        Login = reader.GetString(1),
                        Nome = reader.GetString(2),
                        Funcionario = reader.GetInt32(3) == 1,
                        EmailConfirmado = reader.GetInt32(4) == 1,
                        Setor = reader.IsDBNull(5) ? "Administração" : reader.GetString(5),
                        CargoEspecifico = reader.IsDBNull(6) ? null : reader.GetString(6),
                        Telefone = reader.IsDBNull(7) ? "-" : reader.GetString(7),
                        Imagem = reader.IsDBNull(8) ? null : reader.GetString(8),
                        MascaraRestricoes = reader.GetInt32(9)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.ObterFuncionarios] Erro: {ex.Message}");
            }

            return lista;
        }

        public static bool DeletarFuncionario(int id)
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                string sql = "DELETE FROM Funcionarios WHERE Id = $id";
                using var cmd = new SqliteCommand(sql, connection);
                cmd.Parameters.AddWithValue("$id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.DeletarFuncionario] Erro: {ex.Message}");
                return false;
            }
        }

        // ==========================================
        // PRATOS
        // ==========================================
        public static void SalvarPratosApi(List<Prato> pratos, bool listaCompleta = true, bool nutricaoPendente = false)
        {
            if (pratos == null) return;

            try
            {
                using var connection = GetConnection();
                connection.Open();
                using var transaction = connection.BeginTransaction();

                // Remove pratos mockados residuais ou pratos excluídos da API (preservando cadastros offline com PendenteEnvio = 1)
                var apiIds = pratos.Select(p => p.Id).Where(id => id > 0).ToList();
                if (listaCompleta && (apiIds.Count > 0 || pratos.Count == 0))
                {
                    string idsIn = string.Join(",", apiIds);
                    string filtro = apiIds.Count == 0 ? "" : $" AND (ApiId NOT IN ({idsIn}) OR ApiId IS NULL)";
                    using var delCmd = new SqliteCommand($"DELETE FROM Pratos WHERE PendenteEnvio = 0{filtro};", connection, transaction);
                    delCmd.ExecuteNonQuery();
                }

                foreach (var p in pratos)
                {
                    try
                    {
                        // Não sobrescreve pratos pendentes de envio
                        // Se já existe com esse ApiId ou mesmo nome, atualiza
                        using var checkCmd = new SqliteCommand("SELECT Id, PendenteEnvio FROM Pratos WHERE ApiId = $apiId ORDER BY PendenteEnvio DESC, Id LIMIT 1;", connection, transaction);
                        checkCmd.Parameters.AddWithValue("$apiId", p.Id);
                        int? existingId;
                        using (var existente = checkCmd.ExecuteReader())
                        {
                            if (existente.Read())
                            {
                                if (existente.GetInt32(1) == 1 && !nutricaoPendente) continue;
                                existingId = existente.GetInt32(0);
                            }
                            else
                            {
                                existingId = null;
                            }
                        }

                        string? localImg = p.LocalImagePath;

                        if (existingId != null)
                        {
                            string updateSql = @"
                                UPDATE Pratos SET
                                    ApiId = $apiId,
                                    Nome = $nome, Descricao = $desc, Vegano = $vegano, ImagemUrl = $img,
                                    NotaTecnica = $nota, DescricaoIA = $ia, CategoriaId = $catId, CategoriaDescricao = $catDesc,
                                    Calorias = $kcal, Proteinas = $prot, Carboidratos = $carb, Gorduras = $gord,
                                    Alergenicos = $alergenicos, PendenteEnvio = $pendente,
                                    LocalImagePath = COALESCE($localImg, LocalImagePath)
                                WHERE Id = $id;
                            ";
                            using var updCmd = new SqliteCommand(updateSql, connection, transaction);
                            updCmd.Parameters.AddWithValue("$id", existingId);
                            updCmd.Parameters.AddWithValue("$apiId", p.Id);
                            updCmd.Parameters.AddWithValue("$pendente", nutricaoPendente ? 1 : 0);
                            updCmd.Parameters.AddWithValue("$localImg", (object?)localImg ?? DBNull.Value);
                            AdicionarParametrosPrato(updCmd, p);
                            updCmd.ExecuteNonQuery();
                        }
                        else
                        {
                            string insertSql = @"
                                INSERT INTO Pratos (ApiId, Nome, Descricao, Vegano, ImagemUrl, NotaTecnica, DescricaoIA, CategoriaId, CategoriaDescricao, Calorias, Proteinas, Carboidratos, Gorduras, LocalImagePath, PendenteEnvio, Alergenicos)
                                VALUES ($apiId, $nome, $desc, $vegano, $img, $nota, $ia, $catId, $catDesc, $kcal, $prot, $carb, $gord, $localImg, $pendente, $alergenicos);
                            ";
                            using var insCmd = new SqliteCommand(insertSql, connection, transaction);
                            insCmd.Parameters.AddWithValue("$apiId", p.Id);
                            insCmd.Parameters.AddWithValue("$pendente", nutricaoPendente ? 1 : 0);
                            insCmd.Parameters.AddWithValue("$localImg", (object?)localImg ?? DBNull.Value);
                            AdicionarParametrosPrato(insCmd, p);
                            insCmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception exPrato)
                    {
                        Console.WriteLine($"[SQLiteDatabase.SalvarPratosApi] Erro no prato '{p.Nome}': {exPrato.Message}");
                    }
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.SalvarPratosApi] Erro: {ex.Message}");
            }
        }

        public static int SalvarPratoLocal(Prato prato, string? localImagePath, bool pendenteEnvio = true)
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                string sql = @"
                    INSERT INTO Pratos (ApiId, Nome, Descricao, Vegano, ImagemUrl, NotaTecnica, DescricaoIA, CategoriaId, CategoriaDescricao, Calorias, Proteinas, Carboidratos, Gorduras, LocalImagePath, PendenteEnvio, Alergenicos)
                    VALUES ($apiId, $nome, $desc, $vegano, $img, $nota, $ia, $catId, $catDesc, $kcal, $prot, $carb, $gord, $localImg, $pendente, $alergenicos);
                    SELECT last_insert_rowid();
                ";

                using var cmd = new SqliteCommand(sql, connection);
                cmd.Parameters.AddWithValue("$apiId", prato.Id > 0 ? prato.Id : (object)DBNull.Value);
                AdicionarParametrosPrato(cmd, prato);
                cmd.Parameters.AddWithValue("$localImg", (object?)localImagePath ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$pendente", pendenteEnvio ? 1 : 0);

                long newId = (long)(cmd.ExecuteScalar() ?? 0);
                return (int)newId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.SalvarPratoLocal] Erro: {ex.Message}");
                return 0;
            }
        }

        private static void AdicionarParametrosPrato(SqliteCommand cmd, Prato p)
        {
            cmd.Parameters.AddWithValue("$nome", p.Nome ?? "");
            cmd.Parameters.AddWithValue("$desc", (object?)p.Descricao ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$vegano", p.Vegano ? 1 : 0);
            cmd.Parameters.AddWithValue("$alergenicos", p.Alergenicos);
            cmd.Parameters.AddWithValue("$img", (object?)p.Imagem ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$nota", (object?)p.NotaTecnica ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$ia", (object?)p.DescricaoIA ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$catId", p.Categoria?.Id ?? 1);
            cmd.Parameters.AddWithValue("$catDesc", p.Categoria?.Descricao ?? "Carnes");

            double calorias = p.ValorNutricional != null && p.ValorNutricional.Kcal > 0 ? p.ValorNutricional.Kcal : p.Calorias;
            double proteinas = p.ValorNutricional != null && p.ValorNutricional.Proteinas > 0 ? p.ValorNutricional.Proteinas : p.Proteinas;
            double carboidratos = p.ValorNutricional != null && p.ValorNutricional.Carboidratos > 0 ? p.ValorNutricional.Carboidratos : p.Carboidratos;
            double gorduras = p.ValorNutricional != null && p.ValorNutricional.Lipidios > 0 ? p.ValorNutricional.Lipidios : p.Gorduras;

            cmd.Parameters.AddWithValue("$kcal", calorias);
            cmd.Parameters.AddWithValue("$prot", proteinas);
            cmd.Parameters.AddWithValue("$carb", carboidratos);
            cmd.Parameters.AddWithValue("$gord", gorduras);
        }

        public static List<Prato> ObterPratos()
        {
            var lista = new List<Prato>();

            try
            {
                using var connection = GetConnection();
                connection.Open();

                string sql = @"
                    SELECT Id, ApiId, Nome, Descricao, Vegano, ImagemUrl, NotaTecnica, DescricaoIA,
                           CategoriaId, CategoriaDescricao, Calorias, Proteinas, Carboidratos, Gorduras, LocalImagePath, PendenteEnvio, Alergenicos
                    FROM Pratos
                    ORDER BY CategoriaId ASC, Nome ASC;
                ";

                using var cmd = new SqliteCommand(sql, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int localId = reader.GetInt32(0);
                    int? apiId = reader.IsDBNull(1) ? null : reader.GetInt32(1);

                    double kcal = reader.IsDBNull(10) ? 0 : reader.GetDouble(10);
                    double prot = reader.IsDBNull(11) ? 0 : reader.GetDouble(11);
                    double carb = reader.IsDBNull(12) ? 0 : reader.GetDouble(12);
                    double gord = reader.IsDBNull(13) ? 0 : reader.GetDouble(13);

                    string? localImg = reader.IsDBNull(14) ? null : reader.GetString(14);
                    if (string.IsNullOrEmpty(localImg) || !File.Exists(localImg))
                    {
                        string pastaImagens = Path.Combine(pasta, "imagens");
                        int chaveId = apiId ?? localId;
                        string[] extensoes = { ".png", ".jpg", ".jpeg", ".webp" };
                        foreach (var ext in extensoes)
                        {
                            string cand = Path.Combine(pastaImagens, $"prato_{chaveId}{ext}");
                            if (File.Exists(cand) && new FileInfo(cand).Length > 0)
                            {
                                localImg = cand;
                                break;
                            }
                        }
                    }

                    var prato = new Prato
                    {
                        Id = apiId ?? -localId,
                        Nome = reader.GetString(2),
                        Descricao = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        Vegano = reader.GetInt32(4) == 1,
                        Alergenicos = reader.GetInt32(16),
                        Imagem = reader.IsDBNull(5) ? null : reader.GetString(5),
                        NotaTecnica = reader.IsDBNull(6) ? null : reader.GetString(6),
                        DescricaoIA = reader.IsDBNull(7) ? null : reader.GetString(7),
                        Categoria = new Categoria
                        {
                            Id = reader.IsDBNull(8) ? 1 : reader.GetInt32(8),
                            Descricao = reader.IsDBNull(9) ? "Carnes" : reader.GetString(9)
                        },
                        Calorias = kcal,
                        Proteinas = prot,
                        Carboidratos = carb,
                        Gorduras = gord,
                        LocalImagePath = localImg,
                        ValorNutricional = new ValorNutricionalModel
                        {
                            Kcal = (float)kcal,
                            Proteinas = (float)prot,
                            Carboidratos = (float)carb,
                            Lipidios = (float)gord,
                            Medida = "100g"
                        }
                    };

                    lista.Add(prato);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.ObterPratos] Erro: {ex.Message}");
            }

            return lista;
        }

        public class PratoPendenteInfo
        {
            public int LocalId { get; set; }
            public Prato Prato { get; set; } = new Prato();
            public string? LocalImagePath { get; set; }
        }

        public static List<PratoPendenteInfo> ObterPratosPendentes()
        {
            var pendentes = new List<PratoPendenteInfo>();

            try
            {
                using var connection = GetConnection();
                connection.Open();

                string sql = @"
                    SELECT Id, Nome, Descricao, Vegano, NotaTecnica, DescricaoIA, CategoriaId, CategoriaDescricao,
                           Calorias, Proteinas, Carboidratos, Gorduras, LocalImagePath, ApiId, Alergenicos
                    FROM Pratos
                    WHERE PendenteEnvio = 1;
                ";

                using var cmd = new SqliteCommand(sql, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var info = new PratoPendenteInfo
                    {
                        LocalId = reader.GetInt32(0),
                        Prato = new Prato
                        {
                            Id = reader.IsDBNull(13) ? 0 : reader.GetInt32(13),
                            Nome = reader.GetString(1),
                            Descricao = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            Vegano = reader.GetInt32(3) == 1,
                            Alergenicos = reader.GetInt32(14),
                            NotaTecnica = reader.IsDBNull(4) ? null : reader.GetString(4),
                            DescricaoIA = reader.IsDBNull(5) ? null : reader.GetString(5),
                            Categoria = new Categoria
                            {
                                Id = reader.IsDBNull(6) ? 1 : reader.GetInt32(6),
                                Descricao = reader.IsDBNull(7) ? "Carnes" : reader.GetString(7)
                            },
                            Calorias = reader.IsDBNull(8) ? 0 : reader.GetDouble(8),
                            Proteinas = reader.IsDBNull(9) ? 0 : reader.GetDouble(9),
                            Carboidratos = reader.IsDBNull(10) ? 0 : reader.GetDouble(10),
                            Gorduras = reader.IsDBNull(11) ? 0 : reader.GetDouble(11)
                        },
                        LocalImagePath = reader.IsDBNull(12) ? null : reader.GetString(12)
                    };
                    pendentes.Add(info);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.ObterPratosPendentes] Erro: {ex.Message}");
            }

            return pendentes;
        }

        public static bool RegistrarEnvioPrato(int localId, int apiId, bool pendente, string? imagemUrl = null)
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                using var transaction = connection.BeginTransaction();
                string sql = "UPDATE Pratos SET ApiId = $apiId, PendenteEnvio = $pendente, ImagemUrl = COALESCE($img, ImagemUrl) WHERE Id = $id;";
                using var cmd = new SqliteCommand(sql, connection, transaction);
                cmd.Parameters.AddWithValue("$apiId", apiId);
                cmd.Parameters.AddWithValue("$pendente", pendente ? 1 : 0);
                cmd.Parameters.AddWithValue("$img", (object?)imagemUrl ?? DBNull.Value);
                cmd.Parameters.AddWithValue("$id", localId);
                if (cmd.ExecuteNonQuery() == 0) return false;

                using var removerDuplicata = new SqliteCommand(
                    "DELETE FROM Pratos WHERE ApiId = $apiId AND Id <> $id AND PendenteEnvio = 0;", connection, transaction);
                removerDuplicata.Parameters.AddWithValue("$apiId", apiId);
                removerDuplicata.Parameters.AddWithValue("$id", localId);
                removerDuplicata.ExecuteNonQuery();
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.RegistrarEnvioPrato] Erro: {ex.Message}");
                return false;
            }
        }

        public static bool AtualizarPratoPendente(Prato prato, string? imagem)
        {
            try
            {
                using var conexao = GetConnection();
                conexao.Open();
                using var comando = new SqliteCommand(
                    """
                    UPDATE Pratos SET Nome = $nome, Descricao = $descricao, Vegano = $vegano, CategoriaId = $categoria,
                        CategoriaDescricao = $categoriaNome, LocalImagePath = COALESCE($imagem, LocalImagePath),
                        Calorias = $kcal, Proteinas = $proteinas, Carboidratos = $carboidratos, Gorduras = $gorduras,
                        Alergenicos = $alergenicos
                    WHERE Id = $id AND PendenteEnvio = 1 AND ApiId IS NULL;
                    """, conexao);
                comando.Parameters.AddWithValue("$id", -prato.Id);
                comando.Parameters.AddWithValue("$nome", prato.Nome);
                comando.Parameters.AddWithValue("$descricao", prato.Descricao);
                comando.Parameters.AddWithValue("$vegano", prato.Vegano ? 1 : 0);
                comando.Parameters.AddWithValue("$categoria", (object?)prato.Categoria?.Id ?? DBNull.Value);
                comando.Parameters.AddWithValue("$categoriaNome", (object?)prato.Categoria?.Descricao ?? DBNull.Value);
                comando.Parameters.AddWithValue("$imagem", (object?)imagem ?? DBNull.Value);
                comando.Parameters.AddWithValue("$kcal", prato.Calorias);
                comando.Parameters.AddWithValue("$proteinas", prato.Proteinas);
                comando.Parameters.AddWithValue("$carboidratos", prato.Carboidratos);
                comando.Parameters.AddWithValue("$gorduras", prato.Gorduras);
                comando.Parameters.AddWithValue("$alergenicos", prato.Alergenicos);
                return comando.ExecuteNonQuery() == 1;
            }
            catch (SqliteException) { return false; }
        }

        public static bool DeletarPrato(int id)
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                string sql = "DELETE FROM Pratos WHERE ($id > 0 AND ApiId = $id) OR ($id < 0 AND Id = -$id AND ApiId IS NULL);";
                using var cmd = new SqliteCommand(sql, connection);
                cmd.Parameters.AddWithValue("$id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.DeletarPrato] Erro: {ex.Message}");
                return false;
            }
        }

        // ==========================================
        // AVISOS / MURAL
        // ==========================================
        public static void SalvarAvisosApi(List<Aviso> avisos, bool listaCompleta = true)
        {
            if (avisos == null) return;

            try
            {
                using var connection = GetConnection();
                connection.Open();
                using var transaction = connection.BeginTransaction();

                // Remove avisos mockados ou excluídos da API (preservando cadastros offline pendentes)
                var apiIds = avisos.Select(a => a.Id).Where(id => id > 0).ToList();
                if (listaCompleta && (apiIds.Count > 0 || avisos.Count == 0))
                {
                    string idsIn = string.Join(",", apiIds);
                    string filtro = apiIds.Count == 0 ? "" : $" AND (ApiId NOT IN ({idsIn}) OR ApiId IS NULL)";
                    using var delCmd = new SqliteCommand($"DELETE FROM Avisos WHERE PendenteEnvio = 0{filtro};", connection, transaction);
                    delCmd.ExecuteNonQuery();
                }

                foreach (var a in avisos)
                {
                    using var checkCmd = new SqliteCommand("SELECT Id FROM Avisos WHERE ApiId = $apiId AND PendenteEnvio = 0;", connection, transaction);
                    checkCmd.Parameters.AddWithValue("$apiId", a.Id);
                    var existingId = checkCmd.ExecuteScalar();

                    if (existingId != null)
                    {
                        string updSql = @"
                            UPDATE Avisos SET
                                Titulo = $titulo, Descricao = $desc, Model = $model,
                                DataInicial = $di, DataFinal = $df, DataCriacao = $dc, UserId = $user
                            WHERE Id = $id;
                        ";
                        using var updCmd = new SqliteCommand(updSql, connection, transaction);
                        updCmd.Parameters.AddWithValue("$id", existingId);
                        AdicionarParametrosAviso(updCmd, a);
                        updCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        string insSql = @"
                            INSERT INTO Avisos (ApiId, Titulo, Descricao, Model, DataInicial, DataFinal, DataCriacao, UserId, PendenteEnvio)
                            VALUES ($apiId, $titulo, $desc, $model, $di, $df, $dc, $user, 0);
                        ";
                        using var insCmd = new SqliteCommand(insSql, connection, transaction);
                        insCmd.Parameters.AddWithValue("$apiId", a.Id);
                        AdicionarParametrosAviso(insCmd, a);
                        insCmd.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.SalvarAvisosApi] Erro: {ex.Message}");
            }
        }

        public static int SalvarAvisoLocal(Aviso aviso, bool pendenteEnvio = true)
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                string sql = @"
                    INSERT INTO Avisos (ApiId, Titulo, Descricao, Model, DataInicial, DataFinal, DataCriacao, UserId, PendenteEnvio)
                    VALUES ($apiId, $titulo, $desc, $model, $di, $df, $dc, $user, $pendente);
                    SELECT last_insert_rowid();
                ";

                using var cmd = new SqliteCommand(sql, connection);
                cmd.Parameters.AddWithValue("$apiId", aviso.Id > 0 ? aviso.Id : (object)DBNull.Value);
                AdicionarParametrosAviso(cmd, aviso);
                cmd.Parameters.AddWithValue("$pendente", pendenteEnvio ? 1 : 0);

                long newId = (long)(cmd.ExecuteScalar() ?? 0);
                return (int)newId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.SalvarAvisoLocal] Erro: {ex.Message}");
                return 0;
            }
        }

        private static void AdicionarParametrosAviso(SqliteCommand cmd, Aviso a)
        {
            cmd.Parameters.AddWithValue("$titulo", a.Titulo ?? "");
            cmd.Parameters.AddWithValue("$desc", a.Descricao ?? "");
            cmd.Parameters.AddWithValue("$model", a.Model ?? "Mural");
            cmd.Parameters.AddWithValue("$di", a.DataInicial?.ToString("yyyy-MM-dd") ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$df", a.DataFinal?.ToString("yyyy-MM-dd") ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$dc", (object?)a.DataCriacao?.ToString("yyyy-MM-dd HH:mm:ss") ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$user", a.User?.Id ?? (UsuarioSession.UsuarioLogado?.Id ?? 1));
        }

        public static List<Aviso> ObterAvisos()
        {
            var lista = new List<Aviso>();

            try
            {
                using var connection = GetConnection();
                connection.Open();

                string sql = "SELECT Id, ApiId, Titulo, Descricao, Model, DataInicial, DataFinal, DataCriacao, UserId FROM Avisos ORDER BY Id DESC;";
                using var cmd = new SqliteCommand(sql, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int localId = reader.GetInt32(0);
                    int? apiId = reader.IsDBNull(1) ? null : reader.GetInt32(1);

                    var aviso = new Aviso
                    {
                        Id = apiId ?? -localId,
                        Titulo = reader.GetString(2),
                        Descricao = reader.GetString(3),
                        Model = reader.IsDBNull(4) ? "Mural" : reader.GetString(4),
                        DataInicial = reader.IsDBNull(5) ? null : DateTime.TryParse(reader.GetString(5), out var di) ? di : null,
                        DataFinal = reader.IsDBNull(6) ? null : DateTime.TryParse(reader.GetString(6), out var df) ? df : null,
                        DataCriacao = reader.IsDBNull(7) ? null : DateTime.TryParse(reader.GetString(7), out var dc) ? dc : null,
                        User = new Usuario { Id = reader.IsDBNull(8) ? 1 : reader.GetInt32(8) }
                    };

                    lista.Add(aviso);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.ObterAvisos] Erro: {ex.Message}");
            }

            return lista;
        }

        public class AvisoPendenteInfo
        {
            public int LocalId { get; set; }
            public Aviso Aviso { get; set; } = new Aviso();
        }

        public static List<AvisoPendenteInfo> ObterAvisosPendentes()
        {
            var pendentes = new List<AvisoPendenteInfo>();

            try
            {
                using var connection = GetConnection();
                connection.Open();

                string sql = "SELECT Id, Titulo, Descricao, Model, DataInicial, DataFinal, DataCriacao, UserId FROM Avisos WHERE PendenteEnvio = 1;";
                using var cmd = new SqliteCommand(sql, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var info = new AvisoPendenteInfo
                    {
                        LocalId = reader.GetInt32(0),
                        Aviso = new Aviso
                        {
                            Titulo = reader.GetString(1),
                            Descricao = reader.GetString(2),
                            Model = reader.IsDBNull(3) ? "Mural" : reader.GetString(3),
                            DataInicial = reader.IsDBNull(4) ? null : DateTime.TryParse(reader.GetString(4), out var di) ? di : null,
                            DataFinal = reader.IsDBNull(5) ? null : DateTime.TryParse(reader.GetString(5), out var df) ? df : null,
                            DataCriacao = reader.IsDBNull(6) ? null : DateTime.TryParse(reader.GetString(6), out var dc) ? dc : null,
                            User = new Usuario { Id = reader.IsDBNull(7) ? 1 : reader.GetInt32(7) }
                        }
                    };
                    pendentes.Add(info);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.ObterAvisosPendentes] Erro: {ex.Message}");
            }

            return pendentes;
        }

        public static void MarcarAvisoSincronizado(int localId, int apiId)
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                string sql = "UPDATE Avisos SET ApiId = $apiId, PendenteEnvio = 0 WHERE Id = $id;";
                using var cmd = new SqliteCommand(sql, connection);
                cmd.Parameters.AddWithValue("$apiId", apiId);
                cmd.Parameters.AddWithValue("$id", localId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.MarcarAvisoSincronizado] Erro: {ex.Message}");
            }
        }

        public static bool AtualizarAvisoPendente(Aviso aviso)
        {
            try
            {
                using var conexao = GetConnection();
                conexao.Open();
                using var comando = new SqliteCommand(
                    """
                    UPDATE Avisos SET Titulo = $titulo, Descricao = $desc, Model = $model,
                        DataInicial = $di, DataFinal = $df, DataCriacao = $dc, UserId = $user
                    WHERE Id = $id AND PendenteEnvio = 1 AND ApiId IS NULL;
                    """, conexao);
                comando.Parameters.AddWithValue("$id", -aviso.Id);
                AdicionarParametrosAviso(comando, aviso);
                return comando.ExecuteNonQuery() == 1;
            }
            catch (SqliteException) { return false; }
        }

        public static bool DeletarAviso(int id)
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                string sql = "DELETE FROM Avisos WHERE ($id > 0 AND ApiId = $id) OR ($id < 0 AND Id = -$id AND ApiId IS NULL);";
                using var cmd = new SqliteCommand(sql, connection);
                cmd.Parameters.AddWithValue("$id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.DeletarAviso] Erro: {ex.Message}");
                return false;
            }
        }

        // ==========================================
        // CARDAPIO SEMANAL
        // ==========================================
        public static void SalvarCardapioSemana(DateTime segunda, string jsonData)
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                string chave = segunda.ToString("yyyy-MM-dd");
                string sql = @"
                    INSERT INTO CardapioSemana (SegundaFeira, JsonData)
                    VALUES ($segunda, $json)
                    ON CONFLICT(SegundaFeira) DO UPDATE SET JsonData = excluded.JsonData;
                ";

                using var cmd = new SqliteCommand(sql, connection);
                cmd.Parameters.AddWithValue("$segunda", chave);
                cmd.Parameters.AddWithValue("$json", jsonData);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.SalvarCardapioSemana] Erro: {ex.Message}");
            }
        }

        public static string? ObterCardapioSemana(DateTime segunda)
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                string chave = segunda.ToString("yyyy-MM-dd");
                string sql = "SELECT JsonData FROM CardapioSemana WHERE SegundaFeira = $segunda;";
                using var cmd = new SqliteCommand(sql, connection);
                cmd.Parameters.AddWithValue("$segunda", chave);

                var res = cmd.ExecuteScalar();
                return res as string;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.ObterCardapioSemana] Erro: {ex.Message}");
                return null;
            }
        }
    }
}
