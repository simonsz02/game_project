﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TowerDefenseGame.Menu
{
    class MenuRenderer
    {
        MenuModel model;
        DrawingGroup dg;
        public Drawing[] MenuItems { get; private set; }
        public MenuRenderer(MenuModel model)
        {
            this.model = model;
            MenuItems = new GeometryDrawing[3];
        }
        public Drawing BuildDrawing()
        {
            dg = new DrawingGroup();
            GetBackground();
            MenuItems[0] = CreateButton("New Game", 100);
            MenuItems[1] = CreateButton("Load Game", 160);
            MenuItems[2] = CreateButton("Highscore", 220);
            return dg;
        }
        private void GetBackground()
        {
            Geometry backgroundGeometry = new RectangleGeometry(new Rect(0, 0, model.GameWidth, model.GameHeight));
            dg.Children.Add(new GeometryDrawing(Brushes.Black, null, backgroundGeometry));
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
            GeometryDrawing buttonText = new GeometryDrawing(null, new Pen(Brushes.Black, 1),
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
    }

}
