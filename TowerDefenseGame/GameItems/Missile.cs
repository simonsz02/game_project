using System;
using TowerDefenseGame.Abstracts;

namespace TowerDefenseGame.GameItems
{
    [Serializable]
    public class Missile : Projectile
    {
        public Missile(double x, double y, double w, double h, int m, int d, DamageType dt, Enemy t = null) : base(x, y, w, h, m, d, dt, t)
        {
        }
        /// <summary>
        /// Do damage. A missile causes splash damage in radius of tileSize
        /// </summary>
        /// <param name="enemy">The primary target of the damage</param>
        /// <param name="die"></param>
        /// <param name="dmgtype">Type of damage</param>
        /// <returns>Returns true if the health of the target is above 0, false if not</returns>
        public override bool CauseDamage(Enemy enemy, Action<Enemy> die, DamageType dmgtype)
        {
            return enemy.ReceiveDamage(Damage, dmgtype, die);
        }
    }
}
