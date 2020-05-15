using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerDefenseGame.Model;
using TowerDefenseGame.Model.GameItems;

namespace TowerDefenseGame.Logic.Tests
{
    [TestFixture]
    public class DamageTests
    {
        [TestCase(4, DamageType.physical, 18)]
        [TestCase(4, DamageType.fire, 16)]
        public void DamageEnemyTest(int dmg, DamageType dmgt, int expect)
        {
            //Arrange
            Enemy enemy = new Enemy(0, 0, 5, 5, 20, 2, new Point(0, 0), 5);

            //Act
            enemy.ReceiveDamage(dmg, dmgt, (e) => { });

            //Assert
            Assert.AreEqual(enemy.Health, expect);
        }
    }


}
