using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace TowerDefenseGame
{
    class TowerDefenseControl : FrameworkElement
    {
        TowerDefenseLogic logic;
        TowerDefenseRenderer renderer;
        TowerDefenseModel model;
        Stopwatch stw;
        DispatcherTimer tickTimer;
        DispatcherTimer spawnEnemyTimer;

        public TowerDefenseControl()
        {
            Loaded += TowerDefenseControl_Loaded;
        }
        private void TowerDefenseControl_Loaded(object sender, RoutedEventArgs e)
        {
            stw = new Stopwatch();
            model = new TowerDefenseModel(ActualWidth, ActualHeight);
            logic = new TowerDefenseLogic(model);
            renderer = new TowerDefenseRenderer(model);

            Window win = Window.GetWindow(this);
            if (win != null)
            {
                tickTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(40)
                };
                tickTimer.Tick += TickTimer_Tick;
                tickTimer.Start();
                win.KeyDown += Win_KeyDown;
                MouseDown += TowerDefenseControl_MouseDown;
                spawnEnemyTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(5000)
                };
                spawnEnemyTimer.Tick += SpawnEnemyTimer_Tick;
                spawnEnemyTimer.Start();
            }
            InvalidateVisual();
            stw.Start();
        }

        private void SpawnEnemyTimer_Tick(object sender, EventArgs e)
        {
            ///TODO create logic of spawning different enemies
            if (!model.debug)
            {
                model.Enemies.Add(new Enemy(model.EntryPoint.X,
                                            model.EntryPoint.Y + model.TileSize / 4,
                                            model.TileSize / 2,
                                            model.TileSize / 2,
                                            1,
                                            1,
                                            logic.GetTilePos(model.EntryPoint),
                                            5));
                InvalidateVisual();
            }
        }

        private void TickTimer_Tick(object sender, EventArgs e)
        {
            logic.MoveEnemies(model.Enemies);
            InvalidateVisual();
        }
        private void TowerDefenseControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(this);
            if (e.ChangedButton == MouseButton.Left)
            {
                //Point tilePos = logic.GetTilePos(mousePos);
                //MessageBox.Show(tilePos.ToString());
                logic.AddTower(mousePos);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                MessageBox.Show(mousePos.ToString());
            }
            InvalidateVisual();
        }        
        private void Win_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter: 
                    MessageBox.Show(logic.GetDistances()); 
                    if (!model.debug)
                    {
                        model.Enemies.Add(new Enemy(model.EntryPoint.X,
                                                    model.EntryPoint.Y + model.TileSize / 4,
                                                    model.TileSize / 2,
                                                    model.TileSize / 2,
                                                    1,
                                                    1,
                                                    logic.GetTilePos(model.EntryPoint),
                                                    5));
                        InvalidateVisual();
                    }
                    break;
            }
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (renderer != null) drawingContext.DrawDrawing(renderer.BuildDrawing());
        }
    }
}