using barArcadeGame.View;
using Menu.Data;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Menu
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private string connectionString = @"Data Source=DESKTOP-MUE5L5M\SQLEXPRESS06;Initial Catalog=GameDB;Integrated Security=True;encrypt=false";

		private MediaPlayer mediaPlayer;

		public MainWindow()
		{
			InitializeComponent();
			SetImageSources();
		}

		private void SetImageSources()
		{
			// Set the background image
			bgImageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/bgImg.jpg"));

			// Set the images for buttons
			playImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Play.png"));
			scoreImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/score.png"));
			quitImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/quit2.png"));
			backImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/back2.png"));
		}


		private void MediaPlayer_MediaEnded(object sender, EventArgs e)
		{
			mediaPlayer.Position = TimeSpan.Zero; // Loop the music
			mediaPlayer.Play();
		}

		private void btnPlay_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
			var game = new Game1();
			//game.Run();

		}

		private void btnQuit_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void btnScore_Click(object sender, RoutedEventArgs e)
		{
			LoadPlayerScores();
		}

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			ScoresPanel.Visibility = Visibility.Collapsed;
			MainButtonsPanel.Visibility = Visibility.Visible;
		}

		private void LoadPlayerScores()
		{
			List<PlayerScore> scores = new List<PlayerScore>();

			string query = "SELECT PlayerName, HighScore FROM PlayerScores ORDER BY HighScore DESC";
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				SqlCommand command = new SqlCommand(query, connection);
				connection.Open();

				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						scores.Add(new PlayerScore
						{
							PlayerName = reader["PlayerName"].ToString(),
							HighScore = Convert.ToInt32(reader["HighScore"])
						});
					}
				}
			}

			dataGridScores.ItemsSource = scores;
			ScoresPanel.Visibility = Visibility.Visible;
			MainButtonsPanel.Visibility = Visibility.Collapsed;
		}
	}
}
