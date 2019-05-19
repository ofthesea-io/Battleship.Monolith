namespace Battleship.Core.Models
{
    using System.Collections.Generic;

    public class GamingGrid
    {
        public IEnumerable<string> X { get; set; }

        public IEnumerable<int> Y { get; set; }
    }
}