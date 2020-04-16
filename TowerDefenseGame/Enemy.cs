using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerDefenseGame.Interface;

namespace TowerDefenseGame
{
    class Enemy : GameItem, IGameItem, IEnemy
    {
        public Enemy(double x, double y, double w, double h) : base(x, y, w, h)
        {
        }

        public int Health { get; set; }
        public int Armour { get; set; }
        public int Range { get; set; }

        public IGameItem Target { get; set; }

        public Point Destination { get; set; }
        public Point Position { get; set; }
        public Point Origin { get; set; }

        public bool ReceiveDamage(double dam)
        {
            throw new NotImplementedException();
        }
    }
}
