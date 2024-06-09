using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Menu
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {

		private bool isMuted = false;

		public event EventHandler<double> VolumeChanged;

		public SettingsWindow(double currentVolume)
        {
            InitializeComponent();
			sliderVolume.Value = currentVolume * 100; 
		}

		 private void sliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VolumeChanged?.Invoke(this, sliderVolume.Value / 100);


		}

		private void btnMute_Click(object sender, RoutedEventArgs e)
		{
			if (isMuted)
			{
				imgMute.Source = new BitmapImage(new Uri("C:\\Users\\Pepelito\\Downloads\\Image\\sound2.png", UriKind.Absolute));
				sliderVolume.Value = 30.0; // Set to a default volume when unmuted
			}
			else
			{
				imgMute.Source = new BitmapImage(new Uri("C:\\Users\\Pepelito\\Downloads\\Image\\muteU2.png", UriKind.Absolute));
				sliderVolume.Value = 0;
			}

			isMuted = !isMuted;


		}
	}
}
