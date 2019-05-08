using System.Collections.Generic;
using Battleship.Core.Components;
using Battleship.Core.Components.Board;
using Battleship.Core.Components.Ships;
using NUnit.Framework;

namespace Battleship.Core.Tests
{
    [TestFixture]
    public class GridGeneratorTests : ComponentBase
    {
        private readonly IGridGenerator gridGenerator;

        private readonly IShipRandomiser shipRandomiser;

        public GridGeneratorTests()
        {
            shipRandomiser = ShipRandomiser.Instance();
            gridGenerator = GridGenerator.Instance();
        }

        [Test]
        public void Board_WhenGridGenerated_ReturnThirteenOccupiedSegments()
        {
            // Arrange
            var ships = new List<IShip> {new BattleShip(1), new Destroyer(2), new Destroyer(3)};
            var occupiedSegments = shipRandomiser.GetRandomisedShipCoordinates(ships).Count;

            // Act
            var result = gridGenerator.NumberOfOccupiedSegments;

            // Assert
            Assert.AreNotEqual(occupiedSegments, result);
        }

        [Test]
        public void Board_WhenOneBattleShipGenerated_ReturnFiveOccupiedSegments()
        {
            // Arrange
            IShip ship = new BattleShip(0);
            var ships = new List<IShip>();
            ships.Add(ship);
            var occupiedSegments = shipRandomiser.GetRandomisedShipCoordinates(ships).Count;

            // Act

            // Assert
            Assert.AreEqual(occupiedSegments, ship.ShipLength);
        }
    }
}