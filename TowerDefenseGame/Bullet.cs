using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TowerDefenseGame
{
    class Bullet : Projectile
    {
        public Bullet(double x, double y, double w, double h, Point p, double d) : base(x, y, w, h, p, d)
        {

        }
    }
}
