





using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerDefenseGame.Abstracts;
using TowerDefenseGame.GameItems;

namespace TowerDefenseGame
{
    class TowerDefenseLogic
    {
        TowerDefenseModel model;

        public TowerDefenseLogic(TowerDefenseModel model)
        {
            this.model = model;
            InitModel();
        }
        public TowerDefenseLogic(TowerDefenseModel model, string fname)
        {
            this.model = model;
            InitModel(fname);
        }
        /// <summary>
        /// A -100-as paraméter ara szolgál, hogy lefoglaljunk 
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
            model.TileSize = Math.Min(Math.Min((model.GameWidth*0.95) / width, (model.GameWidth - 100) / width), model.GameHeight / height);
            for (int i = 0; i < height; i++)
            {
                if (model.Path[0, i])
                {
                    model.ExitPoint = new Point(0, i * model.TileSize);
                }
                if (model.Path[width-1, i])
                {
                    model.EntryPoint = new Point(width * model.TileSize, i * model.TileSize);
                }
            }
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (model.Path[x,y])
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
        /// TODO fileLoad
        /// This is only a demo version
        /// </summary>
        /// <param name="enemyList"></param>
        public void MoveEnemies(List<Enemy> enemyList)
        {
            foreach (MovingGameItem enemy in enemyList)
            {
                if (enemy.Destination != null)
                {
                    Point destCoords = GetPosTile(enemy.Destination);
                    double x = enemy.Area.X + Math.Sign(destCoords.X - enemy.Area.X) * 
                                 Math.Min(Math.Abs(destCoords.X - enemy.Area.X), 
                                          (double)enemy.Movement);
                    double y = enemy.Area.Y + Math.Sign(destCoords.Y - enemy.Area.Y) *
                                 Math.Min(Math.Abs(destCoords.Y - enemy.Area.Y),
                                          (double)enemy.Movement);
                    if ( destCoords.X == x &&
                         destCoords.Y == y )
                    {
                        SetNewDestionation(enemy);                                 
                    }
                    enemy.SetXY(x, y);
                }
                else
                {
                    enemy.Destination = GetTilePos(model.EntryPoint);
                }
            }
        }

        public void MoveProjectiles(List<Projectile> projList)
        {
            Point destCoords = new Point();
            List<Projectile> delete = new List<Projectile>();
            foreach (Projectile p in projList)
            {
                if (p.Target != null)
                {
                    destCoords = p.Target.Area.TopLeft;
                    Vector v = Point.Subtract(destCoords, p.Area.TopLeft);
                    p.Location = Point.Add(p.Area.TopLeft,Math.Min(v.Length, (double)p.Movement) * v / v.Length);
                    if (destCoords==p.Area.TopLeft)
                    {
                        MessageBox.Show("Találat!");
                        delete.Add(p);
                        model.Enemies.Remove(p.Target);
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
                                //MessageBox.Show("New Destionation: " + (x + i) + "-" + (y + j));
                                enemy.Origin = enemy.Destination;
                                if (x==1)
                                {
                                    enemy.Destination = new Point(x + i -1, y + j);
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
        /// performancia tesztelés céljából van itt
        /// </summary>
        /// <returns></returns>
        internal string GetDistances()
        {
            Point p = GetPosTile(new Point(7, 0));
            Enemy target = null;
            double minDis = double.MaxValue;
            foreach (Enemy tar in model.Enemies)
            {
                double distanceSquared = (new Point(tar.Area.X, tar.Area.Y) - p).LengthSquared;
                if (minDis > distanceSquared)
                {
                    minDis = distanceSquared;
                    target = tar;
                }
            }
            if (target!=null)
            {
                Point res = GetTilePos(new Point(target.Area.X, target.Area.Y));
                return $"Nearest enemy to tile [7:0] is on tile: [{res.X}:{res.Y}]";
            }
            return $"No enemy on the field";
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

        private void InitModel(string fname)
        {
            /*
                     private void InitModel(string fname)
                    {
                        Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fname);
                        StreamReader sr = new StreamReader(stream);
                        string[] lines = sr.ReadToEnd().Replace("\r", "").Split('\n');

                        int width = int.Parse(lines[0]);
                        int height = int.Parse(lines[1]);
                        model.Walls = new bool[width, height];
                        model.TileSize = Math.Min(model.GameWidth/width, model.GameHeight/height);
                        for (int x=0; x<width; x++)
                        {
                            for (int y = 0; y < height; y++)
                            {
                                char current = lines[y+2][x];
                                model.Walls[x, y] = (current == 'e');
                                if (current == 'S') model.Player = new Point(x, y);
                                if (current == 'F') model.Exit = new Point(x, y);
                            }
                        }

                    }  
            */
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
        /// Gets top left quarter point of Tile
        /// </summary>
        /// <param name="tile"></param>
        /// <returns>Point</returns>
        public Point GetPosTile(Point tile)
        {
            return new Point(tile.X * model.TileSize + model.TileSize / 4,
                              tile.Y * model.TileSize + model.TileSize / 4);
        }
        public void AddTower(Point mousePos)
        {
            if (model.Path[(int)GetTilePos(mousePos).X, (int)GetTilePos(mousePos).Y] == false)
            {
                model.Towers.Add(new Tower(GetTilePos(mousePos).X * model.TileSize, GetTilePos(mousePos).Y * model.TileSize,
                                    model.TileSize, model.TileSize));
            }
            else
            {
                MessageBox.Show("Az útra nem lehet tornyot elhelyezni");
            }

        }
    }
}        