using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseGame.Model
{
    /// <summary>
    /// MenuModel constructor
    /// </summary>
    public class MenuModel
    {
        /// <summary>
        /// 
        /// </summary>
        public double GameWidth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double GameHeight { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameWidth"></param>
        /// <param name="gameHeight"></param>
        public MenuModel(double gameWidth, double gameHeight)
        {
            GameWidth = gameWidth;
            GameHeight = gameHeight;
        }
    }
}
