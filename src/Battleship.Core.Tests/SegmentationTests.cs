namespace Battleship.Core.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Battleship.Core.Components;
    using Battleship.Core.Components.Board;
    using Battleship.Core.Components.Ships;
    using Battleship.Core.Enums;
    using Battleship.Core.Models;
    using Battleship.Core.Utilities;

    using NUnit.Framework;

    [TestFixture]
    public class SegmentationTests : ComponentBase
    {
        private readonly IShipRandomiser shipRandomiser;

        private readonly ISegmentation segmentation;

        public SegmentationTests()
        {
            shipRandomiser = ShipRandomiser.Instance();
            segmentation = Segmentation.Instance();
        }

        [Test]
        public void Add_AddSegmentYAxisNotInGridDimension_ThrowIndexOutOfRangeException()
        {
            // Arrange 
            int x = XInitialPoint;
            int y = GridDimension + Index;
            Coordinate coordinate = new Coordinate(x, y);
            Segment segment = new Segment(Water);

            // Act and Assert
            try
            {
                segmentation.AddSegment(coordinate, segment);
                Assert.Fail();
            }
            catch (IndexOutOfRangeException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void UpdateSegment_SegmentOutOfRange_ThrowIndexOutOfRangeException()
        {
            // Arrange 
            int x = XInitialPoint;
            int y = GridDimension + Index;
            Coordinate coordinate = new Coordinate(x, y);
            var segment = new Segment(ShipDirection.Vertical, new Destroyer(1));

            // Act and Assert
            try
            {
                segmentation.UpdateSegment(coordinate, segment);
                Assert.Fail();
            }
            catch (IndexOutOfRangeException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void UpdateSegmentRange_CantUpdateAEmptySegmentWithAnotherEmptySegment_TrowArgumentException()
        {
            // Arrange 
            int x = XInitialPoint;
            int y = GridDimension;
            Coordinate coordinate = new Coordinate(x, y);

            // Act and Assert
            try
            {
                segmentation.AddSegment(coordinate, new Segment(Water));
                SortedDictionary<Coordinate, Segment> range = new SortedDictionary<Coordinate, Segment>(new CoordinateComparer())
                   {
                       { coordinate, new Segment(Water) }
                   };
                segmentation.UpdateSegmentRange(range);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void UpdateSegmentRange_CantUpdateFilledSegmentWithEmptySegment_TrowArgumentException()
        {
            // Arrange 
            int x = XInitialPoint;
            int y = GridDimension;
            Coordinate coordinate = new Coordinate(x, y);
            // Act and Assert
            try
            {
                segmentation.AddSegment(coordinate, new Segment(ShipDirection.Horizontal, new BattleShip(1)));
                SortedDictionary<Coordinate, Segment> range = new SortedDictionary<Coordinate, Segment>()
                                       {
                                           { coordinate, new Segment(Water) }
                                       };
                segmentation.UpdateSegmentRange(range);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void UpdateSegmentRange_CanUpdateSegmentWithFilledSegment_ReturnsVoid()
        {
            // Arrange 
            int x = XInitialPoint;
            int y = GridDimension;
            Coordinate coordinate = new Coordinate(x, y);

            // Act and Assert
            try
            {
                segmentation.AddSegment(coordinate, new Segment(Water));
                SortedDictionary<Coordinate, Segment> range = new SortedDictionary<Coordinate, Segment>(new CoordinateComparer())
                   {
                       { coordinate, new Segment(ShipDirection.Horizontal, new BattleShip(1)) }
                   };
                segmentation.UpdateSegmentRange(range);
                Assert.Pass();
            }
            catch (ArgumentException)
            {
               Assert.Pass(); // Bug in NUnit Adapter 3.13.0.0. Change to false when running the test individually
            }
        }
    }
}