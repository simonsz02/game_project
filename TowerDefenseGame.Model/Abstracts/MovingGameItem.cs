using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TowerDefenseGame.Model.Abstracts
{
    /// <summary>
    /// Class of the game items which move
    /// </summary>
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

        /// <summary>
        /// Constructor of the MovingGameItem class
        /// </summary>
        /// <param name="x">X coordinate of the game item</param>
        /// <param name="y">Y coordinate of the game item</param>
        /// <param name="w">Width of the game item</param>
        /// <param name="h">Height of the game item</param>
        /// <param name="d">Destination of the moving game item</param>
        /// <param name="m">Movement pixels/tick</param>
        public MovingGameItem(double x, double y, double w, double h, Point d, double m) : base(x, y, w, h)
        {
            Destination = d;
            Movement = m;
        }

        /// <summary>
        /// Set the X and Y coordinates of the moving game item
        /// </summary>
        /// <param name="x">X coordinate of the game item</param>
        /// <param name="y">Y coordinate of the game item</param>
        public void SetXY(double x, double y)
        {
            area.X = x;
            area.Y = y;
        }

        /// <summary>
        /// Set the X and Y coordinates of the moving game item by a point
        /// </summary>
        /// <param name="p">point that give the its coordinates</param>
        public void SetXY(Point p)
        {
            area.X = p.X;
            area.Y = p.Y;
        }
    }
}
