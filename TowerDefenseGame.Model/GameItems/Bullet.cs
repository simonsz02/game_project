using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerDefenseGame.Model.Abstracts;

namespace TowerDefenseGame.Model.GameItems
{
    [Serializable]
    public class Bullet : Projectile
    {
        public Bullet(double x, double y, double w, double h, int m, int d, DamageType dt, Enemy t = null) : base(x, y, w, h, m, d, dt, t)
        {
        }
        /// <summary>
        /// Do damage
        /// </summary>
        /// <param name="enemy">The target of the damage</param>
        /// <param name="die">Death handler</param>
        /// <param name="dmgType">Type of damage</param>
        /// <returns>Returns true if the health of the target is above 0, false if not</returns>
        public override bool CauseDamage(Enemy enemy, Action<Enemy> die, DamageType dt)
        {
            return enemy.ReceiveDamage(Damage, dt, die);
        }        
    }
}
