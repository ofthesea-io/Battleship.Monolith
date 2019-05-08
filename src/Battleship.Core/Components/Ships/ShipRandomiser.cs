namespace Battleship.Core.Components.Ships
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Enums;
    using Models;
    using Utilities;

    public class ShipRandomiser : ComponentBase,  IShipRandomiser
    {
        private static readonly Random Randomise = new Random();

        private static volatile ShipRandomiser instance;

        private readonly SortedDictionary<Coordinate, Segment> segments;

        private readonly int xMidPoint;

        private readonly int yMidPoint;

        private Coordinate coordinate;

        protected ShipRandomiser()
        {
            segments = new SortedDictionary<Coordinate, Segment>(new CoordinateComparer());

            yMidPoint = GridDimension / 2;
            xMidPoint = (XInitialPoint + GridDimension / 2) - Index;
        }

        public static ShipRandomiser Instance()
        {
            if (instance == null)
            {
                lock (SyncObject)
                {
                    if (instance == null)
                    {
                        instance = new ShipRandomiser();
                    }
                }
            }

            return instance;
        }

        #region IShipRandomiser Members

        public SortedDictionary<Coordinate, Segment> GetRandomisedShipCoordinates(IList<IShip> ships)
        {
            if (ships != null && segments != null)
            {
                int totalShipLength = ships.Sum(q => q.ShipLength);

                // Create a temporary segment list and pass it along by reference
                // Once done, we can clear it out and make sure the GC does its job
                SortedDictionary<Coordinate, Segment> temporarySegments = new SortedDictionary<Coordinate, Segment>(new CoordinateComparer());

                segments.Clear();

                if (totalShipLength != segments.Count)
                {
                    foreach (IShip ship in ships)
                    {
                        ShipDirection direction = Randomise.Next(this.GridDimension) % 2 == 0
                                                      ? ShipDirection.Horizontal
                                                      : ShipDirection.Vertical;

                        while (temporarySegments != null && temporarySegments.Count != ship.ShipLength)
                        {
                            if (direction == ShipDirection.Horizontal)
                            {
                                this.MapXAxis(ship, ref temporarySegments);
                                if (temporarySegments.Count == ship.ShipLength)
                                {
                                    segments.AddRange(temporarySegments);
                                    temporarySegments.Clear();
                                    break;
                                }
                            }

                            if (direction == ShipDirection.Vertical)
                            { 
                                this.MapYAxis(ship, ref temporarySegments);

                                if (temporarySegments != null && temporarySegments.Count == ship.ShipLength)
                                {
                                    segments.AddRange(temporarySegments);
                                    temporarySegments.Clear();
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return segments;
        }

        #endregion

        private void MapYAxis(IShip ship, ref SortedDictionary<Coordinate, Segment> temporarySegments)
        {
            coordinate = this.GenerateCoordinate();

            // Top is 1 to 5 and Bottom is 5 to 10 
            Boundaries orientation = coordinate.Y > yMidPoint ? Boundaries.Top : Boundaries.Bottom;

            if (orientation == Boundaries.Top)
            {
                for (int y = coordinate.Y; y > coordinate.Y - ship.ShipLength; y--)
                {
                    if (!this.AddToYAxis(coordinate.X, y, ship, ref temporarySegments))
                    {
                        break;
                    }
                }
            }

            if (orientation == Boundaries.Bottom)
            {
                for (int y = coordinate.Y; y < coordinate.Y + ship.ShipLength; y++)
                {
                    if (!this.AddToYAxis(coordinate.X, y, ship, ref temporarySegments))
                    {
                        break;
                    }
                }
            }
        }

        private void MapXAxis(IShip ship, ref SortedDictionary<Coordinate, Segment> temporarySegments)
        {
            coordinate = this.GenerateCoordinate();

            // Left to Right 65 to 69 Right to Left 74 to 70
            Boundaries orientation = coordinate.X >= xMidPoint ? Boundaries.RightToLeft : Boundaries.LeftToRight;

            if (orientation == Boundaries.LeftToRight)
            {
                for (int x = coordinate.X; x < coordinate.X + ship.ShipLength; x++)
                {
                    if (!this.AddToXAxis(x, coordinate.Y, ship, ref temporarySegments))
                    {
                        break;
                    }
                }
            }

            if (orientation == Boundaries.RightToLeft)
            {
                for (int x = coordinate.X; x > coordinate.X - ship.ShipLength; x--)
                {
                    if (!this.AddToXAxis(x, coordinate.Y, ship, ref temporarySegments))
                    {
                        break;
                    }
                }
            }
        }

        private bool AddToYAxis(int currentXPosition, int currentYPosition, IShip ship, ref SortedDictionary<Coordinate, Segment> temporarySegments)
        {
            bool result = false;

            // If the current segment position is valid add to the temporary list, otherwise clear the list and start again
            if (segments.IsSegmentAvailable(currentXPosition, currentYPosition)
                && BattleshipExtensions.IsSegmentWithInGridRange(currentXPosition, currentYPosition))
            {
                temporarySegments.Add(new Coordinate(currentXPosition, currentYPosition), new Segment(ShipDirection.Vertical, ship));
                result = true;
            }
            else
            {
                this.Clear(temporarySegments);
            }

            return result;
        }

        private bool AddToXAxis(int currentXPosition, int currentYPosition, IShip ship, ref SortedDictionary<Coordinate, Segment> temporarySegments)
        {
            bool result = false;

            // If the current segment position is valid add to the temporary list, otherwise clear the list and start again
            if (segments.IsSegmentAvailable(currentXPosition, currentYPosition)
                && BattleshipExtensions.IsSegmentWithInGridRange(currentXPosition, currentYPosition))
            {
                temporarySegments.Add(new Coordinate(currentXPosition, currentYPosition), new Segment(ShipDirection.Horizontal, ship));
                result = true;
            }
            else
            {
                this.Clear(temporarySegments);
            }

            return result;
        }

        private void Clear(SortedDictionary<Coordinate, Segment> temporarySegments)
        {
            temporarySegments.Clear();
        }

        private Coordinate GenerateCoordinate()
        {
            int positionX = Randomise.Next(XInitialPoint, XInitialPoint + GridDimension);
            int positionY = Randomise.Next(Index, this.GridDimension);

             // if we hit the xMidPoint seed and add/subtract to positionX
            if (positionX == xMidPoint)
            {
                int seed = Randomise.Next(XInitialPoint, xMidPoint);
                positionX = seed % 2 == 0 ? positionX + seed : positionX - seed;
            }

            // if we hit the yMidPoint seed and add/subtract to positionY
            if (positionY == yMidPoint)
            {
                int seed = Randomise.Next(Index, yMidPoint);
                positionY = seed % 2 == 0 ? positionY + seed : positionY - seed;
            }

            coordinate = new Coordinate(positionX, positionY);
            return coordinate;
        }
    }
}