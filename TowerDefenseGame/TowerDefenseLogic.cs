using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        /// egy minimum 100 pixel széles sávot oldalt a menünek
        /// Nilván szebb lenne paraméterbe tenni
        /// </summary>
        private void InitModel()
        {
            int width = 30;
            int height = 10;
            model.Fields = new bool[width, height];
            model.TileSize = Math.Min((model.GameWidth-100) / width, model.GameHeight / height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    model.Fields[x, y] = true;
                }
            }
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
