using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

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
                    model.EntryPoint = new Point(0, i * model.TileSize);
                }
                if (model.Path[width-1, i])
                {
                    model.ExitPoint = new Point(width * model.TileSize, i * model.TileSize);
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
        public void MoveEnemy(List<IEnemy> enemyList)
        {
            foreach (IEnemy item in enemyList)
            {

            }
        }

        public void AddTower(double x, double y)
        {

            model.Towers.Add(new Tower(x,y,model.TileSize, model.TileSize));
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
        public Point GetTilePos(Point mousePos) // Pixel position => Tile position
        {
            return new Point((int)(mousePos.X / model.TileSize),
                            (int)(mousePos.Y / model.TileSize));
        }
    }
}
