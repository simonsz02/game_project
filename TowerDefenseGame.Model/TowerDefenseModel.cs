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
        public List<Enemy> Enemies { get { return enemies; } }
        public List<Tower> Towers { get { return towers; } }
        public List<Projectile> Projectiles { get { return projectiles; } }
        public TowerSelectorRect[] TowerSelectorRects { get; set; } 
        public bool[,] Fields { get; set; }
        public bool[,] Path { get; set; }
        public double GameWidth { get; set; }
        public double GameHeight { get; set; }
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
