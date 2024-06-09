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
			MessageBox.Show("Play button clicked!");
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
	}
}