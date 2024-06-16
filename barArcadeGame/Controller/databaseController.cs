using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace barArcadeGame._Managers
{
    internal class databaseController
    {
        //private string connectionString = "Data Source=YourDatabase.db;Version=3;";
        private static string connectionString = @"Data Source=..\..\Files\database.db; Version=3;";
        public static void InitializeDatabase()
        {
            if (!File.Exists(@"..\..\Files\LibraryManagementSystem.db"))
            {
                SQLiteConnection.CreateFile(@"..\..\Files\database.db");

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string createBooksTableQuery = @"
                    CREATE TABLE IF NOT EXISTS highscores (
                        id INTEGER PRIMARY KEY AUTOINCREMENT, 
                        whackamole INTEGER NOT NULL,
                        quiz INTEGER NOT NULL
                    );";

                    string createUsersTableQuery = @"
                    CREATE TABLE IF NOT EXISTS coins (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        value INTEGER NOT NULL
                    );";

                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = createBooksTableQuery;
                        command.ExecuteNonQuery();
                        command.CommandText = createUsersTableQuery;
                        command.ExecuteNonQuery();

                    }
                }
            }
        }

        public static void highscorefaker()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                int whackamole = 0;
                int quiz = 0;

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText =
                    @"INSERT INTO highscores (whackamole, quiz) VALUES (@whackamole, @quiz);";
                    command.Parameters.AddWithValue("@whackamole", whackamole);
                    command.Parameters.AddWithValue("@quiz", quiz);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
            }
        }

        public static void coinfaker()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                int value = 0;

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText =
                    @"INSERT INTO coins (value) VALUES (@value);";
                    command.Parameters.AddWithValue("@value", value);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
            }
        }

        public static void UpdateWhackamoleHighScore(int newScore)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT whackamole FROM highscores ORDER BY whackamole DESC LIMIT 1;";

                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    object result = command.ExecuteScalar();
                    int currentHighScore = result != null ? Convert.ToInt32(result) : 0;

                    if (newScore > currentHighScore)
                    {
                        string updateQuery = "INSERT INTO highscores (whackamole,quiz) VALUES (@whackamole, 0);";

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
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT quiz FROM highscores ORDER BY whackamole DESC LIMIT 1;";

                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    object result = command.ExecuteScalar();
                    int currentHighScore = result != null ? Convert.ToInt32(result) : 0;

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
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT value FROM coins LIMIT 1;";
                string updateQuery = "UPDATE coins SET value = @newValue WHERE id = 1;";

                using (var selectCommand = new SQLiteCommand(selectQuery, connection))
                using (var updateCommand = new SQLiteCommand(updateQuery, connection))
                {
                    object result = selectCommand.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        int currentValue = Convert.ToInt32(result);
                        int newValue = currentValue + valueToAdd;

                        updateCommand.Parameters.AddWithValue("@newValue", newValue);
                        updateCommand.ExecuteNonQuery();
                    }
                }
            }
        }


        public static int GetHighestWhackamoleScore()
        {
            int highestScore = 0;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT MAX(whackamole) FROM highscores;";

                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        highestScore = Convert.ToInt32(result);
                    }
                }
            }

            return highestScore;
        }

        public static int GetHighestQuizScore()
        {
            int highestScore = 0;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT MAX(quiz) FROM highscores;";

                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        highestScore = Convert.ToInt32(result);
                    }
                }
            }
            return highestScore;
        }

        public static int GetCoinValue()
        {
            int highestScore = 0;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT MAX(value) FROM coins;";

                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        highestScore = Convert.ToInt32(result);
                    }
                }
            }

            return highestScore;
        }

        public static void RemoveCoinValue()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT value FROM coins LIMIT 1;";
                string updateQuery = "UPDATE coins SET value = @newValue WHERE id = 1;";

                using (var selectCommand = new SQLiteCommand(selectQuery, connection))
                using (var updateCommand = new SQLiteCommand(updateQuery, connection))
                {
                    object result = selectCommand.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        int currentValue = Convert.ToInt32(result);

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
}
