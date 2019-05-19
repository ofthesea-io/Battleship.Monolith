namespace Battleship.Core.Components.Board
{
    /// <summary>
    ///     The gaming board for battle ships
    /// </summary>
    public interface IGridGenerator
    {
        int? NumberOfSegments { get; set; }

        int? NumberOfOccupiedSegments { get; set; }
        int[] GetNumericRows();

        string[] GetAlphaColumnChars();
    }
}