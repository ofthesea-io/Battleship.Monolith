namespace Battleship.Core.Components
{
    /// <summary>
    ///     Base for the components and Test.
    ///     Try to not have any magic numbers in the game.
    /// </summary>
    public abstract class ComponentBase
    {
        protected const char Hit = '*';

        protected const char Miss = 'M';

        protected const char Water = '~';

        protected const char DestroyerCode = 'D';

        protected const char BattleShipCode = 'B';

        // using the double-checked locking, but we are in a single thread
        protected static readonly object SyncObject = new object();

        protected readonly int GridDimension = 12;

        protected readonly int Index = 1;

        protected readonly int XInitialPoint = 65;
    }
}