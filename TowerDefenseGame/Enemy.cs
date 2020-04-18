using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerDefenseGame.Interface;

namespace TowerDefenseGame
{
    class Enemy : MovingGameItem, IEnemy
    {
        int health;
        int armor;
        

        public int Health { get { return health; } set { health = value; } }
        public int Armor { get { return armor; } set { armor = value; } }

        public Enemy(double x, double y, double w, double h, int health, int armor, Point d, double s) : base(x, y, w, h, d, s)
        {
            this.health = health;
            this.armor = armor;
        }

        public bool ReceiveDamage(double dam)
        {
            throw new NotImplementedException();
        }
    }
}
