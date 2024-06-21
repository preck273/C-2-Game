using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace Games.Files
{
    internal class database
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
                        currenthighscore INTEGER NOT NULL,
                        username INTEGER NOT NULL
                    );";

                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = createBooksTableQuery;
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
                int currenthighscore = 0;
                int username = 0;

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText =
                    @"INSERT INTO highscores (currenthighscore, username) VALUES (@currenthighscore, @username);";
                    command.Parameters.AddWithValue("@currenthighscore", currenthighscore);
                    command.Parameters.AddWithValue("@username", username);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
            }
        }



        public static void UpdateHighScore(int newScore, string username)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT currenthighscore FROM highscores ORDER BY currenthighscore DESC LIMIT 1;";

                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    object result = command.ExecuteScalar();
                    int currentHighScore = result != null ? Convert.ToInt32(result) : 0;

                    if (newScore > currentHighScore)
                    {
                        string updateQuery = "INSERT INTO highscores (currenthighscore,username) VALUES (@currenthighscore, @username);";

                        using (var updateCommand = new SQLiteCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@currenthighscore", newScore);
                            updateCommand.ExecuteNonQuery();
                            updateCommand.Parameters.AddWithValue("@username", username);
                            updateCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        //Add user
        public static int GetHighscore()
        {
            int highestScore = 0;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT MAX(currenthighscore) FROM highscores;";

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



    }
}