using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseGame
{
    class MovingGameItem : GameItem
    {

        public MovingGameItem(double x, double y, double w, double h) : base(x, y, w, h)
        {
            
        }

        public void SetXY(double x, double y)
        {
            area.X = x;
            area.Y = y;
        }
    }
}
