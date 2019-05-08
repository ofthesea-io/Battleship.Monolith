namespace Battleship.Core.Tests
{
    using Battleship.Core.Components;
    using Battleship.Core.Components.Ships;

    using NUnit.Framework;

    [TestFixture]
    public class ShipTests : ComponentBase
    {
        [Test]
        public void BattleShip_HitCounterEqualsShipLength_ReturnIsShipSunkTrue()
        {
            // Arrange
            IShip battleShip = new BattleShip(1);

            // Act
            for (int i = 0; i < battleShip.ShipLength; i++)
            {
                battleShip.CoordinateStatus++;
            }
            
            bool isSunk = battleShip.CoordinateStatus == battleShip.ShipLength;

            // Assert
            Assert.IsTrue(isSunk);
        }

        [Test]
        public void Destroyer_HitCounterEqualsShipLength_ReturnIsShipSunkTrue()
        {
            // Arrange
            IShip destroyer = new Destroyer(1);

            // Act
            for (int i = 0; i < destroyer.ShipLength; i++)
            {
                destroyer.CoordinateStatus++;
            }

            bool isSunk = destroyer.CoordinateStatus == destroyer.ShipLength;

            // Assert
            Assert.IsTrue(isSunk);
        }
    }
}