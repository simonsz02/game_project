using System;
using System.Runtime.Serialization;
using System.Windows.Threading;
using TowerDefenseGame.Model.Abstracts;

namespace TowerDefenseGame.Model.GameItems
{
    /// <summary>
    /// Top class of the tower hierarchy
    /// </summary>
    [Serializable]
    public class Tower : GameItem, ITower
    {
        // Ez a flag jelzi, hogy eltelt-e elég idő egy újabb lövés leadásához
        bool canShot;

        /// <summary>
        /// Store the price of the tower
        /// </summary>
        protected int price;

        /// <summary>
        /// Armour of the tower
        /// </summary>
        public int Armour { get; set; }

        /// <summary>
        /// Self healing skill volume of the tower
        /// </summary>
        public int SelfHealing { get; set; }

        /// <summary>
        /// Range in pixel where the tower can shoots an enemy
        /// </summary>
        public double Range { get; set; }

        /// <summary>
        /// Damage Type of the tower
        /// </summary>
        DamageType TypeOfDamage { get; set; }

        /// <summary>
        /// Stores the target of the tower
        /// </summary>
        Enemy target;

        /// <summary>
        /// Returns and sets the target of the tower
        /// </summary>
        public Enemy Target
        {
            get { return target; }
            set { target = value; Boom(); }
        }

        /// <summary>
        /// Stores the grade of the tower
        /// </summary>
        private int grade;

        /// <summary>
        /// Returns and sets the target of the tower
        /// </summary>
        public int Grade
        {
            get { return grade; }
            set { grade = value; }
        }

        /// <summary>
        /// Returns the price of the tower
        /// </summary>
        public int Price
        {
            get { return price; }
        }

        /// <summary>
        /// Creates the projectile object
        /// </summary>
        Action<double, double, double, double, int, int, DamageType, Enemy> LoadGun;

        /// <summary>
        /// Constructor of the tower class
        /// </summary>
        /// <param name="x">position X coordinate</param>
        /// <param name="y">position Y coordinate</param>
        /// <param name="w">width of unit pixels</param>
        /// <param name="h">height of unit pixels</param>
        /// <param name="L">Creates the projectile object</param>
        /// <param name="timer">Time interval the tower shoots</param>
        /// <param name="dt">Damage type of the tower</param>
        public Tower(double x, double y, double w, double h, Action<double, double, double, double, int, int, DamageType, Enemy>L, DispatcherTimer timer, DamageType dt = DamageType.physical) : base(x, y, w, h)
        {
            target = null;
            canShot = false;
            LoadGun = L;
            timer.Tick += Timer_Tick;
            Range = 2*w;
            TypeOfDamage = dt;
            grade = 1;
        }

        /// <summary>
        /// Shoot event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Timer_Tick(object sender, EventArgs e)
        {
            canShot = true;
            Boom();
        }

        /// <summary>
        /// Shoot if the enemy within the range and still alive
        /// </summary>
        private void Boom()
        {
            if (target != null & canShot)
            {
                if (target.Health > 0 & (target.Centre - Centre).Length < Range)
                {
                    LoadGun(Centre.X, Centre.Y, Area.Width, Area.Height, 8, 10+((grade-1)*5), TypeOfDamage, Target);
                    canShot = false;
                }
            }
        }
    }
}
