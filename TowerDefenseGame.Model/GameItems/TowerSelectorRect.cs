using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefenseGame.Model.Abstracts;

namespace TowerDefenseGame.Model.GameItems
{
    /// <summary>
    /// Class of the tower selector rectangulars 
    /// </summary>
    [Serializable]
    public class TowerSelectorRect : GameItem
    {
        private bool selected;
        private int price;
        private DamageType type;

        /// <summary>
        /// Returns or sets true if the given tower was selected
        /// </summary>
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        /// <summary>
        /// Price of the selected tower
        /// </summary>
        public int Price
        {
            get { return price; }
        }

        /// <summary>
        /// Damage type of the selected tower
        /// </summary>
        public DamageType damageType
        {
            get { return type; }
        }

        /// <summary>
        /// Constructor of the TowerSelectroRect class
        /// </summary>
        /// <param name="x">position X coordinate</param>
        /// <param name="y">position Y coordinate</param>
        /// <param name="w">width of unit pixels</param>
        /// <param name="h">height of unit pixels</param>
        /// <param name="i">the damage type depends on its number</param>
        public TowerSelectorRect(double x, double y, double w, double h, int i) : base(x, y, w, h)
        {
            selected = false;
            type = GetDamageType(i);
            price = GetPrice(type);
        }

        private DamageType GetDamageType(int num)
        {
            DamageType damageType = DamageType.physical;

            switch (num)
            {
                case 1:
                    damageType = DamageType.poison;
                    break;
                case 2:
                    damageType = DamageType.fire;
                    break;
                case 3:
                    damageType = DamageType.frost;
                    break;
                case 4:
                    damageType = DamageType.air;
                    break;
                case 5:
                    damageType = DamageType.earth;
                    break;
            }

            return damageType;
        }

        private int GetPrice(DamageType type)
        {
            int price = 0;

            switch (type)
            {
                case DamageType.physical:
                    price = 250;
                    break;
                case DamageType.fire:
                    price = 260;
                    break;
                case DamageType.frost:
                    price = 270;
                    break;
                case DamageType.air:
                    price = 250;
                    break;
                case DamageType.earth:
                    price = 270;
                    break;
                case DamageType.poison:
                    price = 900;
                    break;
            }

            return price;
        }
    }
}
