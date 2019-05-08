namespace Battleship.Core.Components.Ships
{
    public sealed class Destroyer : ComponentBase, IShip
    {
        private readonly int shipLength = 4;

        private readonly char shipType = DestroyerCode;

        public Destroyer(int shipIndex)
        {
            this.ShipLength = shipLength;
            this.ShipChar = shipType;
            this.ShipIndex = shipIndex;
        }

        #region IShip Members

        public int ShipLength { get; }

        public char ShipChar { get; }

        public int ShipIndex { get; }

        public int CoordinateStatus { get; set; }

        #endregion
    }
}