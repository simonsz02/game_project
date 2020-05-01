using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TowerDefenseGame.Abstracts
{
    [Serializable]
    public abstract class MovingGameItem : GameItem
    {
        /// <summary>
        /// Change of distance pixel!
        /// </summary>
        public double Movement { get; set; }
        /// <summary>
        /// A point to be reached in a straight line TILE!
        /// </summary>
        public Point Destination { get; set; }
        /// <summary>
        /// The angle representing the heading of the entity
        /// </summary>
        public double Direction { get; set; }

        /// <summary>
        /// Previous position TILE!
        /// </summary>
        public Point Origin { get; set; }
        public MovingGameItem(double x, double y, double w, double h, Point d, double m) : base(x, y, w, h)
        {
            Destination = d;
            Movement = m;
        }
        public Point Centre()
        {
            return Point.Add(Area.Location, new Vector(Area.Width / 2, Area.Height / 2));
        }
        public void SetXY(double x, double y)
        {
            area.X = x;
            area.Y = y;
        }
        public void SetXY(Point p)
        {
            area.X = p.X;
            area.Y = p.Y;
        }
    }
}
