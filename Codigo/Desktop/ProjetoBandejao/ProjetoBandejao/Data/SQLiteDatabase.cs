using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;
using ProjetoBandejao.Models;

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

        public static void Inicializar()
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                string sql = @"
                    CREATE TABLE IF NOT EXISTS Funcionarios (
                        Id INTEGER PRIMARY KEY,
                        Login TEXT NOT NULL UNIQUE,
                        Nome TEXT NOT NULL,
                        Funcionario INTEGER NOT NULL,
                        EmailConfirmado INTEGER NOT NULL DEFAULT 1
                    );
                ";

                using var command = new SqliteCommand(sql, connection);
                command.ExecuteNonQuery();

                // Garante compatibilidade caso o banco tenha sido criado em versão anterior sem a coluna EmailConfirmado
                try
                {
                    using var checkCmd = new SqliteCommand("ALTER TABLE Funcionarios ADD COLUMN EmailConfirmado INTEGER NOT NULL DEFAULT 1;", connection);
                    checkCmd.ExecuteNonQuery();
                }
                catch
                {
                    // A coluna já existe, ignorar
                }

                // Garante ao menos um usuário de contingência para acesso offline caso o banco esteja vazio
                try
                {
                    using var countCmd = new SqliteCommand("SELECT COUNT(*) FROM Funcionarios;", connection);
                    long total = (long)(countCmd.ExecuteScalar() ?? 0);
                    if (total == 0)
                    {
                        using var seedCmd = new SqliteCommand(
                            "INSERT OR IGNORE INTO Funcionarios (Id, Login, Nome, Funcionario, EmailConfirmado) VALUES (1, 'admin@cotil.unicamp.br', 'Administrador (Offline)', 1, 1);",
                            connection);
                        seedCmd.ExecuteNonQuery();
                    }
                }
                catch
                {
                    // Ignora falha de seed
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.Inicializar] Erro: {ex.Message}");
            }
        }
        
        public static void SalvarFuncionarios(List<Usuario> usuarios)
        {
            if (usuarios == null || usuarios.Count == 0) return;

            try
            {
                using var connection = GetConnection();
                connection.Open();

                using var transaction = connection.BeginTransaction();

                string sql = @"
                    INSERT OR REPLACE INTO Funcionarios (Id, Login, Nome, Funcionario, EmailConfirmado)
                    VALUES ($id, $login, $nome, $funcionario, $emailConfirmado);
                ";

                foreach (var u in usuarios)
                {
                    using var cmd = new SqliteCommand(sql, connection, transaction);
                    cmd.Parameters.AddWithValue("$id", u.Id);
                    cmd.Parameters.AddWithValue("$login", u.Login);
                    cmd.Parameters.AddWithValue("$nome", u.Nome);
                    cmd.Parameters.AddWithValue("$funcionario", u.Funcionario ? 1 : 0);
                    cmd.Parameters.AddWithValue("$emailConfirmado", u.EmailConfirmado ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.SalvarFuncionarios] Erro: {ex.Message}");
            }
        }

        public static List<Usuario> ObterFuncionarios()
        {
            var lista = new List<Usuario>();

            try
            {
                using var connection = GetConnection();
                connection.Open();

                string sql = "SELECT Id, Login, Nome, Funcionario, EmailConfirmado FROM Funcionarios ORDER BY Nome ASC";
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
                        EmailConfirmado = reader.GetInt32(4) == 1
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.ObterFuncionarios] Erro: {ex.Message}");
            }

            return lista;
        }

        public static void DeletarFuncionario(int id)
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                string sql = "DELETE FROM Funcionarios WHERE Id = $id";
                using var cmd = new SqliteCommand(sql, connection);
                cmd.Parameters.AddWithValue("$id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SQLiteDatabase.DeletarFuncionario] Erro: {ex.Message}");
            }
        }
    }
}