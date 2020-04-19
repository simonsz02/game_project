using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TowerDefenseGame.Abstracts;
using TowerDefenseGame.GameItems;

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
            AddEnemiesDrawing(dg);
            AddProjectileDrawing(dg);
            return dg;
        }
        /// <summary>
        /// Draw all enemies
        /// </summary>
        /// <param name="dg"></param>
        private void AddEnemiesDrawing(DrawingGroup dg)
        {
            Brush enemyBrush = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0));
            foreach (MovingGameItem enemy in model.Enemies)
            {
                GeometryDrawing enemyGeo = new GeometryDrawing(enemyBrush,
                    new Pen(Brushes.Black, 1),
                    new EllipseGeometry(enemy.Area));
                dg.Children.Add(enemyGeo);
            }
        }
        /// <summary>
        /// Draw all projectiles
        /// </summary>
        /// <param name="dg"></param>
        private void AddProjectileDrawing(DrawingGroup dg)
        {
            Brush projBrush = new SolidColorBrush(Color.FromArgb(50, 220, 60, 255));
            foreach (Projectile p in model.Projectiles)
            {
                GeometryDrawing projGeo = new GeometryDrawing(projBrush,
                    new Pen(Brushes.DarkMagenta, 1),
                    new EllipseGeometry(p.Area));
                dg.Children.Add(projGeo);
            }
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

            oldTowers = new GeometryDrawing(Brushes.DarkGray, new Pen(Brushes.Black, 1), g);

            return oldTowers;
        }
    }
}