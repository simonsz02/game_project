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
using TowerDefenseGame.Abstracts;
using TowerDefenseGame.GameItems;

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
        DispatcherTimer towerShotTimer;

        public TowerDefenseModel Model { get => model; set => model = value; }

        public TowerDefenseControl()
        {
            Loaded += TowerDefenseControl_Loaded;
        }
        private void TowerDefenseControl_Loaded(object sender, RoutedEventArgs e)
        {
            stw = new Stopwatch();
            model = model ?? new TowerDefenseModel(ActualWidth, ActualHeight);
            logic = new TowerDefenseLogic(model);
            renderer = new TowerDefenseRenderer(model);

            Window win = Window.GetWindow(this);
            if (win != null)
            {
                //Drive the game
                tickTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(model.baseTickSpeed)
                };
                tickTimer.Tick += TickTimer_Tick;
                tickTimer.Start();
                win.KeyDown += Win_KeyDown;
                MouseDown += TowerDefenseControl_MouseDown;
                //Spawn enemy
                spawnEnemyTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(model.baseTickSpeed*125)
                };
                spawnEnemyTimer.Tick += SpawnEnemyTimer_Tick;
                spawnEnemyTimer.Start();
                //Tower Shot
                towerShotTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(model.baseTickSpeed * 25)
                };
                //If there are any towers while initialization, their Timer_Tick method will be signed up
                foreach (Tower t in model.Towers)
                {
                    towerShotTimer.Tick += t.Timer_Tick;
                }
                towerShotTimer.Start();

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
            logic.SetTowerTargets(model.Enemies, model.Towers);
            logic.MoveProjectiles(model.Projectiles);
            InvalidateVisual();
        }
        private void TowerDefenseControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(this);
            if (e.ChangedButton == MouseButton.Left)
            {
                logic.AddTower(mousePos, towerShotTimer);
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
                    if (model.debug)
                    {
                        model.Enemies.Add(new Enemy(model.EntryPoint.X,
                                                    model.EntryPoint.Y + model.TileSize / 4,
                                                    model.TileSize / 2,
                                                    model.TileSize / 2,
                                                    20,
                                                    5,
                                                    logic.GetTilePos(model.EntryPoint),
                                                    5));
                        InvalidateVisual();
                    }
                    break;

                case Key.A:
                    if (model.debug)
                    {
                        model.Projectiles.Add(new Bullet( 0 - (model.TileSize / 4),
                                                          0 - (model.TileSize / 4),
                                                          model.TileSize / 4,
                                                          model.TileSize / 4,
                                                          8,
                                                          10));
                        InvalidateVisual();
                    }
                    break;

                case Key.D:
                    if (model.debug)
                    {
                        model.Projectiles.Add(new FrostBullet( model.EntryPoint.X,
                                                          model.EntryPoint.Y + model.TileSize / 8,
                                                          model.TileSize / 4,
                                                          model.TileSize / 4,
                                                          8,
                                                          10));
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