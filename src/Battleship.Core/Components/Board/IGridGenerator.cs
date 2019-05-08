namespace Battleship.Core.Components.Board
{

    /// <summary>
    /// The gaming board for battle ships
    /// </summary>
    public interface IGridGenerator
    {
         int[] GetNumericRows();

         string[] GetAlphaColumnChars();

         int? NumberOfSegments { get; set; }

         int? NumberOfOccupiedSegments { get; set; }
    }
}