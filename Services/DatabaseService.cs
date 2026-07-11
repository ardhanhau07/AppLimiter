using Microsoft.Data.Sqlite;
using AppLimit.Models;
using System.IO;

namespace AppLimit.Services;

public static class DatabaseService
{
    private static readonly string DbPath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "AppLimit.db");

    private static readonly string ConnectionString =
        $"Data Source={DbPath}";

    public static void Initialize()
    {
        //Create a Data Folder if it doesn't exist
        var dataFolder = Path.GetDirectoryName(DbPath);

        if (!string.IsNullOrEmpty(dataFolder) && !Directory.Exists(dataFolder))
        {
            Directory.CreateDirectory(dataFolder);
        }

        using var connection = GetConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS AppRules
            (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            ExePath TEXT NOT NULL,
            ProcessName TEXT NOT NULL,
            TimeLimit INTEGER NOT NULL,
            Enabled INTEGER NOT NULL
            );

            CREATE TABLE IF NOT EXISTS UsageLogs
            (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                RuleId INTEGER NOT NULL,
                Date TEXT NOT NULL,
                Minutes INTEGER NOT NULL
            );
        ";

        command.ExecuteNonQuery();
        connection.Close();
    }

    private static SqliteConnection GetConnection()
    {
        return new SqliteConnection(ConnectionString);
    }

    public static void AddRule(AppRule rule)
    {
        using var connection = GetConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
            INSERT INTO AppRules
            (
                Name,
                ExePath,
                ProcessName,
                TimeLimit,
                Enabled
            )
            VALUES
            (
                @Name,
                @ExePath,
                @ProcessName,
                @TimeLimit,
                @Enabled
            );
        ";

        command.Parameters.AddWithValue("@Name", rule.Name);
        command.Parameters.AddWithValue("@ExePath", rule.ExePath);
        command.Parameters.AddWithValue("@ProcessName", rule.ProcessName);
        command.Parameters.AddWithValue("@TimeLimit", rule.TimeLimit);
        command.Parameters.AddWithValue("@Enabled", rule.Enabled ? 1 : 0);

        command.ExecuteNonQuery();
    }

    public static List<AppRule> GetAllRules()
    {
        var rules = new List<AppRule>();
        using var connection = GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM AppRules";

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            rules.Add(new AppRule
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                ExePath = reader.GetString(2),
                ProcessName = reader.GetString(3),
                TimeLimit = reader.GetInt32(4),
                Enabled = reader.GetInt32(5) == 1
            });
        }
        return rules;
    }

    public static void UpdateRule(AppRule rule)
    {
        using var connection = GetConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
        UPDATE AppRules
        SET
            Name = $name,
            ExePath = $exe,
            ProcessName = $process,
            TimeLimit = $limit,
            Enabled = $enabled
        WHERE Id = $id;
    ";

        command.Parameters.AddWithValue("$id", rule.Id);
        command.Parameters.AddWithValue("$name", rule.Name);
        command.Parameters.AddWithValue("$exe", rule.ExePath);
        command.Parameters.AddWithValue("$process", rule.ProcessName);
        command.Parameters.AddWithValue("$limit", rule.TimeLimit);
        command.Parameters.AddWithValue("$enabled", rule.Enabled ? 1 : 0);

        command.ExecuteNonQuery();
    }

    public static void DeleteRule(int id)
    {
        using var connection = GetConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
            "DELETE FROM AppRules WHERE Id = $id";

        command.Parameters.AddWithValue("$id", id);

        command.ExecuteNonQuery();
    }
}