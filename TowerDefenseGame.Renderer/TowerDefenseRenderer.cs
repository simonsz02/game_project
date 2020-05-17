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
    /// <summary>
    /// Draws the Game items on the screen
    /// </summary>
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
        Drawing oldEnemies;
        Drawing oldSelectors;

        /// <summary>
        /// Constructor of the Renderer
        /// </summary>
        /// <param name="model"></param>
        public TowerDefenseRenderer(TowerDefenseModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// Gets back all the game items that will be drawn
        /// </summary>
        /// <returns></returns>
        public Drawing BuildDrawing()
        {
            DrawingGroup dg = new DrawingGroup();
            dg.Children.Add(GetBackground());
            dg.Children.Add(GetFields());
            dg.Children.Add(GetPath());
            dg.Children.Add(GetCastle());
            dg.Children.Add(GetTowers());
            dg.Children.Add(GetNumberOfTowers());
            dg.Children.Add(GetNumberOfCoins());
            dg.Children.Add(GetTowerSelectorPanel());
            dg.Children.Add(GetEnemies());

            //AddEnemiesDrawing(dg);
            AddProjectileDrawing(dg);

            return dg;
        }

        private Drawing GetTowerSelectorPanel()
        {
            DrawingGroup selectorGroups = new DrawingGroup();
            Brush image = null;

            for (int i = 0; i < model.TowerSelectorRects.Length; i++)
            {
                Geometry selectorGeo = new RectangleGeometry(new Rect(model.TowerSelectorRects[i].Area.X, model.TowerSelectorRects[i].Area.Y, model.TowerSelectorRects[i].Area.Width, model.TowerSelectorRects[i].Area.Height));

                switch (i)
                {
                    case 0:
                        image = GetBrush("TowerDefenseGame.Renderer.Image.Selectors.hammer.png",false);
                        break;
                    case 1:
                        image = GetBrush("TowerDefenseGame.Renderer.Image.Selectors.poison.png", false);
                        break;
                    case 2:
                        image = GetBrush("TowerDefenseGame.Renderer.Image.Selectors.fire.png", false);
                        break;
                    case 3:
                        image = GetBrush("TowerDefenseGame.Renderer.Image.Selectors.frost.png", false);
                        break;
                    case 4:
                        image = GetBrush("TowerDefenseGame.Renderer.Image.Selectors.air.png", false);
                        break;
                    case 5:
                        image = GetBrush("TowerDefenseGame.Renderer.Image.Selectors.earth.png", false);
                        break;
                }

                if (model.TowerSelectorRects[i].Selected)
                {
                    selectorGroups.Children.Add(new GeometryDrawing(image, new Pen(Brushes.Red,3), selectorGeo));
                }
                else
                {
                    selectorGroups.Children.Add(new GeometryDrawing(image, null, selectorGeo));
                }

                selectorGroups.Children.Add(GetDamageTypeName(i,model.TowerSelectorRects[i].Area.Width, model.TowerSelectorRects[i].Area.X, model.TowerSelectorRects[i].Area.Y));
                selectorGroups.Children.Add(GetPricesTexts(i, model.TowerSelectorRects[i].Area.Height, model.TowerSelectorRects[i].Area.X, model.TowerSelectorRects[i].Area.Y));
                
            }

            oldSelectors = selectorGroups;

            return oldSelectors;

        }

        private Drawing GetDamageTypeName(int pictNum, double w, double x, double y)
        {
            GeometryDrawing text;

            FormattedText formattedText = new FormattedText($"{model.TowerSelectorRects[pictNum].damageType}", System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 20, Brushes.Yellow);

            text = new GeometryDrawing(null, new Pen(Brushes.Yellow, 2), formattedText.BuildGeometry(new Point(x+w+20, y)));

            return text;
        } 

        private Drawing GetPricesTexts(int pictNum,double h, double x, double y)
        {
            DrawingGroup pricesTexts = new DrawingGroup();

            for (int i = 1; i <= 3; i++)
            {
                GeometryDrawing text;
                GeometryDrawing price;

                FormattedText formattedText = null;
                FormattedText formattedPrice = null;

                int lineSpacing = 0;

                switch (i)
                {
                    case 1:
                        formattedText = new FormattedText("Price", System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 15, Brushes.Yellow);
                        formattedPrice = new FormattedText($"{model.TowerSelectorRects[pictNum].Price}", System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 15, Brushes.Yellow);
                        break;
                    case 2:
                        formattedText = new FormattedText("Upgrade I.", System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 15, Brushes.Yellow);
                        formattedPrice = new FormattedText($"{model.TowerSelectorRects[pictNum].Price* ((int)Math.Pow(2,i-1))}", System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 15, Brushes.Yellow);
                        break;
                    case 3:
                        formattedText = new FormattedText("Upgrade II.", System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 15, Brushes.Yellow);
                        formattedPrice = new FormattedText($"{model.TowerSelectorRects[pictNum].Price * ((int)Math.Pow(2, i - 1))}", System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 15, Brushes.Yellow);
                        break;
                }

                if (i == 1)
                    lineSpacing = 10;
                else
                    lineSpacing = 20;

                text = new GeometryDrawing(null, new Pen(Brushes.Yellow, 1.5), formattedText.BuildGeometry(new Point(x, y + h + (i* lineSpacing))));
                price = new GeometryDrawing(null, new Pen(Brushes.Yellow, 1.5), formattedPrice.BuildGeometry(new Point(x + 90, y + h + (i * lineSpacing))));

                pricesTexts.Children.Add(text);
                pricesTexts.Children.Add(price);
            }

            return pricesTexts;
        }

        private Drawing GetNumberOfTowers()
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

        private Drawing GetNumberOfCoins()
        {
            FormattedText formattedText = new FormattedText(model.Coins.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 26, Brushes.Black);
            GeometryDrawing text = new GeometryDrawing(null, new Pen(Brushes.Gold, 2), formattedText.BuildGeometry(new Point(model.GameWidth * 0.75, 50)));

            return text;
        }

        /// <summary>
        /// Returns the castle tiles
        /// </summary>
        /// <returns></returns>
        private Drawing GetCastle()
        {
            if (oldCastle==null)
            {
                DrawingGroup castle = new DrawingGroup();
                Geometry upperCorner = new RectangleGeometry(new Rect(model.ExitPoint.X, model.ExitPoint.Y - (2 * model.TileSize), model.TileSize, model.TileSize));
                castle.Children.Add(new GeometryDrawing(GetBrush("TowerDefenseGame.Renderer.Image.Castle.s2d0e800.bmp",true), null, upperCorner));
                Geometry upperWall = new RectangleGeometry(new Rect(model.ExitPoint.X, model.ExitPoint.Y - model.TileSize, model.TileSize, model.TileSize));
                castle.Children.Add(new GeometryDrawing(GetBrush("TowerDefenseGame.Renderer.Image.Castle.s2d0b800.bmp",true), null, upperWall));
                Geometry gate = new RectangleGeometry(new Rect(model.ExitPoint.X, model.ExitPoint.Y, model.TileSize, model.TileSize));
                castle.Children.Add(new GeometryDrawing(GetBrush("TowerDefenseGame.Renderer.Image.Castle.s2d0b801.bmp",true), null, gate));
                Geometry lowerWall = new RectangleGeometry(new Rect(model.ExitPoint.X, model.ExitPoint.Y + model.TileSize, model.TileSize, model.TileSize));
                castle.Children.Add(new GeometryDrawing(GetBrush("TowerDefenseGame.Renderer.Image.Castle.s2d0b800.bmp",true), null, lowerWall));
                Geometry lowerCorner = new RectangleGeometry(new Rect(model.ExitPoint.X, model.ExitPoint.Y + (2 * model.TileSize), model.TileSize, model.TileSize));
                castle.Children.Add(new GeometryDrawing(GetBrush("TowerDefenseGame.Renderer.Image.Castle.s2d0e810.bmp",true), null, lowerCorner));
                oldCastle = castle;
            }
            return oldCastle;
        }

        /// <summary>
        /// Animation when player wins
        /// </summary>
        public void ShowWinAnimation()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Animation when player loses
        /// </summary>
        public void ShowLostAnimation()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Draw all enemies
        /// </summary>
        private Drawing GetEnemies()
        {

            DrawingGroup enemies = new DrawingGroup();

            foreach (Enemy enemy in model.Enemies)
            {
                Geometry enemyGeo = new RectangleGeometry(new Rect(enemy.Area.X, enemy.Area.Y, enemy.Area.Width, enemy.Area.Height));

                enemies.Children.Add(new GeometryDrawing(GetBrush("TowerDefenseGame.Renderer.Image.Enemy.ork.png",false), null, enemyGeo));
            }

            oldEnemies = enemies;

            return oldEnemies;

            //foreach (MovingGameItem enemy in model.Enemies)
            //{
            //    int key = enemy.GetHashCode();

            //    if (!imageCache.ContainsKey(key))
            //    {
            //        /*
            //        BitmapImage bi = new BitmapImage();
            //        bi.BeginInit();
            //        bi.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream(GetEmbendedResourceInFolder("Image.Enemy.scythe.png")[0]);
            //        bi.Rotation = Rotation.Rotate270;
            //        bi.EndInit();
            //        */
            //        System.Drawing.Bitmap bim = new System.Drawing.Bitmap(@"C:\Users\ladeak\Source\Repos\Prog4\oenik_prog4_2020_2_l0gkpu_oseu51\TowerDefenseGame.Renderer\Image\Enemy\scythe_small.png");
            //        BitmapImage bi = ToBitmapImage(bim);
            //        //BitmapImage bi = new BitmapImage(new Uri(@"C:\Users\ladeak\Source\Repos\Prog4\oenik_prog4_2020_2_l0gkpu_oseu51\TowerDefenseGame.Renderer\Image\Enemy\scythe.png", UriKind.Absolute));

            //        imageCache.Add(key, bi);
            //    }
            //    ImageDrawing img = new ImageDrawing
            //    {                   
            //        Rect = enemy.Area,
            //        ImageSource = imageCache[key]
            //    };               
            //    dg.Children.Add(img);
            //}
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

        /// <summary>
        /// Returns the background
        /// </summary>
        /// <returns></returns>
        private Drawing GetBackground()
        {
            if (oldBackground == null)
            {
                Geometry backgroundGeometry = new RectangleGeometry(new Rect(0, 0, model.GameWidth, model.GameHeight));
                oldBackground = new GeometryDrawing(GetBrush("TowerDefenseGame.Renderer.Image.Wallpaper.stone.jpg",false), null, backgroundGeometry);
            }
            return oldBackground;
        }


        /// <summary>
        /// Returns the fields
        /// </summary>
        /// <returns></returns>
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
                ImageBrush fieldBrush = new ImageBrush(fieldPics[0])
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
        
        /// <summary>
        /// Returns the existing towers drawings
        /// </summary>
        /// <returns></returns>
        private Drawing GetTowers()
        {
            DrawingGroup towers = new DrawingGroup();

            foreach (Tower tower in model.Towers)
            {
                Geometry towerGeo = new RectangleGeometry(new Rect(tower.Area.X, tower.Area.Y, model.TileSize, model.TileSize));

                switch (tower.Grade)
                {
                    case 1:
                        towers.Children.Add(new GeometryDrawing(GetBrush("TowerDefenseGame.Renderer.Image.Towers.rocket1_dryground.png",true), null, towerGeo));
                        break;
                    case 2:
                        towers.Children.Add(new GeometryDrawing(GetBrush("TowerDefenseGame.Renderer.Image.Towers.rocket2_dryground.png",true), null, towerGeo));
                        break;
                    case 3:
                        towers.Children.Add(new GeometryDrawing(GetBrush("TowerDefenseGame.Renderer.Image.Towers.rocket3_dryground.png",true), null, towerGeo));
                        break;
                }
            }
            oldTowers = towers;

            return oldTowers;
        }

        /// <summary>
        /// Returns the image from the cache or the StreamSource
        /// </summary>
        /// <param name="image"></param>
        /// <param name="IsTiled"></param>
        /// <returns></returns>
        private Brush GetBrush(string image, bool IsTiled)
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

                ImageBrush imgBrush = new ImageBrush(img);

                if (IsTiled)
                {
                    imgBrush.TileMode = TileMode.Tile;
                    imgBrush.Viewport = new Rect(0, 0, model.TileSize, model.TileSize);
                    imgBrush.ViewportUnits = BrushMappingMode.Absolute;
                }

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