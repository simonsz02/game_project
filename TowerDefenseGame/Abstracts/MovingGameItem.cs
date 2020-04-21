using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TowerDefenseGame.Abstracts
{
    [Serializable]
    public abstract class MovingGameItem : GameItem
    {
        /// <summary>
        /// Change of distance TILE!
        /// </summary>
        public double Movement { get; set; }
        /// <summary>
        /// A point to be reached in a straight line TILE!
        /// </summary>
        public Point Destination { get; set; }
        /// <summary>
        /// Previous position TILE!
        /// </summary>
        public Point Origin { get; set; }
        public MovingGameItem(double x, double y, double w, double h, Point d, double m) : base(x, y, w, h)
        {
            Destination = d;
            Movement = m;
        }

        public void SetXY(double x, double y)
        {
            area.X = x;
            area.Y = y;
        }
    }
}
