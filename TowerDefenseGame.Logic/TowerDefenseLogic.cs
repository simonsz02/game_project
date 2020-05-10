﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TowerDefenseGame.Model;
using TowerDefenseGame.Model.Abstracts;
using TowerDefenseGame.Model.GameItems;

namespace TowerDefenseGame.Logic
{
    [Serializable]
    public class TowerDefenseLogic
    {
        public static Random rnd = new Random();

        public bool debug = false;
        public int baseTickSpeed = 40;
        #region finishing the game
        public int enemyCounter = 0;
        public int playerHealth;
        [NonSerialized]
        public Action finishGame;
        #endregion
        List<Enemy> deleteEnemies = new List<Enemy>();

        TowerDefenseModel model;
        private string userName;

        public TowerDefenseLogic(TowerDefenseModel model)
        {
            this.model = model;
            InitModel();
        }
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
        private void InitModel()
        {
            int width = 15;
            int height = 9;
            model.Path = new bool[width, height];
            SetPath(model.Path);
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

        public int SpawnNewEnemy(Action raiseSpawnSpeed)
        {
            int res = 0;
            double hp = rnd.Next(50, 100);
            model.Enemies.Add(new Enemy(model.EntryPoint.X,
                                        model.EntryPoint.Y,
                                        model.TileSize / 2,
                                        model.TileSize / 2,
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
                                            model.TileSize / 3 * 2,
                                            model.TileSize / 3 * 2,
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
        public void AddTower(Point mousePos, System.Windows.Threading.DispatcherTimer timer, DamageType damageType = DamageType.physical)
        {

                RocketTower tempTower = new RocketTower(GetTilePos(mousePos).X * model.TileSize,
                                           GetTilePos(mousePos).Y * model.TileSize,
                                           model.TileSize,
                                           model.TileSize,
                                           (x, y, w, h, m, d, dt, t) => model.Projectiles.Add(new Missile(x, y, w / 4, h / 4, m, d, dt, t)),
                                           timer,
                                           damageType
                                           );

                model.Towers.Add(tempTower);

                model.Coins -= tempTower.Price;

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
        private void SetPath(bool[,] path)
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