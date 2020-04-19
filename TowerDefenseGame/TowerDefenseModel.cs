using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerDefenseGame.Abstracts;
using TowerDefenseGame.GameItems;
using TowerDefenseGame.Interface;

namespace TowerDefenseGame
{
    public enum DamageType
    {
        physical, frost, fire, earth, air, magic, poison
    }
    class TowerDefenseModel
    {
        public bool debug = true;
        private readonly List<Enemy> enemies = new List<Enemy>();
        private readonly List<Tower> towers = new List<Tower>(); 
        private readonly List<Projectile> projectiles = new List<Projectile>(); 
        public List<Enemy> Enemies { get { return this.enemies; } }
        public List<Tower> Towers { get { return this.towers; } }
        public List<Projectile> Projectiles { get { return this.projectiles; } }
        public bool[,] Fields { get; set; }
        public bool[,] Path { get; set; }
        public bool[,] Towerplaces { get; set; }
        public double GameWidth { get; set; }
        public double GameHeight { get; set; }
        public double TileSize { get; set; }
        public Point EntryPoint { get; set; }
        public Point ExitPoint { get; set; }

        public TowerDefenseModel(double w, double h)
        {
            GameWidth = w;
            GameHeight = h;
        }
    }
}
