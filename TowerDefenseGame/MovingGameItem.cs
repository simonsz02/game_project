using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseGame
{
    class MovingGameItem : GameItem
    {
        public int Dx { get; set; }
        public int Dy { get; set; }

        public MovingGameItem(double x, double y, double w, double h) : base(x, y, w, h)
        {
            
        }

        public void ChangeX(double diff)
        {
            area.X += diff;
        }

        public void ChangeY(double diff)
        {
            area.Y += diff;
        }
    }
}
