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
        public int Dx { get; set; }
        public int Dy { get; set; }
        public GameItem(double x, double y, double w, double h)
        {
            area = new Rect(x, y, w, h);
            Dx = 0;
            Dy = 0;
        }

        public void ChangeX(double diff)
        {
            area.X += diff;
        }
        public void ChangeY(double diff)
        {
            area.Y += diff;
        }
        public void SetXY(double x, double y)
        {
            area.X = x;
            area.Y = y;
        }
    }
}
