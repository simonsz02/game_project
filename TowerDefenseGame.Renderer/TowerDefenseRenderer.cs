using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TowerDefenseGame.Model;
using TowerDefenseGame.Model.Abstracts;
using TowerDefenseGame.Model.GameItems;
using TowerDefenseGame.Logic;

namespace TowerDefenseGame.Renderer
{
    [Serializable]
    public class TowerDefenseRenderer
    {
        TowerDefenseModel model;

        Dictionary<string, ImageBrush> imageBrushCache = new Dictionary<string, ImageBrush>();
        Dictionary<int, BitmapImage> imageCache = new Dictionary<int, BitmapImage>();

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
            dg.Children.Add(NumberOfTowers());
            dg.Children.Add(NumberOfCoins());

            //AddEnemiesDrawing(dg);
            AddProjectileDrawing(dg);

            return dg;
        }

        private Drawing NumberOfTowers()
        {
            GeometryDrawing text;
            
            FormattedText formattedText = new FormattedText($"{model.Towers.Count.ToString()} / 6", System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 26, Brushes.Black);
            if(model.Towers.Count < 6)
            {
                text = new GeometryDrawing(null, new Pen(Brushes.Black, 2), formattedText.BuildGeometry(new Point(model.GameWidth * 0.75, 10)));
            }
            else
            {
                text = new GeometryDrawing(null, new Pen(Brushes.Red, 2), formattedText.BuildGeometry(new Point(model.GameWidth * 0.75, 10)));
            }
            
            return text;
        }

        private Drawing NumberOfCoins()
        {
            FormattedText formattedText = new FormattedText(model.Coins.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 26, Brushes.Black);
            GeometryDrawing text = new GeometryDrawing(null, new Pen(Brushes.Gold, 2), formattedText.BuildGeometry(new Point(model.GameWidth * 0.75, 50)));

            return text;
        }

        private Drawing GetCastle()
        {
            if (oldCastle==null)
            {
                DrawingGroup castle = new DrawingGroup();
                Geometry upperCorner = new RectangleGeometry(new Rect(model.ExitPoint.X, model.ExitPoint.Y - (2 * model.TileSize), model.TileSize, model.TileSize));
                castle.Children.Add(new GeometryDrawing(GetBrush("TowerDefenseGame.Renderer.Image.Castle.s2d0e800.bmp"), null, upperCorner));
                Geometry upperWall = new RectangleGeometry(new Rect(model.ExitPoint.X, model.ExitPoint.Y - model.TileSize, model.TileSize, model.TileSize));
                castle.Children.Add(new GeometryDrawing(GetBrush("TowerDefenseGame.Renderer.Image.Castle.s2d0b800.bmp"), null, upperWall));
                Geometry gate = new RectangleGeometry(new Rect(model.ExitPoint.X, model.ExitPoint.Y, model.TileSize, model.TileSize));
                castle.Children.Add(new GeometryDrawing(GetBrush("TowerDefenseGame.Renderer.Image.Castle.s2d0b801.bmp"), null, gate));
                Geometry lowerWall = new RectangleGeometry(new Rect(model.ExitPoint.X, model.ExitPoint.Y + model.TileSize, model.TileSize, model.TileSize));
                castle.Children.Add(new GeometryDrawing(GetBrush("TowerDefenseGame.Renderer.Image.Castle.s2d0b800.bmp"), null, lowerWall));
                Geometry lowerCorner = new RectangleGeometry(new Rect(model.ExitPoint.X, model.ExitPoint.Y + (2 * model.TileSize), model.TileSize, model.TileSize));
                castle.Children.Add(new GeometryDrawing(GetBrush("TowerDefenseGame.Renderer.Image.Castle.s2d0e810.bmp"), null, lowerCorner));
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
            foreach (MovingGameItem enemy in model.Enemies)
            {
                int key = enemy.GetHashCode();

                if (!imageCache.ContainsKey(key))
                {
                    /*
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream(GetEmbendedResourceInFolder("Image.Enemy.scythe.png")[0]);
                    bi.Rotation = Rotation.Rotate270;
                    bi.EndInit();
                    */
                    System.Drawing.Bitmap bim = new System.Drawing.Bitmap(@"C:\Users\ladeak\Source\Repos\Prog4\oenik_prog4_2020_2_l0gkpu_oseu51\TowerDefenseGame.Renderer\Image\Enemy\scythe_small.png");
                    BitmapImage bi = ToBitmapImage(bim);
                    //BitmapImage bi = new BitmapImage(new Uri(@"C:\Users\ladeak\Source\Repos\Prog4\oenik_prog4_2020_2_l0gkpu_oseu51\TowerDefenseGame.Renderer\Image\Enemy\scythe.png", UriKind.Absolute));

                    imageCache.Add(key, bi);
                }
                ImageDrawing img = new ImageDrawing
                {                   
                    Rect = enemy.Area,
                    ImageSource = imageCache[key]
                };               
                dg.Children.Add(img);
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
                            ImageBrush pathBrush = new ImageBrush(pics[TowerDefenseLogic.rnd.Next(0, pics.Length)])
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
            DrawingGroup towers = new DrawingGroup();

            foreach (Tower tower in model.Towers)
            {
                Geometry towerGeo = new RectangleGeometry(new Rect(tower.Area.X, tower.Area.Y, model.TileSize, model.TileSize));

                switch (tower.Grade)
                {
                    case 1:
                        towers.Children.Add(new GeometryDrawing(GetBrush("TowerDefenseGame.Renderer.Image.Towers.rocket1.jpg"), null, towerGeo));
                        break;
                    case 2:
                        towers.Children.Add(new GeometryDrawing(GetBrush("TowerDefenseGame.Renderer.Image.Towers.rocket2.jpg"), null, towerGeo));
                        break;
                    case 3:
                        towers.Children.Add(new GeometryDrawing(GetBrush("TowerDefenseGame.Renderer.Image.Towers.rocket3.jpg"), null, towerGeo));
                        break;
                }
            }
            oldTowers = towers;

            return oldTowers;
        }
        private Brush GetBrush(string image)
        {
            if (imageBrushCache.ContainsKey(image))
            {
                return imageBrushCache[image];
            }
            else
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
                imageBrushCache.Add(image, imgBrush);
                return imageBrushCache[image];
            }
        }
        private string[] GetEmbendedResourceInFolder(string folder)
        {
            var assembly = Assembly.GetCallingAssembly().GetManifestResourceNames();
            string[] res = assembly.Where(x => x.Contains(folder)).Select(x => x).ToArray();
            return res;
        }
        private BitmapImage ToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png); // Was .Bmp, but this did not show a transparent background.

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }
    }
}