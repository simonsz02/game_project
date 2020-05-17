using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TowerDefenseGame.Model.Abstracts
{
    /// <summary>
    /// Top class of the game items hierarchy
    /// </summary>
    [Serializable]
    public abstract class GameItem
    {
        /// <summary>
        /// Store the area of the object
        /// </summary>
        protected Rect area;

        /// <summary>
        /// Returns the area of the game item
        /// </summary>
        public Rect Area
        {
            get { return area; }
        }

        /// <summary>
        /// Returns the coordinates of center of the game item
        /// </summary>
        public Point Centre
        {
            get
            {
                return Point.Add( Area.Location, 
                         new Vector(Area.Width / 2, Area.Height / 2));
            }
            set
            {
                area.Location = Point.Subtract(value,
                         new Vector(Area.Width / 2, Area.Height / 2));
            }
        }

        /// <summary>
        /// Returns the location of the game item
        /// </summary>
        public Point Location
        {
            get
            {
                return area.Location;
            }
            set
            {
                area.Location = value;
            }
        }

        /// <summary>
        /// Constructor of the GameItem class
        /// </summary>
        /// <param name="x">X coordinate of the game item</param>
        /// <param name="y">Y coordinate of the game item</param>
        /// <param name="w">Width of the game item</param>
        /// <param name="h">Height of the game item</param>
        public GameItem(double x, double y, double w, double h)
        {
            area = new Rect(x, y, w, h);
        }
    }
}
