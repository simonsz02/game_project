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
    public class ResourceTests
    {
        [TestCase(1000)]
        public void GetCoinTest(int coin)
        {
            //Arrange
            TowerDefenseModel model = new TowerDefenseModel(100, 100, coin);
            TowerDefenseLogic logic = new TowerDefenseLogic(model,true);
            Enemy enemy = new Enemy(10, 10, 10, 10, 1, 0, new Point(10, 10), 10);
            model.Enemies.Add(enemy);
            Missile missile = new Missile(11, 11, 11, 11, 1000, 1000, DamageType.fire, enemy);
            model.Projectiles.Add(missile);

            //Act
            logic.MoveProjectiles(model.Projectiles);

            //Assert
            Assert.Greater(model.Coins, coin);
        }

        [TestCase(1000)]
        public void PayWithCoinTest(int coin)
        {
            //Arrange
            TowerDefenseModel model = new TowerDefenseModel(1920, 1080, coin);
            TowerDefenseLogic logic = new TowerDefenseLogic(model, true);
            bool OperationHasFailed;

            //Act
            OperationHasFailed = logic.AddOrUpgradeTower(new Point(1, 1), new System.Windows.Threading.DispatcherTimer()); ;

            //Assert
            Assert.AreEqual(OperationHasFailed, false);
            Assert.Greater(coin, model.Coins);
        }
    }
}

