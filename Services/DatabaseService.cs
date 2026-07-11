using Microsoft.Data.Sqlite;
using PlayLimit.Models;
using System.IO;

namespace PlayLimit.Services;

public static class DatabaseService
{
    private static readonly string DbPath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "playlimit.db");

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
}