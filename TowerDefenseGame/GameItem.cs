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
        protected Rect area;
        public Rect Area
        {
            get { return area; }
        }
        public GameItem(double x, double y, double w, double h)
        {
            area = new Rect(x, y, w, h);
        }
    }
}
