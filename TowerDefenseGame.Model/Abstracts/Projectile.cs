using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerDefenseGame.Model.GameItems;

namespace TowerDefenseGame.Model.Abstracts
{
    /// <summary>
    /// Projectile class of the MovingGameItem class
    /// </summary>
    [Serializable]
    public abstract class Projectile : MovingGameItem
    {

        /// <summary>
        /// Enemy that the projectile aims to reach
        /// </summary>
        public Enemy Target { get; set; }

        /// <summary>
        /// Volume of the damage caused by the projectile
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Type of the damage the projecile has
        /// </summary>
        public DamageType TypeOfDamage { get; set; }

        /// <summary>
        /// Constuctor of the projectile class
        /// </summary>
        /// <param name="x">X coordinate of the projectile</param>
        /// <param name="y">Y coordinate of the projectile</param>
        /// <param name="w">Width of the projectile</param>
        /// <param name="h">Height of the projectile</param>
        /// <param name="m">Movement pixels/tick</param>
        /// <param name="d">Initial destination</param>
        /// <param name="dt">Damage typeof the projectile</param>
        /// <param name="t">Target of the projectile</param>
        public Projectile(double x, double y, double w, double h, int m, int d, DamageType dt, Enemy t = null) : base(x, y, w, h, new Point(), m)
        {
            Damage = d;
            Target = t;
            TypeOfDamage = dt;
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

        /// <summary>
        /// Decrease the health of the target enemy
        /// </summary>
        /// <param name="enemy">Target enemy</param>
        /// <param name="die">Die action</param>
        /// <param name="dt">Damage type the projectile has</param>
        /// <returns></returns>
        public abstract bool CauseDamage(Enemy enemy, Action<Enemy> die, DamageType dt);
    }
}
