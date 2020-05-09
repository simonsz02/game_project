using System;
using System.Runtime.Serialization;
using System.Windows.Threading;
using TowerDefenseGame.Model.Abstracts;

namespace TowerDefenseGame.Model.GameItems
{
    [Serializable]
    public class Tower : GameItem, ITower
    {
        // Ez a flag jelzi, hogy eltelt-e elég idő egy újabb lövés leadásához
        bool canShot;
        protected int price;
        public int Armour { get; set; }
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

        //torony létrehozásakor minden torony egyes szintű
        private int grade;
        public int Grade
        {
            get { return grade; }
            set { grade = value; }
        }

        public int Price
        {
            get { return price; }
        }

        // Ez a metódus hozza létre a Projectile típusú objektumot, 
        // aminek a sebzéstípusa a torony sebzéstípusa lesz
        Action<double, double, double, double, int, int, DamageType, Enemy> LoadGun;

        public Tower(double x, double y, double w, double h, Action<double, double, double, double, int, int, DamageType, Enemy>L, DispatcherTimer timer, DamageType dt = DamageType.physical) : base(x, y, w, h)
        {
            target = null;
            canShot = false;
            LoadGun = L;
            timer.Tick += Timer_Tick;
            //Ennek lehet hogy nem itt kéne lennie
            Range = 2*w;
            TypeOfDamage = dt;
            this.grade = 1;
        }
        public void Timer_Tick(object sender, EventArgs e)
        {
            canShot = true;
            Boom();
        }
        private void Boom()
        {
            if (target != null & canShot)
            {
                if (target.Health > 0 & (target.Centre - Centre).Length < Range)
                {
                    LoadGun(Centre.X, Centre.Y, Area.Width, Area.Height, 8, 10, TypeOfDamage, Target);
                    canShot = false;
                }
            }
        }
    }
}
