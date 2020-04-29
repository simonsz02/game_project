using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerDefenseGame.GameItems;

namespace TowerDefenseGame.Logic.Tests
{
    [TestFixture]
    class DamageTests
    {
        [Test]
        public void DamageEnemyTest()
        {
            //Arrange
            TowerDefenseModel model = new TowerDefenseModel(100, 100);
            Enemy enemy = new Enemy(0, 0, 5, 5, 20, 2, new Point(0, 0), 5);

            //Act
            enemy.ReceiveDamage(4, DamageType.fire, (Enemy e) => { model.Enemies.Remove(e); });

            //Assert
            Assert.AreEqual(enemy.Health, 16);
        }
    }
}
