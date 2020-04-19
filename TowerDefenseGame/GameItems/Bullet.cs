using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerDefenseGame.Abstracts;

namespace TowerDefenseGame.GameItems
{
    public class Bullet : Projectile
    {
        public Bullet(double x, double y, double w, double h, int m, int d, Enemy t = null) : base(x, y, w, h, m, d, t)
        {

        }
    }
}
