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
            TowerDefenseLogic logic = new TowerDefenseLogic();
            bool OperationHasFailed;

            //Act
            OperationHasFailed = logic.AddOrUpgradeTower(new Point(1, 1), new System.Windows.Threading.DispatcherTimer());

            //Assert
            Assert.AreEqual(OperationHasFailed,false);
        }

        [TestCase]
        public void UpgradeTower()
        {
            //Arrang
            TowerDefenseLogic logic = new TowerDefenseLogic();
            bool OperationHasFailed;

            //Act
            OperationHasFailed = logic.AddOrUpgradeTower(new Point(1, 1), new System.Windows.Threading.DispatcherTimer());
            OperationHasFailed = logic.AddOrUpgradeTower(new Point(1, 1), new System.Windows.Threading.DispatcherTimer());

            //Assert
            Assert.AreEqual(OperationHasFailed, false);
        }

        [TestCase]
        public void RemoveTower()
        {
            //Arrange
            TowerDefenseLogic logic = new TowerDefenseLogic();
            bool OperationHasFailed;

            //Act
            OperationHasFailed = logic.AddOrUpgradeTower(new Point(1, 1), new System.Windows.Threading.DispatcherTimer());
            OperationHasFailed = logic.RemoveTower(new Point(1, 1));

            //Assert
            Assert.AreEqual(OperationHasFailed, false);
        }
    }
}
