using System;

namespace TowerDefenseGame.Model
{
    public interface ITower
    {
        void Timer_Tick(object sender, EventArgs e);
    }
}