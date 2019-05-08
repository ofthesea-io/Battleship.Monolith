namespace Battleship.Core.Components.Ships
{
    public interface IShip
    {
        // The length of the ship 
        int ShipLength { get; }

        // The type if char
        char ShipChar { get; }

        // Ship hit counter
        int CoordinateStatus { get; set; }

        // Ship Index
        int ShipIndex { get;  }
    }
}