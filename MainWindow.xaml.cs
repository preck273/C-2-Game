using barArcadeGame;
using Games.Files;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Games
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SoundPlayer win = new SoundPlayer(@"C:\Users\g3k01\OneDrive\Desktop\BarArcade\Games\sounds\win.wav");
        public MainWindow()
        {
            InitializeComponent();

            //database.InitializeDatabase();
            //database.coinfaker();
            //database.highscorefaker();

        }
        

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void exitClick(object sender, RoutedEventArgs e)
        {
            TitlePage.Visibility = Visibility.Collapsed;
            ArcadeMain.Content = new WhackAMoleMain();

        }

        private void gameClick(object sender, RoutedEventArgs e)
        {

            win.Play();
            var game = new Game1();
            game.Run();
        }

        public void NavigateToMainWindow()
        {
            ArcadeMain.Content = null;
            TitlePage.Visibility = Visibility.Visible;
        }
       

    }
}
