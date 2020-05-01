using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        Drawing oldCastle;
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
            dg.Children.Add(GetCastle());
            dg.Children.Add(GetTowers());
            AddEnemiesDrawing(dg);
            AddProjectileDrawing(dg);

            FormattedText formattedText = new FormattedText($"{model.Towers.Count.ToString()}/6",System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 24, Brushes.Black);
            GeometryDrawing text = new GeometryDrawing(null, new Pen(Brushes.Black, 3), formattedText.BuildGeometry(new Point(model.GameWidth-5, 5)));

            dg.Children.Add(text);

            return dg;
        }

        private Drawing GetCastle()
        {
            if (oldCastle==null)
            {
                DrawingGroup castle = new DrawingGroup();
                Geometry upperCorner = new RectangleGeometry(new Rect(model.ExitPoint.X, model.ExitPoint.Y - (2 * model.TileSize), model.TileSize, model.TileSize));
                castle.Children.Add(new GeometryDrawing(GetImageBrush("TowerDefenseGame.Image.Castle.S2D0E800.BMP"), null, upperCorner));
                Geometry upperWall = new RectangleGeometry(new Rect(model.ExitPoint.X, model.ExitPoint.Y - model.TileSize, model.TileSize, model.TileSize));
                castle.Children.Add(new GeometryDrawing(GetImageBrush("TowerDefenseGame.Image.Castle.S2D0B800.BMP"), null, upperWall));
                Geometry gate = new RectangleGeometry(new Rect(model.ExitPoint.X, model.ExitPoint.Y, model.TileSize, model.TileSize));
                castle.Children.Add(new GeometryDrawing(GetImageBrush("TowerDefenseGame.Image.Castle.S2D0B801.BMP"), null, gate));
                Geometry lowerWall = new RectangleGeometry(new Rect(model.ExitPoint.X, model.ExitPoint.Y + model.TileSize, model.TileSize, model.TileSize));
                castle.Children.Add(new GeometryDrawing(GetImageBrush("TowerDefenseGame.Image.Castle.S2D0B800.BMP"), null, lowerWall));
                Geometry lowerCorner = new RectangleGeometry(new Rect(model.ExitPoint.X, model.ExitPoint.Y + (2 * model.TileSize), model.TileSize, model.TileSize));
                castle.Children.Add(new GeometryDrawing(GetImageBrush("TowerDefenseGame.Image.Castle.S2D0E810.BMP"), null, lowerCorner));
                oldCastle = castle;
            }
            return oldCastle;
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
            Brush projPhysBrush = new SolidColorBrush(Color.FromArgb(80, 170, 180, 185));
            Brush projFrosBrush = new SolidColorBrush(Color.FromArgb(80, 135, 195, 235));
            Brush projPoisBrush = new SolidColorBrush(Color.FromArgb(80, 40, 175, 95));
            Brush usedBrush;
            foreach (Projectile p in model.Projectiles)
            {
                switch (p.TypeOfDamage)
                {
                    case DamageType.physical:
                        usedBrush = projPhysBrush;
                        break;
                    case DamageType.frost:
                        usedBrush = projFrosBrush;
                        break;
                    case DamageType.poison:
                        usedBrush = projPoisBrush;
                        break;
                    default:
                        usedBrush = projPhysBrush;
                        break;
                }
                GeometryDrawing projGeo = new GeometryDrawing(usedBrush,
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
                DrawingGroup rndPath = new DrawingGroup();
                #region pathGraphic
                string[] resources = GetEmbendedResourceInFolder("Image.Path.");
                BitmapImage[] pics = new BitmapImage[resources.Length];
                for (int i = 0; i < resources.Length; i++)
                {
                    pics[i] = new BitmapImage();
                    pics[i].BeginInit();
                    pics[i].StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resources[i]);
                    pics[i].EndInit();
                }
                #endregion                
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
                            ImageBrush pathBrush = new ImageBrush(pics[TowerDefenseModel.rnd.Next(0, pics.Length)])
                            {
                                TileMode = TileMode.Tile,
                                Viewport = new Rect(0, 0, model.TileSize, model.TileSize),
                                ViewportUnits = BrushMappingMode.Absolute
                            };
                            rndPath.Children.Add(new GeometryDrawing(pathBrush, null, box));
                        }
                    }
                }
                // Field
                #region FieldGraphic
                resources = GetEmbendedResourceInFolder("Image.Field.");
                BitmapImage[] fieldPics = new BitmapImage[resources.Length];
                for (int i = 0; i < resources.Length; i++)
                {
                    fieldPics[i] = new BitmapImage();
                    fieldPics[i].BeginInit();
                    fieldPics[i].StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resources[i]);
                    fieldPics[i].EndInit();
                }
                #endregion
                ImageBrush fieldBrush = new ImageBrush(fieldPics[1])
                {
                    TileMode = TileMode.Tile,
                    Viewport = new Rect(0, 0, model.TileSize, model.TileSize),
                    ViewportUnits = BrushMappingMode.Absolute
                };
                oldFields = new GeometryDrawing(fieldBrush, new Pen(Brushes.DimGray, 1), g);
                oldPath = rndPath;
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



        private ImageBrush GetImageBrush(string image)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream(image);
            img.EndInit();

            ImageBrush imgBrush = new ImageBrush(img)
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect(0, 0, model.TileSize, model.TileSize),
                ViewportUnits = BrushMappingMode.Absolute
            };
            return imgBrush;
        }
        private string[] GetResourceInFolder(string folder)
        {
            var assembly = Assembly.GetCallingAssembly();
            var resourcesName = assembly.GetName().Name + ".g.resources";
            var stream = assembly.GetManifestResourceStream(resourcesName);
            var resourceReader = new ResourceReader(stream);
            var resources =
                from valval in resourceReader.OfType<DictionaryEntry>()
                let theme = (string)valval.Key
                where theme.StartsWith(folder)
                select theme.Substring(folder.Length);
            return resources.ToArray();            
        }
        private string[] GetEmbendedResourceInFolder(string folder)
        {
            var assembly = Assembly.GetCallingAssembly().GetManifestResourceNames();
            string[] res = assembly.Where(x => x.Contains(folder)).Select(x => x).ToArray();
            return res;
        }
    }
}