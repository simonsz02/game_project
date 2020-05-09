using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace TowerDefenseGame.Model.GameItems
{
    [Serializable]
    public class GunTower : Tower
    {
        public GunTower(double x, double y, double w, double h, Action<double, double, double, double, int, int, DamageType, Enemy> L, DispatcherTimer timer, DamageType dt = DamageType.physical) : base(x, y, w, h, L, timer, dt)
        {
            this.price = 250;  
        }
    }
}
