using System;
using System.Collections.Generic;
using System.IO;
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
using TowerDefenseGame.GameItems;
using TowerDefenseGame.Repository;

namespace TowerDefenseGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        //TODO! create and load mainmenu that is loaded here
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists("TowerDefenseLastState.bin"))
            {
                if (MessageBox.Show("Press YES to load last state or NO to start new game?", "Load last game", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    ((TowerDefenseControl)Content).Model = SerializationAsBinary.Import<TowerDefenseModel>("TowerDefenseLastState.bin");
                }
            }
        }            

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SerializationAsBinary.Export<TowerDefenseModel>("TowerDefenseLastState.bin", ((TowerDefenseControl)Content).Model);
        }
    }
}
