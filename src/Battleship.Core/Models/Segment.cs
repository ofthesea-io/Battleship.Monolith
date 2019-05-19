namespace Battleship.Core.Models
{
    using Battleship.Core.Components.Ships;
    using Battleship.Core.Enums;
    using Newtonsoft.Json;

    /// <summary>
    ///     Individual segment (cell) in the segmentation list on the grid
    /// </summary>
    public class Segment
    {
        public Segment(char character)
        {
            this.IsEmpty = true;
            this.Character = character;
        }

        [JsonConstructor]
        public Segment(ShipDirection shipDirection, IShip ship)
        {
            this.ShipDirection = shipDirection;
            this.Ship = ship;
            this.IsEmpty = false;
        }

        public bool IsEmpty { get; set; }

        public char Character { get; set; }

        public ShipDirection ShipDirection { get; set; }

        public IShip Ship { get; set; }
    }
}