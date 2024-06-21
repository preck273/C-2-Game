using System;
using System.Data.SQLite;
using System.IO;

namespace barArcadeGame.Managers
{
    internal static class DatabaseController
    {
        private static readonly string ConnectionString = @"Data Source=..\..\Files\database.db; Version=3;";

        public static void InitializeDatabase()
        {
            string databasePath = @"..\..\Files\database.db";
            if (!File.Exists(databasePath))
            {
                SQLiteConnection.CreateFile(databasePath);

                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    string createHighScoresTableQuery = @"
                    CREATE TABLE IF NOT EXISTS highscores (
                        id INTEGER PRIMARY KEY AUTOINCREMENT, 
                        whackamole INTEGER NOT NULL,
                        quiz INTEGER NOT NULL
                    );";

                    string createCoinsTableQuery = @"
                    CREATE TABLE IF NOT EXISTS coins (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        value INTEGER NOT NULL
                    );";

                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = createHighScoresTableQuery;
                        command.ExecuteNonQuery();
                        command.CommandText = createCoinsTableQuery;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void AddFakeHighScores()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                int whackamole = 0;
                int quiz = 0;

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"INSERT INTO highscores (whackamole, quiz) VALUES (@whackamole, @quiz);";
                    command.Parameters.AddWithValue("@whackamole", whackamole);
                    command.Parameters.AddWithValue("@quiz", quiz);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void AddFakeCoins()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                int value = 0;

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"INSERT INTO coins (value) VALUES (@value);";
                    command.Parameters.AddWithValue("@value", value);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateWhackamoleHighScore(int newScore)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string selectQuery = "SELECT MAX(whackamole) FROM highscores;";

                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    int currentHighScore = Convert.ToInt32(command.ExecuteScalar() ?? 0);

                    if (newScore > currentHighScore)
                    {
                        string updateQuery = "INSERT INTO highscores (whackamole, quiz) VALUES (@whackamole, 0);";

                        using (var updateCommand = new SQLiteCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@whackamole", newScore);
                            updateCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public static void UpdateQuizHighScore(int newScore)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string selectQuery = "SELECT MAX(quiz) FROM highscores;";

                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    int currentHighScore = Convert.ToInt32(command.ExecuteScalar() ?? 0);

                    if (newScore > currentHighScore)
                    {
                        string updateQuery = "INSERT INTO highscores (whackamole, quiz) VALUES (0, @quiz);";

                        using (var updateCommand = new SQLiteCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@quiz", newScore);
                            updateCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public static void UpdateCoinsValue(int valueToAdd)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string selectQuery = "SELECT value FROM coins LIMIT 1;";
                string updateQuery = "UPDATE coins SET value = @newValue WHERE id = 1;";

                using (var selectCommand = new SQLiteCommand(selectQuery, connection))
                using (var updateCommand = new SQLiteCommand(updateQuery, connection))
                {
                    int currentValue = Convert.ToInt32(selectCommand.ExecuteScalar() ?? 0);
                    int newValue = currentValue + valueToAdd;

                    updateCommand.Parameters.AddWithValue("@newValue", newValue);
                    updateCommand.ExecuteNonQuery();
                }
            }
        }

        public static int GetHighestWhackamoleScore()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string selectQuery = "SELECT MAX(whackamole) FROM highscores;";

                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar() ?? 0);
                }
            }
        }

        public static int GetHighestQuizScore()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string selectQuery = "SELECT MAX(quiz) FROM highscores;";

                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar() ?? 0);
                }
            }
        }

        public static int GetCoinValue()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string selectQuery = "SELECT MAX(value) FROM coins;";

                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar() ?? 0);
                }
            }
        }

        public static void DecreaseCoinValue()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string selectQuery = "SELECT value FROM coins LIMIT 1;";
                string updateQuery = "UPDATE coins SET value = @newValue WHERE id = 1;";

                using (var selectCommand = new SQLiteCommand(selectQuery, connection))
                using (var updateCommand = new SQLiteCommand(updateQuery, connection))
                {
                    int currentValue = Convert.ToInt32(selectCommand.ExecuteScalar() ?? 0);

                    if (currentValue > 0)
                    {
                        int newValue = currentValue - 1;
                        updateCommand.Parameters.AddWithValue("@newValue", newValue);
                        updateCommand.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
