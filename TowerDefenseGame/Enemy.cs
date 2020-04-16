using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefenseGame.Interface;

namespace TowerDefenseGame
{
    class Enemy : GameItem, IEnemy
    {
        public Enemy(double x, double y, double w, double h) : base(x, y, w, h)
        {
        }

        public bool ReceiveDamage(double dam)
        {
            throw new NotImplementedException();
        }
    }
}
