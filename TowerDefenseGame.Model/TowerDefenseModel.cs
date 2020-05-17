using System;
using System.Collections.Generic;
using System.Windows;
using TowerDefenseGame.Model.Abstracts;
using TowerDefenseGame.Model.GameItems;

namespace TowerDefenseGame.Model
{
    [Serializable]
    public enum DamageType
    {
        physical, poison, fire, frost, air, earth,  
    }
    [Serializable]
    public class TowerDefenseModel
    {
        private readonly List<Enemy> enemies = new List<Enemy>();
        private readonly List<Tower> towers = new List<Tower>();
        private readonly List<Projectile> projectiles = new List<Projectile>();

        /// <summary>
        /// list of the enemies
        /// </summary>
        public List<Enemy> Enemies { get { return enemies; } }

        /// <summary>
        /// list of the towers
        /// </summary>
        public List<Tower> Towers { get { return towers; } }

        /// <summary>
        /// list of the projectiles
        /// </summary>
        public List<Projectile> Projectiles { get { return projectiles; } }

        /// <summary>
        /// array of the tower selector rectangular
        /// </summary>
        public TowerSelectorRect[] TowerSelectorRects { get; set; }

        /// <summary>
        /// two dimension array of the fields
        /// </summary>
        public bool[,] Fields { get; set; }

        /// <summary>
        /// two dimension array of the path
        /// </summary>
        public bool[,] Path { get; set; }

        /// <summary>
        /// actual gamewidth
        /// </summary>
        public double GameWidth { get; set; }

        /// <summary>
        /// actual gameheight
        /// </summary>
        public double GameHeight { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double TileSize { get; set; }
        public Point EntryPoint { get; set; }
        public Point ExitPoint { get; set; }
        public int Coins { get; set; }

        public TowerDefenseModel(double w, double h, int c)
        {
            GameWidth = w;
            GameHeight = h;
            Coins = c;
            TowerSelectorRects = new TowerSelectorRect[Enum.GetValues(typeof(DamageType)).Length];
        }
    }
}
