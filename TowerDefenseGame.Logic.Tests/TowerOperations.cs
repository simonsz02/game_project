using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerDefenseGame.Model;

namespace TowerDefenseGame.Logic.Tests
{

    [TestFixture]
    public class TowerOperations
    {


        [TestCase]
        public void AddTower()
        {
            //Arrange
            TowerDefenseModel model = new TowerDefenseModel(800, 600, 1000);
            TowerDefenseLogic logic = new TowerDefenseLogic(model);
            bool OperationHasFailed;

            //Act
            OperationHasFailed = logic.AddOrUpgradeTower(new Point(1, 1), new System.Windows.Threading.DispatcherTimer());

            //Assert
            Assert.Equals(OperationHasFailed,false);

        }
    }
}
