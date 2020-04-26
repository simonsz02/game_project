using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TowerDefenseGame.Menu;
using TowerDefenseGame.Repository;

namespace TowerDefenseGame
{
    class MenuControl : FrameworkElement
    {
        MenuModel model;
        MenuRenderer renderer;
        Window win;
        public MenuControl()
        {
            Loaded += MenuControl_Loaded;
        }
        private void MenuControl_Loaded(object sender, RoutedEventArgs e)
        {
            model = new MenuModel(ActualWidth, ActualHeight);
            renderer = new MenuRenderer(model);
            win = Window.GetWindow(this);
            if (win != null)
            {
                MouseDown += MenuControl_MouseDown;
            }
            InvalidateVisual();
        }

        private void MenuControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(this);
            for (int i = 0; i < renderer.MenuItems.Length; i++)
            {
                if (renderer.MenuItems[i].Bounds.Contains(mousePos))
                {
                    switch (i)
                    {
                        case 0:
                            win.Content = new TowerDefenseControl();
                            break;
                        case 1:
                            if (File.Exists("TowerDefenseLastState.bin"))
                            {
                                win.Content = new TowerDefenseControl();
                                ((TowerDefenseControl)win.Content).Model = SerializationAsBinary.Import<TowerDefenseModel>("TowerDefenseLastState.bin");
                            }
                            else
                            {
                                MessageBox.Show("No saved state exists. Please start a new game!");
                            }
                            break;
                        case 2:
                            MessageBox.Show("Highscore nem megvalósított");
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (model != null)
            {
                if (model.GameWidth != ActualWidth | model.GameHeight != ActualHeight)
                {
                    model.GameWidth = ActualWidth;
                    model.GameHeight = ActualHeight;
                    renderer = new MenuRenderer(model);
                }
            }
            if (renderer != null) drawingContext.DrawDrawing(renderer.BuildDrawing());
        }
    }
}
