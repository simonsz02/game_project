using System;
using System.Collections.Generic;
using System.Windows;
using TowerDefenseGame.Model.Abstracts;
using TowerDefenseGame.Model.GameItems;

namespace TowerDefenseGame.Model
{
    /// <summary>
    /// Damage types
    /// </summary>
    [Serializable]
    public enum DamageType
    {
        /// <summary>
        /// physical damage type
        /// </summary>
        physical,

        /// <summary>
        /// poison damage type
        /// </summary>
        poison,

        /// <summary>
        /// fire damage type
        /// </summary>
        fire,

        /// <summary>
        /// frost damage type
        /// </summary>
        frost,

        /// <summary>
        /// air damage type
        /// </summary>
        air,

        /// <summary>
        /// air damage type
        /// </summary>
        earth,  
    }

    /// <summary>
    /// Class of the game items
    /// </summary>
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
        /// size of the tile
        /// </summary>
        public double TileSize { get; set; }

        /// <summary>
        /// entry pont where the enemies enter the game area
        /// </summary>
        public Point EntryPoint { get; set; }

        /// <summary>
        /// exit pont where the enemies leave the game area
        /// </summary>
        public Point ExitPoint { get; set; }

        /// <summary>
        /// coins with the player can buy tower
        /// </summary>
        public int Coins { get; set; }

        /// <summary>
        /// Constructor of the model class
        /// </summary>
        /// <param name="w">width of the screen</param>
        /// <param name="h">height of the screen</param>
        /// <param name="c">initial coins</param>
        public TowerDefenseModel(double w, double h, int c)
        {
            GameWidth = w;
            GameHeight = h;
            Coins = c;
            TowerSelectorRects = new TowerSelectorRect[Enum.GetValues(typeof(DamageType)).Length];
        }
    }
}
