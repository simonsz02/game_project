using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TowerDefenseGame.Model;
using TowerDefenseGame.Repository;

namespace TowerDefenseGame.Renderer
{
    /// <summary>
    /// Draws the Main menu
    /// </summary>
    public class MenuRenderer
    {
        MenuModel model;
        DrawingGroup dg;

        /// <summary>
        /// Array of the menu items
        /// </summary>
        public Drawing[] MenuItems { get; private set; }

        /// <summary>
        /// If it is true, it shows the highscore list
        /// </summary>
        public bool showHighScoreList { get; set; }

        /// <summary>
        /// Constructor of the Menu Renderer
        /// </summary>
        /// <param name="model"></param>
        public MenuRenderer(MenuModel model)
        {
            this.model = model;
            MenuItems = new GeometryDrawing[3];
            showHighScoreList = false;
        }

        /// <summary>
        /// Draws menu items
        /// </summary>
        /// <returns></returns>
        public Drawing BuildDrawing()
        {
            dg = new DrawingGroup();
            GetBackground();
            MenuItems[0] = CreateButton("New Game", 100);
            MenuItems[1] = CreateButton("Load Game", 160);
            MenuItems[2] = CreateButton("Highscore", 220);
            if (showHighScoreList)
            {
                GetHighScore(new Point(model.GameWidth / 2, 280));
            }
            return dg;
        }

        private void GetHighScore(Point reference)
        {
            dg.Children.Add(new GeometryDrawing(Brushes.Black,
                                                new Pen(Brushes.ForestGreen, 1),
                                                GetFixTetragonal(new Vector(-200, -20),
                                                                new Vector(200, -20),
                                                                new Vector(200, 300),
                                                                new Vector(-200, 300),
                                                                reference
                                                                )));
            dg.Children.Add(new GeometryDrawing(Brushes.ForestGreen,
                                                new Pen(Brushes.ForestGreen, 1),
                                                new LineGeometry(new Point(reference.X, reference.Y),
                                                                 new Point(reference.X, reference.Y += 280))
                                                ));
            List<HighScoreHandler.Row> hsk = HighScoreHandler.ReadHighScoreFile();
            hsk.Sort((x, y) => y.CompareTo(x));
            FormattedText formattedText;
            for (int i = 0; i < Math.Min(hsk.Count,10); i++)
            {
                formattedText = new FormattedText(hsk[i].Name,
                                                  System.Globalization.CultureInfo.CurrentCulture,
                                                  FlowDirection.LeftToRight,
                                                  new Typeface("Calibri"),
                                                  16,
                                                  Brushes.White);
                GeometryDrawing nameText = new GeometryDrawing(null,
                    new Pen(Brushes.White, 1),
                    formattedText.BuildGeometry(Point.Add(new Point(model.GameWidth / 2, 280 + i * 20), new Vector(-150, -5))));
                dg.Children.Add(nameText);

                formattedText = new FormattedText(hsk[i].Score.ToString(),
                                                  System.Globalization.CultureInfo.CurrentCulture,
                                                  FlowDirection.LeftToRight,
                                                  new Typeface("Calibri"),
                                                  16,
                                                  Brushes.White);
                GeometryDrawing scoreText = new GeometryDrawing(null,
                    new Pen(Brushes.White, 1),
                    formattedText.BuildGeometry(Point.Add(new Point(model.GameWidth / 2, 280 + i * 20), new Vector(30, -5))));
                dg.Children.Add(scoreText);
            }
        }

        private void GetBackground()
        {
            Geometry backgroundGeometry = new RectangleGeometry(new Rect(0, 0, model.GameWidth, model.GameHeight));
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream(GetEmbendedResourceInFolder("Image.Menu").First());
            img.EndInit();
            ImageBrush bgBrush = new ImageBrush(img)
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect(0, 0, model.GameWidth, model.GameHeight),
                ViewportUnits = BrushMappingMode.Absolute
            };
            dg.Children.Add(new GeometryDrawing(bgBrush, null, backgroundGeometry));
        }
        private Drawing CreateButton(string text, double verticalShift)
        {
            double vertical = verticalShift;
            Drawing button = new GeometryDrawing(Brushes.Lime,
                                                 new Pen(Brushes.ForestGreen, 1),
                                                 GetFixTetragonal(new Vector(-200, -20),
                                                                  new Vector(200, -10),
                                                                  new Vector(160, 30),
                                                                  new Vector(-150, 20),
                                                                  new Point(model.GameWidth / 2, vertical)
                                                                  )
                                                 );
            FormattedText formattedText = new FormattedText(text,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Calibri"),
                16,
                Brushes.Black);
            GeometryDrawing buttonText = new GeometryDrawing(null, new 
                        Pen(Brushes.Black, 1),
                        formattedText.BuildGeometry(Point.Add(new Point(model.GameWidth / 2, vertical), new Vector(-30, -5))));

            dg.Children.Add(button);
            dg.Children.Add(buttonText);

            return button;
        }
        private Geometry GetFixTetragonal(Vector a, Vector b, Vector c, Vector d, Point m)
        {
            List<Point> points = new List<Point> { Point.Add(m, a), Point.Add(m, b), Point.Add(m, c), Point.Add(m, d) };
            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                geometryContext.BeginFigure(points[0], true, true);
                geometryContext.PolyLineTo(points, true, true);
            }
            return streamGeometry;
        }
        private string[] GetResourceInFolder(string file)
        {
            var assembly = Assembly.GetCallingAssembly();
            var resourcesName = assembly.GetName().Name + ".g.resources";
            var stream = assembly.GetManifestResourceStream(resourcesName);
            var resourceReader = new ResourceReader(stream);
            var resources =
                from valval in resourceReader.OfType<DictionaryEntry>()
                let theme = (string)valval.Key
                where theme.StartsWith(file)
                select theme.Substring(file.Length);
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
