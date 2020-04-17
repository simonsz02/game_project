using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerDefenseGame.Interface;

namespace TowerDefenseGame
{
    class Enemy : MovingGameItem
    {
        int health;
        int armor;
        double speed;

        public int Health { get { return health; } set { health = value; } }
        public int Armor { get { return armor; } set { armor = value; } }
        public double Speed { get { return speed; } set { speed = value; } }

        public Enemy(double x, double y, double w, double h, int health, int armor, double speed) : base(x, y, w, h)
        {
            this.health = health;
            this.armor = armor;
            this.speed = speed;
        }

        
    }
}
