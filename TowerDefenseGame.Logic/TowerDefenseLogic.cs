using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using TowerDefenseGame.Model;
using TowerDefenseGame.Model.Abstracts;
using TowerDefenseGame.Model.GameItems;
using TowerDefenseGame.Repository;

namespace TowerDefenseGame.Logic
{
    /// <summary>
    /// Contains the logic that drives the game
    /// </summary>
    [Serializable]
    public class TowerDefenseLogic
    {
        /// <summary>
        /// Random number generator
        /// </summary>
        public static Random rnd = new Random();

        /// <summary>
        /// Debug mode flag
        /// </summary>
        public bool debug = false;

        /// <summary>
        /// Base tick speed of the enemy is 40
        /// </summary>
        public int baseTickSpeed = 40;
        #region finishing the game
        /// <summary>
        /// Counts spawned enemies
        /// </summary>
        public int enemyCounter = 0;
        /// <summary>
        /// Counts players actual health
        /// </summary>
        public int playerHealth;
        /// <summary>
        /// Action that finishes the game, saves player score and loads the menu
        /// </summary>
        [NonSerialized]
        public Action finishGame;
        #endregion
        List<Enemy> deleteEnemies = new List<Enemy>();

        TowerDefenseModel model;
        private string userName;

        /// <summary>
        /// Constructor of the logic class
        /// </summary>
        /// <param name="model"></param>
        /// <param name="staticPath">for testing purposes</param>
        public TowerDefenseLogic(TowerDefenseModel model, bool staticPath = false)
        {
            this.model = model;
            InitModel(staticPath);
        }

        /// <summary>
        /// Constructor of the logic class
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName">Name of the player</param>
        public TowerDefenseLogic(TowerDefenseModel model, string userName)
        {
            this.model = model;
            this.userName = userName;
            InitModel();
        }
        /// <summary>
        /// A -100-as paraméter arra szolgál, hogy lefoglaljunk 
        /// egy minimum 100, maximum a szélesség 5%-a pixel 
        /// széles sávot oldalt a menünek!
        /// Nilván szebb lenne paraméterbe tenni
        /// </summary>
        /// <param name="staticPath">for testing purposes</param>
        private void InitModel(bool staticPath = false)
        {
            int width = 15;
            int height = 9;
            model.Path = new bool[width, height];
            if (!staticPath)
            {
                SetPath(model.Path);
            }
            else
            {
                SetPathStatic(model.Path);
            }
            model.Fields = new bool[width, height];

            model.TileSize = Math.Min(Math.Min((model.GameWidth * 0.95) / width, (model.GameWidth - 100) / width), model.GameHeight / height);
            for (int i = 0; i < height; i++)
            {
                if (model.Path[0, i])
                {
                    model.ExitPoint = new Point(0, i * model.TileSize);
                }
                if (model.Path[width - 1, i])
                {
                    model.EntryPoint = new Point(width * model.TileSize, i * model.TileSize);
                }
            }
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (model.Path[x, y])
                    {
                        model.Fields[x, y] = false;
                    }
                    else
                    {
                        model.Fields[x, y] = true;
                    }
                }
            }

            for (int i = 0; i < model.TowerSelectorRects.Length; i++)
            {
                model.TowerSelectorRects[i] = new TowerSelectorRect(model.TileSize * width + 15,
                                                                        i* model.GameHeight / model.TowerSelectorRects.Length,
                                                                        model.GameHeight / model.TowerSelectorRects.Length * 0.3,
                                                                        model.GameHeight/ model.TowerSelectorRects.Length * 0.3,
                                                                        i);
                if (i==0)
                {
                    model.TowerSelectorRects[i].Selected = true;
                }
            }

        }

        /// <summary>
        /// Move all enemies on the path 
        /// </summary>
        /// <param name="enemyList"></param>
        public void MoveEnemies(List<Enemy> enemyList)
        {
            foreach (MovingGameItem enemy in enemyList)
            {
                if (enemy.Destination != null)
                {
                    Point destCoords = GetPosTileCentre(enemy.Destination);
                    double x = enemy.Centre.X + Math.Sign(destCoords.X - enemy.Centre.X) *
                                 Math.Min(Math.Abs(destCoords.X - enemy.Centre.X),
                                          (double)enemy.Movement);
                    double y = enemy.Centre.Y + Math.Sign(destCoords.Y - enemy.Centre.Y) *
                                 Math.Min(Math.Abs(destCoords.Y - enemy.Centre.Y),
                                          (double)enemy.Movement);
                    if (destCoords.X == x &&
                         destCoords.Y == y)
                    {
                        SetNewDestionation(enemy);
                    }
                    enemy.Centre = new Point(x, y);
                }
                else
                {
                    enemy.Destination = GetTilePos(model.EntryPoint);
                }
            }
            foreach (Enemy e in deleteEnemies)
            {
                model.Enemies.Remove(e);
            }
            deleteEnemies = new List<Enemy>();
        }
        /// <summary>
        /// Puts new enemy on the gamefield
        /// </summary>
        /// <param name="raiseSpawnSpeed">An action that shortens the speeds up spawntimer</param>
        /// <returns></returns>
        public int SpawnNewEnemy(Action raiseSpawnSpeed)
        {
            int res = 0;
            double hp = rnd.Next(50, 100);
            model.Enemies.Add(new Enemy(model.EntryPoint.X,
                                        model.EntryPoint.Y,
                                        model.TileSize * 0.6,
                                        model.TileSize * 0.6,
                                        hp,
                                        rnd.Next(2,10),
                                        GetTilePos(model.EntryPoint),
                                        rnd.Next(3, 7)));
            enemyCounter++;
            if ((hp%19)==0)
            {
                Thread.Sleep(baseTickSpeed*20);
                model.Enemies.Add(new Enemy(model.EntryPoint.X,
                                            model.EntryPoint.Y,
                                            model.TileSize * 0.9,
                                            model.TileSize * 0.9,
                                            hp*3,
                                            rnd.Next(10, 25),
                                            GetTilePos(model.EntryPoint),
                                            rnd.Next(3, 4)));
                enemyCounter++;
                res = 1;
            }
            if (enemyCounter%10==0)
            {
                raiseSpawnSpeed();
            }
            return res;
        }

        /// <summary>
        /// Move all existing projectiles
        /// </summary>
        /// <param name="projList"></param>
        public void MoveProjectiles(List<Projectile> projList)
        {
            Point destCoords = new Point();
            List<Projectile> delete = new List<Projectile>();
            foreach (Projectile p in projList)
            {
                if (p.Target != null)
                {
                    if (p.Target.Health <= 0)
                    {
                        delete.Add(p);
                    }
                    destCoords = p.Target.Centre;
                    Vector v = Point.Subtract(destCoords, p.Centre);
                    p.Centre = Point.Add(p.Centre, Math.Min(v.Length, p.Movement) * v / v.Length);
                    if (destCoords == p.Centre)
                    {
                        if (debug)
                        {
                            //MessageBox.Show("Találat!");
                        }
                        delete.Add(p);
                        if (!p.CauseDamage(p.Target, 
                                           (Enemy e) => {
                                               deleteEnemies.Add(e);
                                               model.Coins += p.Target.Reward;
                                           }, 
                                           p.TypeOfDamage))
                        {
                            model.Enemies.Remove(p.Target);
                            model.Coins += p.Target.Reward;
                        }
                    }
                }
                else
                {
                    p.GetTarget(model.Enemies);
                }
            }
            /// Remove projectiles that hit their target
            foreach (Projectile del in delete)
            {
                projList.Remove(del);
            }
        }
        /// <summary>
        /// Set and returns a new destionation for the enemy on a tile based coordinate
        /// </summary>
        /// <param name="enemy">Object thats destination has to be changed</param>
        /// <returns>Tile type new destination of the enemy</returns>
        private Point SetNewDestionation(MovingGameItem enemy)
        {
            Point pos = GetTilePos(new Point(enemy.Area.X, enemy.Area.Y));
            int x = (int)pos.X;
            int y = (int)pos.Y;
            bool found = false;
            if (enemy.Area.Right < 0)
            {
                deleteEnemies.Add((Enemy)enemy);
                //sebződik a CASTLE
                playerHealth--;
                if (playerHealth<=0 || enemyCounter >= 110)
                {
                    finishGame();
                }

            }
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (!found)
                    {
                        if (x + i >= 0 && y + j >= 0 && x + i < model.Path.GetLength(0) && y + j < model.Path.GetLength(1) &&
                            Math.Abs(i + j) == 1 && (i != 0 || j != 0) &&
                            (x + i != enemy.Origin.X || y + j != enemy.Origin.Y))
                        {
                            if (model.Path[x + i, y + j])
                            {
                                found = true;
                                enemy.Origin = enemy.Destination;
                                if (x == 1)
                                {
                                    // az utolsó csempéről lesétál az ellen a pályáról
                                    enemy.Destination = new Point(x + i - 1, y + j);
                                }
                                else
                                {
                                    enemy.Destination = new Point(x + i, y + j);
                                }
                            }
                        }
                    }
                }
            }
            return enemy.Destination;
        }

        /// <summary>
        /// Tower shoots an enemy untill it leaves the range of the tower or dies
        /// </summary>
        /// <param name="enemyList"></param>
        /// <param name="towerList"></param>
        public void SetTowerTargets(List<Enemy> enemyList, List<Tower> towerList)
        {
            foreach (Tower tow in towerList)
            {
                if (tow.Target == null)
                {
                    double minDis = double.MaxValue;
                    foreach (Enemy tar in enemyList)
                    {
                        double distanceSquared = (tar.Centre - tow.Centre).LengthSquared;
                        if (minDis > distanceSquared)
                        {
                            minDis = distanceSquared;
                            tow.Target = tar;
                        }
                    }
                }
                else if (tow.Target.Health <= 0 | (tow.Centre - tow.Target.Centre).Length > tow.Range)
                {
                    double minDis = double.MaxValue;
                    foreach (Enemy tar in model.Enemies)
                    {
                        double distanceSquared = (tar.Centre - tow.Centre).LengthSquared;
                        if (minDis > distanceSquared)
                        {
                            minDis = distanceSquared;
                            tow.Target = tar;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Player add or upgrade an existing tower
        /// </summary>
        /// <param name="mousePos"></param>
        /// <param name="timer"></param>
        /// <returns></returns>
        public bool AddOrUpgradeTower(Point mousePos, System.Windows.Threading.DispatcherTimer timer)
        {
            bool OperationHasFailed = false;
            Tower choosenTower = null;

            if (model.Towers.Count != 0)
                choosenTower = ExistsTower(mousePos);

            if (choosenTower==null)
            {
                OperationHasFailed = AddTower(mousePos, timer);
            }
            else
            {
                OperationHasFailed = UpgradeTower(choosenTower);
            }

            return OperationHasFailed;
        }

        /// <summary>
        /// Creates new tower on given position and an own timer
        /// </summary>
        /// <param name="mousePos">position of the new tower</param>
        /// <param name="timer">shot timer</param>
        /// <returns>returns true if placement happend</returns>
        private bool AddTower(Point mousePos, System.Windows.Threading.DispatcherTimer timer)
        {
            bool OperationHasFailed = false;

            if (model.Path[(int)GetTilePos(mousePos).X, (int)GetTilePos(mousePos).Y] == false &&
                model.Towers.Count < 6 &&
                (model.Coins - GetSelectedTower().Price) >= 0)
            {
                RocketTower tempTower = new RocketTower(GetTilePos(mousePos).X * model.TileSize,
                           GetTilePos(mousePos).Y * model.TileSize,
                           model.TileSize,
                           model.TileSize,
                           (x, y, w, h, m, d, dt, t) => model.Projectiles.Add(new Missile(x, y, w / 4, h / 4, m, d, dt, t)),
                           timer,
                           GetSelectedTower().damageType,
                           GetSelectedTower().Price
                           );

                model.Towers.Add(tempTower);

                model.Coins -= tempTower.Price;
            }
            else
                OperationHasFailed = true;

            return OperationHasFailed;
        }
        /// <summary>
        /// Upgrades tower if possible
        /// </summary>
        /// <param name="choosenTower">The entity that needs to be upgraded</param>
        /// <returns></returns>
        private bool UpgradeTower(Tower choosenTower)
        {
            bool OperationHasFailed = false;

            if (choosenTower.Grade < 3 &&
                     (model.Coins - (int)(Math.Pow(2, choosenTower.Grade) * choosenTower.Price)) >= 0)
            {
                model.Coins -= (int)(Math.Pow(2, choosenTower.Grade) * choosenTower.Price);
                choosenTower.Grade++;
            }
            else
                OperationHasFailed = true;

            return OperationHasFailed;
        }

        /// <summary>
        /// Removes an existing tower
        /// </summary>
        /// <param name="mousePos"></param>
        /// <returns></returns>
        public bool RemoveTower(Point mousePos)
        {
            bool OperationIsFailed = false;
            Tower choosenTower = null;

            if (model.Towers.Count != 0)
                choosenTower = ExistsTower(mousePos);

            if (choosenTower != null)
                model.Towers.Remove(choosenTower);
            else
                OperationIsFailed = true;

            return OperationIsFailed;
        }

        /// <summary>
        /// Returns a tower entity if there is one on the specific tile
        /// </summary>
        /// <param name="mousePos">the specific point</param>
        /// <returns>Tower Entity</returns>
        private Tower ExistsTower(Point mousePos)
        {
            Tower founded = null;

            foreach (Tower t in model.Towers)
            {
                if (t.Area.X == GetTilePos(mousePos).X * model.TileSize &&
                    t.Area.Y == GetTilePos(mousePos).Y * model.TileSize)
                {
                    founded = t;
                }
            }
            return founded;
        }

        /// <summary>
        /// Frames the choosen tower selector image
        /// </summary>
        /// <param name="mousePos"></param>
        public void Framing(Point mousePos)
        {
            TowerSelectorRect preSelected = null;
            TowerSelectorRect newlySelected = null;
            bool newImageWasSelected = false;

            foreach (TowerSelectorRect selector in model.TowerSelectorRects)
            {
                if (selector.Selected)
                {
                    preSelected = selector;
                }

                if (selector.Area.Contains(mousePos))
                {
                    newImageWasSelected = true;
                    newlySelected = selector;
                }
            }

            if (newImageWasSelected)
            {
                preSelected.Selected = false;
                newlySelected.Selected = true;
            }
        }

        /// <summary>
        /// Gives back the choosen tower type
        /// </summary>
        /// <returns></returns>
        public TowerSelectorRect GetSelectedTower()
        {
            int selectedNum = 0;

            while (!model.TowerSelectorRects[selectedNum].Selected)
            {
                selectedNum++;
            }

            return model.TowerSelectorRects[selectedNum];
        }

        /// <summary>
        /// Converts pixel to tile coordinates
        /// </summary>
        /// <param name="mousePos">Pixel coordinates</param>
        /// <returns>Tile</returns>
        public Point GetTilePos(Point mousePos)
        {
            return new Point((int)(mousePos.X / model.TileSize),
                            (int)(mousePos.Y / model.TileSize));
        }
        /// <summary>
        /// Gets top left quarter point of tile
        /// </summary>
        /// <param name="tile"></param>
        /// <returns>Point</returns>
        public Point GetPosTile(Point tile)
        {
            return new Point(tile.X * model.TileSize + model.TileSize / 4,
                              tile.Y * model.TileSize + model.TileSize / 4);
        }
        /// <summary>
        /// Gets center coordinates of tile
        /// </summary>
        /// <param name="tile"></param>
        /// <returns>Point</returns>
        public Point GetPosTileCentre(Point tile)
        {
            return new Point(tile.X * model.TileSize + model.TileSize / 2,
                              tile.Y * model.TileSize + model.TileSize / 2);
        }
        /// <summary>
        /// Sets the path based on the datacontainer of the repository
        /// </summary>
        /// <param name="path">matrix representing the path</param>
        private void SetPath(bool[,] path)
        {
            foreach (Point p in PathLoader.ReadPathFile())
            {
                path[(int)p.X, (int)p.Y] = true;
            }
        }
        /// <summary>
        /// Sets the path based on static data in the function
        /// Built for testing purposes
        /// </summary>
        /// <param name="path">matrix representing the path</param>
        private void SetPathStatic(bool[,] path)
        {
            path[0, 4] = true;
            path[1, 4] = true;
            path[2, 4] = true;
            path[2, 5] = true;
            path[2, 6] = true;
            path[3, 6] = true;
            path[4, 6] = true;
            path[5, 6] = true;
            path[5, 5] = true;
            path[5, 4] = true;
            path[5, 3] = true;
            path[5, 2] = true;
            path[5, 1] = true;
            path[6, 1] = true;
            path[7, 1] = true;
            path[8, 1] = true;
            path[9, 1] = true;
            path[10, 1] = true;
            path[11, 1] = true;
            path[11, 2] = true;
            path[11, 3] = true;
            path[11, 4] = true;
            path[11, 5] = true;
            path[11, 6] = true;
            path[11, 7] = true;
            path[12, 7] = true;
            path[13, 7] = true;
            path[13, 6] = true;
            path[13, 5] = true;
            path[13, 4] = true;
            path[14, 4] = true;
        }
    }
}