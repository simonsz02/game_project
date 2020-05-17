using System;
using TowerDefenseGame.Model.Abstracts;

namespace TowerDefenseGame.Model.GameItems
{
    /// <summary>
    /// Missile class of the Projectile class
    /// </summary>
    [Serializable]
    public class Missile : Projectile
    {
        /// <summary>
        /// Constructor of the Missile class
        /// </summary>
        /// <param name="x">position X coordinate</param>
        /// <param name="y">position Y coordinate</param>
        /// <param name="w">width of unit pixels</param>
        /// <param name="h">height of unit in pixels</param>
        /// <param name="m">Movement pixels/tick</param>
        /// <param name="d">Initial destination</param>
        /// <param name="dt">Damage type of the missile</param>
        /// <param name="t">Target enemy</param>
        public Missile(double x, double y, double w, double h, int m, int d, DamageType dt, Enemy t = null) : base(x, y, w, h, m, d, dt, t)
        {
        }
        /// <summary>
        /// Do damage. A missile causes splash damage in radius of tileSize
        /// </summary>
        /// <param name="enemy">The primary target of the damage</param>
        /// <param name="die"></param>
        /// <param name="dt">damage type of the missile</param>
        /// <returns>Returns true if the health of the target is above 0, false if not</returns>
        public override bool CauseDamage(Enemy enemy, Action<Enemy> die, DamageType dt)
        {
            return enemy.ReceiveDamage(Damage, dt, die);
        }
    }
}
