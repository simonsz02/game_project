﻿using System;
using TowerDefenseGame.Abstracts;

namespace TowerDefenseGame.GameItems
{
    [Serializable]
    public class Missile : Projectile
    {
        public Missile(double x, double y, double w, double h, int m, int d, DamageType dt, Enemy t = null) : base(x, y, w, h, m, d, dt, t)
        {
        }

        public override bool CauseDamage(Enemy enemy, Action<Enemy> die)
        {
            return enemy.ReceiveDamage(this.Damage, DamageType.physical, die);
        }
    }
}
