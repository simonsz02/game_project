using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TowerDefenseGame.Model.Abstracts;
using TowerDefenseGame.Model.Interface;

namespace TowerDefenseGame.Model.GameItems
{
    [Serializable]
    public class Enemy : MovingGameItem, IEnemy
    {

        double health;
        double armor;
        int reward;

        //Represents the survival abiltiy of the unit
        public double Health
        {
            get { return health; }
            set { health = value; }
        }

        //Represents the damage mitigation ability of the unit
        public double Armor
        {
            get { return armor; }
            set { armor = value; }
        }

        ////Represents the reward the player gets after killing the given enemy. 
        ///The reward depends on how fast and healthy the killed enemy was
        public int Reward
        {
            get { return reward; }
        }

        /// <summary>
        /// This class represents an enemy
        /// </summary>
        /// <param name="x">position X coordinate</param>
        /// <param name="y">position Y coordinate</param>
        /// <param name="w">width of unit pixels</param>
        /// <param name="h">height of unit in pixels</param>
        /// <param name="health">Health</param>
        /// <param name="armor">Armor</param>
        /// <param name="d">Initial destination</param>
        /// <param name="m">Movement pixels/tick</param>
        public Enemy(double x, double y, double w, double h, double health, double armor, Point d, double m) : base(x, y, w, h, d, m)
        {
            this.health = health;
            this.armor = armor;
            this.reward = (int)health * (int)m/2;
        }
        /// <summary>
        /// Every unit handles its received damage on its own
        /// </summary>
        /// <param name="damage">received damage value</param>
        /// <param name="type">received damage type</param>
        /// <param name="die">death handler</param>
        /// <returns>Boolean value true if the units health is over 0, false if not</returns>
        public bool ReceiveDamage(double damage, DamageType type, Action<Enemy> die)
        {
            switch (type)
            {
                case DamageType.physical:
                    Health -= Math.Max(damage - Armor,0);
                    break;
                case DamageType.frost:
                    Health -= Math.Max(damage - Armor, 0)*0.8;
                    Movement -= Movement - damage * 0.2>0 ? 0 : damage * 0.2;
                    break;
                case DamageType.fire:
                    Health -= damage;
                    break;
                case DamageType.earth:
                    Health -= Math.Max(damage - Armor, 0) * 0.8;
                    Armor -= Armor - damage * 0.2 > 2 ? 0 : damage * 0.2;
                    break;
                case DamageType.air:
                    Health -= Math.Max(damage - Armor, 0) * 0.2;
                    switch (Math.Round(health)%8)
                    {
                        case 0:
                            area.X += Movement * 20;
                            break;
                        case 1:
                            area.X -= Movement * 20;
                            break;
                        case 2:
                            area.Y += Movement * 20;
                            break;
                        case 3:
                            area.Y -= Movement * 20;
                            break;
                        case 4:
                            area.X += Movement * 20;
                            area.Y -= Movement * 20;
                            break;
                        case 5:
                            area.X -= Movement * 20;
                            area.Y += Movement * 20;
                            break;
                        case 6:
                            area.X += Movement * 20;
                            area.Y += Movement * 20;
                            break;
                        case 7:
                            area.X -= Movement * 20;
                            area.Y -= Movement * 20;
                            break;
                        case 8:
                            area.Y -= Movement * 20;
                            break;
                    }
                    break;
                case DamageType.magic:
                    break;  
                case DamageType.poison:
                    //Poison típusú sebzésnél a sebzés másodpercenként feleződik,
                    //ameddig többet sebezne, mint 2
                    if (damage > 2 && Health > 0)
                    {
                        Health -= damage;
                        new Thread(() => {
                            Thread.Sleep(1000);
                            if (!ReceiveDamage(damage / 2, DamageType.poison, die))
                            {
                                die(this);
                            }                            
                        }).Start();
                    };
                    break;
                default:
                    Health -= Math.Max(damage - Armor, 0);
                    break;
            }
            return Health > 0;
        }
    }
}
