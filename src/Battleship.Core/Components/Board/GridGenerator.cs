namespace Battleship.Core.Components.Board
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Battleship.Core.Components.Ships;
    using Battleship.Core.Models;
    using Battleship.Core.Repository;

    public class GridGenerator : ComponentBase, IGridGenerator
    {
        private static volatile GridGenerator instance;

        private readonly ISegmentation segmentation;

        private readonly IShipRandomiser shipRandomiser;

        private readonly List<IShip> ships;

        private int boardLeft;

        private int boardTop;

        protected GridGenerator(
            ISegmentation segmentation,
            IShipRandomiser shipRandomiser,
            List<IShip> ships)
        {
            this.segmentation = segmentation;
            this.shipRandomiser = shipRandomiser;
            this.ships = ships;
        }


        #region Properties

        public int? NumberOfSegments { get; set; }

        public int? NumberOfOccupiedSegments { get; set; }

        #endregion

        #region Methods

        public static GridGenerator Instance()
        {
            if (instance == null)
                lock (SyncObject)
                {
                    if (instance == null)
                    {
                        ISegmentation segmentation = Segmentation.Instance();
                        IShipRandomiser shipRandomiser = ShipRandomiser.Instance();
                        List<IShip> ships = new List<IShip>();
                        instance = new GridGenerator(segmentation, shipRandomiser, ships);
                    }
                }

            return instance;
        }


        public int[] GetNumericRows()
        {
            GameRepository gameRepository = GameRepository.Instance();


            int[] row = new int[this.GridDimension];
            int counter = 0;
            for (int i = 1; i <= this.GridDimension; i++)
            {
                row[counter] = i;
                counter++;
            }

            return row;
        }

        public string[] GetAlphaColumnChars()
        {
            int xDimension = this.XInitialPoint + this.GridDimension;
            string[] column = new string[this.GridDimension];
            int counter = 0;
            for (int i = this.XInitialPoint; i < xDimension; i++)
            {
                column[counter] = ((char) i).ToString();
                counter++;
            }

            return column;
        }


        private void CreateSegmentationGrid()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            this.boardLeft = 4;
            Console.SetCursorPosition(this.boardLeft, this.boardTop);

            int yCounter = 1;
            while (yCounter <= this.GridDimension)
            {
                for (int xCounter = 0; xCounter <= this.GridDimension - this.Index; xCounter++)
                {
                    Segment segment = new Segment(Water);
                    Coordinate coordinates = new Coordinate(this.XInitialPoint + xCounter, yCounter);
                    this.segmentation.AddSegment(coordinates, segment);
                }

                this.boardTop++;
                yCounter++;

                Console.SetCursorPosition(this.boardLeft, this.boardTop);
            }

            this.NumberOfSegments = this.segmentation.GetSegments().Count();

            // update the board with randomly generated ship coordinates
            this.UpdateSegmentationGridWithShips();
        }

        private void UpdateSegmentationGridWithShips()
        {
            SortedDictionary<Coordinate, Segment> segments =
                this.shipRandomiser.GetRandomisedShipCoordinates(this.ships);

            this.segmentation.UpdateSegmentRange(segments);

            this.NumberOfOccupiedSegments = this.segmentation.GetSegments().Count(q => !q.Value.IsEmpty);
        }

        #endregion
    }
}