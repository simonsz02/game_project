using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TowerDefenseGame.Model;
using TowerDefenseGame.Renderer;
using TowerDefenseGame.Repository;
using Microsoft.VisualBasic;
using System;

namespace TowerDefenseGame
{
    class MenuControl : FrameworkElement
    {
        MenuModel model;
        MenuRenderer renderer;
        Window win;
        string userName;
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
                            while (userName == null)
                            {
                                userName = Interaction.InputBox($"Dear player!" + Environment.NewLine + "Please insert your username that will be used during the game", "What's your name?", "", (int)model.GameWidth / 2, (int)model.GameHeight / 2);
                            };
                            win.Content = new TowerDefenseControl(userName);
                            break;
                        case 1:
                            while (userName == null)
                            {
                                userName = Interaction.InputBox($"Dear player!" + Environment.NewLine + "Please insert your username that will be used during the game", "What's your name?", "", (int)model.GameWidth / 2, (int)model.GameHeight / 2);
                            };
                            if (File.Exists("TowerDefenseLastState"+userName+".bin"))
                            {
                                win.Content = new TowerDefenseControl(userName);
                                ((TowerDefenseControl)win.Content).Model = SerializationAsBinary.Import<TowerDefenseModel>("TowerDefenseLastState" + userName + ".bin");
                            }
                            else
                            {
                                MessageBox.Show("No saved state exists. Please start a new game!");
                            }
                            break;
                        case 2:
                            renderer.showHighScoreList = !renderer.showHighScoreList;
                            InvalidateVisual();
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
