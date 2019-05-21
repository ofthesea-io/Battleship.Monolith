namespace Battleship.Core.Components.Ships
{
    public sealed class BattleShip : ComponentBase, IShip
    {
        private readonly int shipLength = 5;

        private readonly char shipType = BattleShipCode;

        public BattleShip(int shipIndex)
        {
            this.ShipLength = this.shipLength;
            this.ShipChar = this.shipType;
            this.ShipIndex = shipIndex;
        }

        #region IShip Members

        public int ShipIndex { get; }

        public int ShipLength { get; }

        public char ShipChar { get; }

        public int CoordinateStatus { get; set; }

        #endregion
    }
}