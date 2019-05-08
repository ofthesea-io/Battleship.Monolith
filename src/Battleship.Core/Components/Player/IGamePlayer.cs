using Battleship.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Core.Components.Player
{
    public interface IGamePlayer
    {
        Task<string> CreatePlayer(string name, string surname, int numberOfShips);

        Task<PlayerStats> UserInput(Coordinate coordinate, string sessionToken);

        bool IsPlayerValid(string token);
    }
}
