namespace Battleship.Core.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Battleship.Core.Models;

    internal interface IGameRepository
    {
        Task<string> CreatePlayer(string name, string surname, string serialisedShips);

        Task<string> GetPayerShipCoordinates(string session);

        Task<string> GetPayerStatistics(string session);

        Task UpdatePlayerShipCoordinates(string serialisedShips, string sessionToken);

        Task UpdatePlayerStatistics(string session, string statistics);

        SortedDictionary<Player, PlayerStats> GetPlayersDetails();

        bool IsPlayerValid(string token);
    }
}