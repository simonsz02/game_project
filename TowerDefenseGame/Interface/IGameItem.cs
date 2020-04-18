using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TowerDefenseGame.Interface
{
    interface IGameItem
    {
        int Health { get; set; }
        int Armour { get; set; }
        int Range { get; set; }
        IGameItem Target { get; set; }
    }
}
