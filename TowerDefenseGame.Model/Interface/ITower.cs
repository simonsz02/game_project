using System;

namespace TowerDefenseGame.Model
{
    /// <summary>
    /// Interface for the towers
    /// </summary>
    public interface ITower
    {
        /// <summary>
        /// Shoot event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Timer_Tick(object sender, EventArgs e);
    }
}