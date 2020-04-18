using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TowerDefenseGame
{
    class Projectile : MovingGameItem
    {
        public Projectile(double x, double y, double w, double h, Point p, double s) : base(x, y, w, h, p, s)
        {

        }
    }
}
