using Battleship.Core.Repository;

namespace Battleship.Core.Components.Board
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Battleship.Core.Components.Ships;
    using Battleship.Core.Models;

    public class GridGenerator : ComponentBase, IGridGenerator
    {

        private readonly ISegmentation segmentation;

        private readonly IShipRandomiser shipRandomiser;

        private readonly List<IShip> ships;

        private static volatile GridGenerator instance;

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

        public int? NumberOfSegments { get;  set; }

        public int? NumberOfOccupiedSegments { get;  set; }

        #endregion

        #region Methods


        public static GridGenerator Instance()
        {
            if (instance == null)
            {
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
            }

            return instance;
        }


        public int[] GetNumericRows()
        {

            GameRepository gameRepository = GameRepository.Instance();


            int[] row = new int[GridDimension];
            int counter = 0;
            for (int i = 1; i <= GridDimension; i++)
            {
                row[counter] = i;
                counter++;
            }

            return row;
        }

        public string[] GetAlphaColumnChars()
        {
            int xDimention = XInitialPoint + GridDimension;
            string[] column = new string[GridDimension];
            int counter = 0;
            for (int i = XInitialPoint; i < xDimention; i++)
            {
                column[counter] = ((char)i).ToString();
                counter++;
            }

            return column;
        }


        private void CreateSegmentationGrid()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            boardLeft = 4;
            Console.SetCursorPosition(boardLeft, boardTop);

            int yCounter = 1;
            while (yCounter <= GridDimension)
            {
                for (int xCounter = 0; xCounter <= GridDimension - Index; xCounter++)
                {
                    Segment segment = new Segment(Water);
                    Coordinate coordinates = new Coordinate(XInitialPoint + xCounter, yCounter);
                    segmentation.AddSegment(coordinates, segment);
                }

                boardTop++;
                yCounter++;

                Console.SetCursorPosition(boardLeft, boardTop);
            }

            this.NumberOfSegments = segmentation.GetSegments().Count();

            // update the board with randomly generated ship coordinates
            this.UpdateSegmentationGridWithShips();
        }

        private void UpdateSegmentationGridWithShips()
        {
            SortedDictionary<Coordinate, Segment> segments = shipRandomiser.GetRandomisedShipCoordinates(ships);

            segmentation.UpdateSegmentRange(segments);

            this.NumberOfOccupiedSegments = segmentation.GetSegments().Count(q => !q.Value.IsEmpty);
        }

  

        #endregion
    }
}