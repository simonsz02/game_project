using System;
using System.Windows.Threading;
using TowerDefenseGame.Abstracts;

namespace TowerDefenseGame.GameItems
{
    [Serializable]
    public class Tower : GameItem, ITower
    {
        public int Armour { get; set; }
        public double SlowDown { get; set; }
        public int SelfHealing { get; set; }
        // Az a pixelben vett távolság, ameddig a torony lőni képes
        public double Range { get; set; }
        // Ez határozza meg, hogy miylen típusú sebzést okoz a lőszere
        DamageType TypeOfDamage { get; set; }
        // Ez az az ellenség, amire lő a torony
        Enemy target;
        public Enemy Target
        {
            get { return target; }
            set { target = value; Boom(); }
        }
        // Ez a metódus hozza létre a Projectile típusú objektumot, 
        // aminek a sebzéstípusa a torony sebzéstípusa lesz
        Action<double, double, double, double, int, int, DamageType, Enemy> LoadGun;
        // Ez a flag jelzi, hogy eltelt-e elég idő egy újabb lövés leadásához
        bool CanShoot { get; set; }

        public Tower(double x, double y, double w, double h, Action<double, double, double, double, int, int, DamageType, Enemy> L, DispatcherTimer timer, DamageType dt = DamageType.physical) : base(x, y, w, h)
        {
            target = null;
            CanShoot = false;
            LoadGun = L;
            timer.Tick += Timer_Tick;
            //Ennek lehet hogy nem itt kéne lennie
            Range = 300;
            TypeOfDamage = dt;
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            CanShoot = true;
            Boom();
        }

        private void Boom()
        {
            if (target != null & CanShoot)
            {
                if (target.Health > 0 & (target.Location - Location).Length < Range)
                {
                    LoadGun(Area.X, Area.Y, Area.Width, Area.Height, 8, 10, TypeOfDamage, Target);
                    CanShoot = false;
                }
            }
        }
    }
}
