using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TowerDefenseGame
{
    public abstract class GameItem
    {
        Rect area;
        public Rect Area
        {
            get { return area; }
        }
        /// <summary>
        /// Change of distance TILE!
        /// </summary>
        public int Movement { get; set; }
        /// <summary>
        /// A point to be reached in a straight line TILE!
        /// </summary>
        public Point Destination { get; set; }
        /// <summary>
        /// Previous position TILE!
        /// </summary>
        public Point Origin { get; set; }
        public GameItem(double x, double y, double w, double h)
        {
            area = new Rect(x, y, w, h);
            Movement = 0;
        }
        public GameItem(double x, double y, double w, double h, int m, Point d)
        {
            area = new Rect(x, y, w, h);
            Movement = m;
            Destination = d;
        }
        public void SetXY(double x, double y)
        {
            area.X = x;
            area.Y = y;
        }
    }
}
