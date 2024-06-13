using BossFightGame;
using Menu.Data;
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

			mediaPlayer  = new MediaPlayer();

			mediaPlayer.Open(new Uri("C:\\Users\\Pepelito\\Downloads\\Sound\\backgroundMusic.mp3", UriKind.Relative));
			mediaPlayer.Volume = 0.5; // Set initial volume to 50%
			mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
			mediaPlayer.Play();
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
			game.Run();
			
		}

		private void btnSettings_Click(object sender, RoutedEventArgs e)
		{
			
			SettingsWindow settingsWindow = new SettingsWindow(mediaPlayer.Volume);
			settingsWindow.VolumeChanged += SettingsWindow_VolumeChanged;
			settingsWindow.ShowDialog();
		}

		private void SettingsWindow_VolumeChanged(object sender, double e)
		{
			mediaPlayer.Volume = e;
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
