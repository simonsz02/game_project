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
        IGameItem Target { get; }
        /// <summary>
        /// A point to be reached in a straight line
        /// </summary>
        Point Destination { get; set; }
        /// <summary>
        /// Actual position of object
        /// </summary>
        Point Position { get; set; }
        /// <summary>
        /// Previous position
        /// </summary>
        Point Origin { get; set; }
    }
}
