using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using System.Net.Http.Formatting;
using System.Windows.Threading;
using System.Media;
using Games.Files;
using barArcadeGame;

namespace Games
{
	/// <summary>
	/// Interaction logic for WhackAMoleGame.xaml
	/// </summary>
    public partial class WhackAMoleGame : Page
    {
        private int score;
        private int highscore;
        private readonly Random random = new Random();
        private DispatcherTimer gameTimer;
        private Rectangle currentMole;
        private int spawnTimer = 5;
        private int rate;
        private int health= 900;

        private SoundPlayer win = new SoundPlayer(@"..\..\sounds\win.wav");
        private SoundPlayer click = new SoundPlayer(@"..\..\sounds\click.wav");
        private SoundPlayer escape = new SoundPlayer(@"..\..\sounds\escape.wav");
        private SoundPlayer fail = new SoundPlayer(@"..\..\sounds\fail.wav");

        //playSound(fail);

        public WhackAMoleGame()
        {
            InitializeComponent();
            setupGame();
        }



        public void setupGame()
        {
            score = 0;
            Score.Content = "Score: " + score;
            health = 900;
            spawnTimer = 4;
            gameTimer = new DispatcherTimer();
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Interval = TimeSpan.FromSeconds(1);
            gameTimer.Start();

            rate = spawnTimer;
            showGame();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            rate -= 4;

            if (rate < 1)
            {
                GameCanvas.Children.Remove(currentMole);
                rate = spawnTimer;
                var mole = new Rectangle
                {
                    Width = 50,
                    Height = 50,
                    Fill = System.Windows.Media.Brushes.Brown,
                    RadiusX = 20,
                    RadiusY = 20
                };

                // Randomly position the mole within the canvas
                double canvasWidth = GameCanvas.ActualWidth - mole.Width;
                double canvasHeight = GameCanvas.ActualHeight - mole.Height;

                Canvas.SetLeft(mole, random.NextDouble() * canvasWidth);
                Canvas.SetTop(mole, random.NextDouble() * canvasHeight);

                currentMole = mole;
                GameCanvas.Children.Add(mole);
            }

            //GameCanvas.Children.OfType<Rectangle>();

            if ( health> 1)
            {
                LifeBar.Width = health;
            }
            else
            {
                showEnd();
            }

        }



        private void incrementScore()
        {
            score += 1;
            Score.Content = "Score: " + score;
        }

        private void destroyMole()
        {
            //GameCanvas.Children.Remove(currentMole);
            //playSound(escape);
        }

        private void leftClicked(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Rectangle)
            {
                Rectangle mole = (Rectangle)e.OriginalSource;
                GameCanvas.Children.Remove(mole);

                incrementScore();
                playSound(win);

            }
            else
            {
                health -= 300;
                playSound(fail);
            }
        }

        private void playSound(SoundPlayer name)
        {
            name.Load();
            name.Play();
        }

        private void showGame()
        {
            Score.Visibility = Visibility.Visible;
            Life.Visibility = Visibility.Visible;
            LifeBar.Visibility = Visibility.Visible;
            GameCanvas.Visibility = Visibility.Visible;
            endText.Visibility = Visibility.Collapsed;
            retry.Visibility = Visibility.Collapsed;
            toStore.Visibility = Visibility.Collapsed;
            Highscore.Visibility = Visibility.Collapsed;
        }

        private void showEnd()
        {
            database.UpdateWhackamoleHighScore(score);
            gameTimer.Stop();
            //GameCanvas.Children.Remove(currentMole);
            endText.Text = "Game Over";
            Score.Visibility = Visibility.Collapsed;
            Life.Visibility = Visibility.Collapsed;
            LifeBar.Visibility = Visibility.Collapsed;
            GameCanvas.Visibility = Visibility.Collapsed;
            endText.Visibility = Visibility.Visible;
            retry.Visibility = Visibility.Visible;
            toStore.Visibility = Visibility.Visible;

            updatecoins();
            Highscore.Content = "Highscore: " + database.GetHighestWhackamoleScore();
            Highscore.Visibility = Visibility.Visible;


        }

        private void updatecoins()
        {
            int scoretemp = score / 10;
            database.UpdateCoinsValue(scoretemp);
        }

        private void toStoreClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.NavigateToMainWindow();
            }
        }

        private void RetryClick(object sender, RoutedEventArgs e)
        {
            setupGame();
            showGame();
        }
    }
}
