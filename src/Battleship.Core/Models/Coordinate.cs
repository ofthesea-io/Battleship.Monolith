namespace Battleship.Core.Models
{
    /// <summary>
    ///     Coordinate on the graph. Decided against the Point2 struct.
    /// </summary>
    public class Coordinate
    {
        public Coordinate(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get; }

        public int Y { get; }
    }
}