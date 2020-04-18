using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TowerDefenseGame
{
    class TowerDefenseRenderer
    {
        TowerDefenseModel model;

        Drawing oldBackground;
        Drawing oldFields;
        Drawing oldPath;
        Drawing oldTowers;

        public TowerDefenseRenderer(TowerDefenseModel model)
        {
            this.model = model;
        }
        public Drawing BuildDrawing()
        {
            DrawingGroup dg = new DrawingGroup();
            dg.Children.Add(GetBackground());
            dg.Children.Add(GetFields());
            dg.Children.Add(GetPath());
            dg.Children.Add(GetTowers());
            return dg;
        }

        private Drawing GetBackground()
        {
            if (oldBackground == null)
            {
                Geometry backgroundGeometry = new RectangleGeometry(new Rect(0, 0, model.GameWidth, model.GameHeight));
                oldBackground = new GeometryDrawing(Brushes.Black, null, backgroundGeometry);
            }
            return oldBackground;
        }

        private Drawing GetFields()
        {
            if (oldFields == null)
            {
                GeometryGroup g = new GeometryGroup();
                GeometryGroup p = new GeometryGroup();
                for (int x = 0; x < model.Fields.GetLength(0); x++)
                {
                    for (int y = 0; y < model.Fields.GetLength(1); y++)
                    {
                        if (model.Fields[x, y])
                        {
                            Geometry box = new RectangleGeometry(new Rect(x * model.TileSize, y * model.TileSize, model.TileSize, model.TileSize));
                            g.Children.Add(box);
                        }
                        else
                        {
                            Geometry box = new RectangleGeometry(new Rect(x * model.TileSize, y * model.TileSize, model.TileSize, model.TileSize));
                            p.Children.Add(box);
                        }
                    }
                }
                oldFields = new GeometryDrawing(Brushes.LightGreen, new Pen(Brushes.DarkGray, 1), g);
                oldPath = new GeometryDrawing(Brushes.BurlyWood, null, p);
            }
            return oldFields;
        }
        private Drawing GetPath()
        {
            return oldPath;
        }

        private Drawing GetTowers()
        {

            GeometryGroup g = new GeometryGroup();

            foreach (Tower tower in model.Towers)
            {
                Geometry towerGeo = new EllipseGeometry(new Rect(tower.Area.X, tower.Area.Y, model.TileSize, model.TileSize));
                g.Children.Add(towerGeo);
            }

            oldTowers = new GeometryDrawing(Brushes.Brown, new Pen(Brushes.Black, 1), g);

            return oldTowers;
        }
    }
}