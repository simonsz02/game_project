﻿using System;
using TowerDefenseGame.Model.GameItems;

namespace TowerDefenseGame.Model
{
    public interface IEnemy
    {
        /// <summary>
        /// Handles amount of damage received
        /// </summary>
        /// <param name="dam">Value of damage caused to the enemy</param>
        /// <returns>True if enemy is alive False if not</returns>
        bool ReceiveDamage(double damage, DamageType type, Action<Enemy> die);
    }
}