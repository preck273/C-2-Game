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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Games
{
    /// <summary>
    /// Interaction logic for WhackAMoleMain.xaml
    /// </summary>
    public partial class WhackAMoleMain : Page
    {
        public WhackAMoleMain()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WhackAMoleGame whackAMole = new WhackAMoleGame();
            this.NavigationService.Navigate(whackAMole);

        }
    }
}
