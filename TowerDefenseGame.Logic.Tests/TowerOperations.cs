using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerDefenseGame.Model;
using TowerDefenseGame.Model.Interface;

namespace TowerDefenseGame.Logic.Tests
{

    [TestFixture]
    public class TowerOperations
    {
        [TestCase]
        public void AddTower()
        {
            //Arrange
            TowerDefenseModel model = new TowerDefenseModel(1920, 1080, 1000);
            TowerDefenseLogic logic = new TowerDefenseLogic(model, true);
            bool OperationHasFailed;

            //Act
            OperationHasFailed = logic.AddOrUpgradeTower(new Point(1, 1), new System.Windows.Threading.DispatcherTimer());

            //Assert
            Assert.AreEqual(OperationHasFailed,false);
            Assert.AreEqual(model.Towers.Count, 1);
        }

        [TestCase]
        public void UpgradeTower()
        {
            //Arrang
            TowerDefenseModel model = new TowerDefenseModel(1920, 1080, 1000);
            TowerDefenseLogic logic = new TowerDefenseLogic(model, true);
            bool OperationHasFailed;

            //Act
            OperationHasFailed = logic.AddOrUpgradeTower(new Point(1, 1), new System.Windows.Threading.DispatcherTimer());
            OperationHasFailed = logic.AddOrUpgradeTower(new Point(1, 1), new System.Windows.Threading.DispatcherTimer());

            //Assert
            Assert.AreEqual(OperationHasFailed, false);
            Assert.AreEqual(model.Towers[0].Grade,2);
        }

        [TestCase]
        public void RemoveTower()
        {
            //Arrange
            TowerDefenseModel model = new TowerDefenseModel(1920, 1080, 1000);
            TowerDefenseLogic logic = new TowerDefenseLogic(model, true);
            bool OperationHasFailed;

            //Act
            OperationHasFailed = logic.AddOrUpgradeTower(new Point(1, 1), new System.Windows.Threading.DispatcherTimer());
            OperationHasFailed = logic.RemoveTower(new Point(1, 1));

            //Assert
            Assert.AreEqual(OperationHasFailed, false);
            Assert.AreEqual(model.Towers.Count,0);
        }
    }
}
