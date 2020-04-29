using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TowerDefenseGame.Abstracts
{
    public abstract class GameItem
    {
        protected Rect area;
        public Rect Area
        {
            get { return area; }
        }
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
        public GameItem(double x, double y, double w, double h)
        {
            area = new Rect(x, y, w, h);
        }
    }
}
