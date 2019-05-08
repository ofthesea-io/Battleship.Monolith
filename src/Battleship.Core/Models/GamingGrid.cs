using System.Collections.Generic;

namespace Battleship.Core.Models
{
    public class GamingGrid
    {
        public IEnumerable<string> X { get; set; }

        public IEnumerable<int> Y { get; set; }
    }
}