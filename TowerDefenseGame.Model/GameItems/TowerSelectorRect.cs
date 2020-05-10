using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefenseGame.Model.Abstracts;

namespace TowerDefenseGame.Model.GameItems
{
    public class TowerSelectorRect : GameItem
    {
        private bool selected;

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public TowerSelectorRect(double x, double y, double w, double h) : base(x, y, w, h)
        {
            selected = false;
        }
    }
}
