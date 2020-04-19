using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerDefenseGame.GameItems;

namespace TowerDefenseGame.Abstracts
{
    public abstract class Projectile : MovingGameItem
    {
        public Enemy Target { get; set; }
        public int Damage { get; set; }
        public Projectile(double x, double y, double w, double h, int m, int d, Enemy t = null) : base(x, y, w, h, new Point(), m)
        {
            Damage = d;
            Target = t;
        }
        /// <summary>
        /// Get nearest target
        /// </summary>
        /// <param name="targetList"></param>
        public void GetTarget(List<Enemy> targetList)
        {
            Enemy res = null;
            double minDis = double.MaxValue;
            foreach (Enemy tar in targetList)
            {
                double actDis = Point.Subtract(this.Location, tar.Location).LengthSquared;
                if (minDis > actDis)
                {
                    minDis = actDis;
                    res = tar;
                }
            }
            Target = res;
        }

        public abstract bool CauseDamage(Enemy enemy);
    }
}
