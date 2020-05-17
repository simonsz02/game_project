using System;
using TowerDefenseGame.Model.GameItems;

namespace TowerDefenseGame.Model
{
    /// <summary>
    /// Interface for the enemies
    /// </summary>
    public interface IEnemy
    {
        /// <summary>
        /// Handles amount of damage received
        /// </summary>
        /// <param name="damage">Value of damage caused to the enemy</param>
        /// <param name="type">Damage type that the enemy receives</param>
        /// <param name="die">Die action</param>
        /// <returns>True if enemy is alive False if not</returns>
        bool ReceiveDamage(double damage, DamageType type, Action<Enemy> die);
    }
}