using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace TowerDefenseGame.Model.GameItems
{
    /// <summary>
    /// Rocket class of the tower class
    /// </summary>
    [Serializable]
    public class RocketTower : Tower
    {
        /// <summary>
        /// Constructor of the Rocket class
        /// </summary>
        /// <param name="x">position X coordinate</param>
        /// <param name="y">position Y coordinate</param>
        /// <param name="w">width of unit pixels</param>
        /// <param name="h">height of unit pixels</param>
        /// <param name="L">Creates the projectile object</param>
        /// <param name="timer">Time interval the tower shoots</param>
        /// <param name="dt">Damage type of the rocket tower</param>
        /// <param name="price">Price of the rocket tower</param>
        public RocketTower(double x, double y, double w, double h, Action<double, double, double, double, int, int, DamageType, Enemy> L, DispatcherTimer timer, DamageType dt, int price) : base(x, y, w, h, L, timer, dt)
        {
            this.price = price;
        }

    }
}
