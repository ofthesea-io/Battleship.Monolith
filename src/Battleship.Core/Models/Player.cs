namespace Battleship.Core.Models
{
    using System;

    public class Player
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string SessionToken { get; set; }

        public DateTime? SessionExpiry { get; set; }

        public int NumberOfShips { get; set; }
    }
}