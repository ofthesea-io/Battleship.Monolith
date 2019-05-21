namespace Battleship.Core.Components.Player
{
    using System.Threading.Tasks;
    using Battleship.Core.Models;

    public interface IGamePlayer
    {
        Task<string> CreatePlayer(string name, string surname, int numberOfShips);

        Task<PlayerStats> UserInput(Coordinate coordinate, string sessionToken);

        bool IsPlayerValid(string token);
    }
}