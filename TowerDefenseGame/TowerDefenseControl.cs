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
using TowerDefenseGame.Model;
using TowerDefenseGame.Logic;
using TowerDefenseGame.Renderer;
using TowerDefenseGame.Model.Abstracts;
using TowerDefenseGame.Model.GameItems;
using System.Media;

namespace TowerDefenseGame
{
    [Serializable]
    public class TowerDefenseControl : FrameworkElement
    {
        TowerDefenseLogic logic;
        TowerDefenseRenderer renderer;
        TowerDefenseModel model;
        Stopwatch stw;
        DispatcherTimer tickTimer;
        DispatcherTimer spawnEnemyTimer;
        DispatcherTimer towerShotTimer;
        //private DamageType choosenDamageType;
        SoundPlayer pl = new SoundPlayer();

        public TowerDefenseModel Model { get => model; set => model = value; }

        public TowerDefenseControl()
        {
            Loaded += TowerDefenseControl_Loaded;
        }
        private void TowerDefenseControl_Loaded(object sender, RoutedEventArgs e)
        {
            stw = new Stopwatch();
            model = model ?? new TowerDefenseModel(ActualWidth, ActualHeight, 1500);
            logic = new TowerDefenseLogic(model);
            renderer = new TowerDefenseRenderer(model);

            Window win = Window.GetWindow(this);
            if (win != null)
            {
                //Drive the game
                tickTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(logic.baseTickSpeed)
                };
                tickTimer.Tick += TickTimer_Tick;
                tickTimer.Start();
                //win.KeyDown += Win_KeyDown;
                MouseDown += TowerDefenseControl_MouseDown;
                //Spawn enemy
                spawnEnemyTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(logic.baseTickSpeed*125)
                };
                spawnEnemyTimer.Tick += SpawnEnemyTimer_Tick;
                spawnEnemyTimer.Start();
                //Tower Shot
                towerShotTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(logic.baseTickSpeed * 25)
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
            if (!logic.debug)
            {
                if (logic.SpawnNewEnemy(() => { spawnEnemyTimer.Interval = TimeSpan.FromMilliseconds((logic.baseTickSpeed * 125) - 50); } ) == 1)
                {
                    pl.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\Media\\BigArrival.wav";
                    pl.Play();
                }
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
            bool TowerActionIsFailed = false;

            Point mousePos = e.GetPosition(this);

            if(mousePos.X <= model.TileSize * model.Path.GetLength(0))
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    TowerActionIsFailed = logic.AddOrUpgradeTower(mousePos, towerShotTimer);
                }
                else
                {
                    TowerActionIsFailed = logic.RemoveTower(mousePos);
                }
            }
            else
            {
                logic.Framing(mousePos);
            }

            if (TowerActionIsFailed) MessageBox.Show("Tower operation is failed");

            InvalidateVisual();

        }        
        //private void Win_KeyDown(object sender, KeyEventArgs e)
        //{
        //    switch (e.Key)
        //    {
        //        case Key.Enter: 
        //            if (logic.debug)
        //            {
        //                model.Enemies.Add(new Enemy(model.EntryPoint.X,
        //                                            model.EntryPoint.Y + model.TileSize / 4,
        //                                            model.TileSize / 2,
        //                                            model.TileSize / 2,
        //                                            20,
        //                                            5,
        //                                            logic.GetTilePos(model.EntryPoint),
        //                                            5));
        //                InvalidateVisual();
        //            }
        //            break;
        //        case Key.A:
        //            if (logic.debug)
        //            {
        //                model.Projectiles.Add(new Missile(0, 0, model.TileSize / 4, model.TileSize / 4, 8, 10, DamageType.physical, model.Enemies.First()));
        //            }
        //            break;
        //        case Key.D:
        //            if (logic.debug)
        //            {
        //            }                
        //            break;
        //        case Key.P:
        //            tickTimer.IsEnabled = !tickTimer.IsEnabled;
        //            spawnEnemyTimer.IsEnabled = !spawnEnemyTimer.IsEnabled;
        //            towerShotTimer.IsEnabled = !towerShotTimer.IsEnabled;
        //            break;
        //        case Key.D0:
        //            choosenDamageType = DamageType.physical;
        //            break;
        //        case Key.D1:
        //            choosenDamageType = DamageType.poison;
        //            break;
        //        case Key.D2:
        //            choosenDamageType = DamageType.fire;
        //            break;
        //        case Key.D3:
        //            choosenDamageType = DamageType.frost;
        //            break;
        //        case Key.D4:
        //            choosenDamageType = DamageType.air;
        //            break;
        //        case Key.D5:
        //            choosenDamageType = DamageType.earth;
        //            break;

        //    }
        //}

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (renderer != null) drawingContext.DrawDrawing(renderer.BuildDrawing());
        }
    }
}