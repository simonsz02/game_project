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
        //Not used anymore
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }
        /// <summary>
        /// If the window is closed during a game, its state will be saved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Content.GetType().Name== "TowerDefenseControl")
            {
                SerializationAsBinary.Export<TowerDefenseModel>("TowerDefenseLastState.bin", ((TowerDefenseControl)Content).Model);
            }
        }
    }
}
