using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefenseGame.Abstracts;

namespace TowerDefenseGame.GameItems
{
    class Tower : GameItem
    {
        public int Armour { get; set; }
        public double SlowDown { get; set; }
        public int SelfHealing { get; set; }
        public double Range { get; set; }

        public Tower(double x, double y, double w, double h) :base(x, y, w, h)
        {

        }


    }
}
