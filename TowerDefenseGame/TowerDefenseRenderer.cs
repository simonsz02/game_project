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

        public TowerDefenseRenderer(TowerDefenseModel model)
        {
            this.model = model;
        }
        public Drawing BuildDrawing()
        {
            DrawingGroup dg = new DrawingGroup();
            dg.Children.Add(GetBackground());
            dg.Children.Add(GetFields());
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
                for (int x = 0; x < model.Fields.GetLength(0); x++)
                {
                    for (int y = 0; y < model.Fields.GetLength(1); y++)
                    {
                        if (model.Fields[x, y])
                        {
                            Geometry box = new RectangleGeometry(new Rect(x * model.TileSize, y * model.TileSize, model.TileSize, model.TileSize));
                            g.Children.Add(box);
                        }
                    }
                }
                oldFields = new GeometryDrawing(Brushes.LightGreen, null, g);
            }
            return oldFields;
        }
    }
}
