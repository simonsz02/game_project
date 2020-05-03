using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseGame.Model
{
    public class MenuModel
    {
        public double GameWidth { get; set; }
        public double GameHeight { get; set; }

        public MenuModel(double gameWidth, double gameHeight)
        {
            GameWidth = gameWidth;
            GameHeight = gameHeight;
        }
    }
}
