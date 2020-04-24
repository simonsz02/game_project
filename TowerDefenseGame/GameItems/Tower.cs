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
        public double Range { get; set; }
        Enemy target;
        public Enemy Target
        {
            get { return target; }
            set { target = value; Boom(); }
        }
        Action<double,double,double,double,int,int,Enemy> LoadGun;
        bool CanShoot { get; set; }

        public Tower(double x, double y, double w, double h, int t, Action<double, double, double, double, int, int, Enemy> L, DispatcherTimer timer) :base(x, y, w, h)
        {
            target = null;
            CanShoot = false;
            LoadGun = L;
            timer.Tick += Timer_Tick;
            //Ennek lehet hogy nem itt kéne lennie
            Range = 300;
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
                    LoadGun(Area.X, Area.Y, Area.Width, Area.Height, 8, 10, Target);
                    CanShoot = false;
                }
            }
        }
    }
}
