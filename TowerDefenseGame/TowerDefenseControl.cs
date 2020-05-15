using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using TowerDefenseGame.Model;
using TowerDefenseGame.Logic;
using TowerDefenseGame.Renderer;
using TowerDefenseGame.Model.GameItems;
using System.Media;
using TowerDefenseGame.Repository;

namespace TowerDefenseGame
{
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
        string userName;
        private bool gameEnd = false;

        public TowerDefenseModel Model { get => model; set => model = value; }

        public TowerDefenseControl(string userName)
        {
            this.userName = userName;
            Loaded += TowerDefenseControl_Loaded;
        }
        private void TowerDefenseControl_Loaded(object sender, RoutedEventArgs e)
        {
            stw = new Stopwatch();
            model = model ?? new TowerDefenseModel(ActualWidth, ActualHeight, 1500);
            logic = new TowerDefenseLogic(model, userName)
            {
                playerHealth = 10,
                finishGame = FinishGame
            };
            renderer = new TowerDefenseRenderer(model);

            Window win = Window.GetWindow(this);
            if (win != null)
            {
                tickTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(logic.baseTickSpeed)
                };
                tickTimer.Tick += TickTimer_Tick;
                tickTimer.Start();
                win.KeyDown += Win_KeyDown;
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

        private void FinishGame()
        {
            if (logic.playerHealth <= 0)
            {
                gameEnd = true;
                tickTimer.Stop();
                spawnEnemyTimer.Stop();
                towerShotTimer.Stop();
                //renderer.showLostAnimation();
                HighScoreHandler.AddRowToHighScoreFile(new HighScoreHandler.Row(userName, model.Coins));
                MessageBox.Show("The horde has won"+Environment.NewLine+"Bad news for you, 'cause you have lost!");
                Window.GetWindow(this).Content = new MenuControl();
            }
            else if (logic.enemyCounter >= 100)
            {
                gameEnd = true;
                tickTimer.Stop();
                spawnEnemyTimer.Stop();
                towerShotTimer.Stop();
                //renderer.showWinAnimation();
                HighScoreHandler.AddRowToHighScoreFile(new HighScoreHandler.Row(userName, model.Coins));
                MessageBox.Show("That was awesome! You have won!" + Environment.NewLine + "Check the highscore list, if you were good enough!");
                Window.GetWindow(this).Content = new MenuControl();
            }
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

            if (TowerActionIsFailed) MessageBox.Show("Tower operation has failed");

            InvalidateVisual();

        }        
        private void Win_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter: 
                    break;
                case Key.A:
                    break;
                case Key.D:        
                    break;
                case Key.P:
                    if (!gameEnd)
                    {
                        tickTimer.IsEnabled = !tickTimer.IsEnabled;
                        spawnEnemyTimer.IsEnabled = !spawnEnemyTimer.IsEnabled;
                        towerShotTimer.IsEnabled = !towerShotTimer.IsEnabled;
                    }
                    break;
                case Key.D1:
                    logic.GetSelectedTower().Selected = false;
                    model.TowerSelectorRects[0].Selected = true;
                    break;
                case Key.D2:
                    logic.GetSelectedTower().Selected = false;
                    model.TowerSelectorRects[1].Selected = true;
                    break;
                case Key.D3:
                    logic.GetSelectedTower().Selected = false;
                    model.TowerSelectorRects[2].Selected = true;
                    break;
                case Key.D4:
                    logic.GetSelectedTower().Selected = false;
                    model.TowerSelectorRects[3].Selected = true;
                    break;
                case Key.D5:
                    logic.GetSelectedTower().Selected = false;
                    model.TowerSelectorRects[4].Selected = true;
                    break;
                case Key.D6:
                    logic.GetSelectedTower().Selected = false;
                    model.TowerSelectorRects[5].Selected = true;
                    break;
            }
        }
        internal void SaveState()
        {
            SerializationAsBinary.Export("TowerDefenseLastState" + userName + ".bin", model);
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (renderer != null) drawingContext.DrawDrawing(renderer.BuildDrawing());
        }
    }
}