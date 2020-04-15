using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseGame
{
    class TowerDefenseModel
    {
        private readonly List<IEnemy> enemies = new List<IEnemy>();
        private readonly List<ITower> towers = new List<ITower>(); 
        private readonly List<IProjectile> projectiles = new List<IProjectile>(); 
        public List<IEnemy> Enemies { get { return this.enemies; } }
        public List<ITower> Towers { get { return this.towers; } }
        public List<IProjectile> Projectiles { get { return this.projectiles; } }
        public bool[,] Field { get; set; }
        public bool[,] Path { get; set; }
        public bool[,] Towerplaces { get; set; }
        public double GameWidth { get; set; }
        public double GameHeight { get; set; }
        public double TileSize { get; set; }

        public TowerDefenseModel(double w, double h)
        {
            GameWidth = w;
            GameHeight = h;
        }
    }
}
