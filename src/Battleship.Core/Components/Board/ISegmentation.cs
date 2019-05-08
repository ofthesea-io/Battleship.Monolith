namespace Battleship.Core.Components.Board
{
    using System.Collections.Generic;

    using Battleship.Core.Models;

    /// <summary>
    /// List of segments Controls each individual segment on the grid.
    /// </summary>
    public interface ISegmentation
    {
        /// <summary>
        /// Add a segment to the segmentation grid
        /// </summary>
        /// <param name="coordinate">Coordinate of the grid</param>
        /// <param name="segment">Segment of the grid</param>
        void AddSegment(Coordinate coordinate, Segment segment);

        /// <summary>
        /// Update a segment in the segmentation grid
        /// </summary>
        /// <param name="coordinate">Coordinate of the grid</param>
        /// <param name="segment">A segment within the grid</param>
        void UpdateSegment(Coordinate coordinate, Segment segment);

        /// <summary>
        /// Update a range of segments in the segmentation grid
        /// </summary>
        /// <param name="segment">A list of segment within the grid</param>
        void UpdateSegmentRange(SortedDictionary<Coordinate, Segment> segment);

        /// <summary>
        ///     Get the list of segments in the segmentation list
        /// </summary>
        /// <returns>A list of Segments</returns>
        SortedDictionary<Coordinate, Segment> GetSegments();

        /// <summary>
        ///     Gets a single segment
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>Returns a single segment</returns>
        Segment GetSegment(int x, int y);
    }
}