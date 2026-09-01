using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.IO;

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
            using var connection = GetConnection();

            connection.Open();

            string sql = @"
                CREATE TABLE IF NOT EXISTS Funcionarios (
                    Id INTEGER PRIMARY KEY,
                    Login TEXT NOT NULL UNIQUE,
                    Nome TEXT NOT NULL,
                    Funcionario INTEGER NOT NULL
                );
            ";

            using var command = new SqliteCommand(sql, connection);

            command.ExecuteNonQuery();
        }
    }
}