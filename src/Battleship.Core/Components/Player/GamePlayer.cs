namespace Battleship.Core.Components.Player
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Battleship.Core.Components.Ships;
    using Battleship.Core.Models;
    using Battleship.Core.Repository;
    using Battleship.Core.Utilities;
    using Newtonsoft.Json;

    public class GamePlayer : ComponentBase, IGamePlayer
    {
        private static volatile GamePlayer instance;
        private readonly IGameRepository gameRepository;

        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        private readonly IShipRandomiser shipRandomiser;

        protected GamePlayer()
        {
            this.shipRandomiser = ShipRandomiser.Instance();
            this.gameRepository = GameRepository.Instance();
        }

        public async Task<string> CreatePlayer(string name, string surname, int numberOfShips)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname)) throw new ArgumentException();

            List<IShip> getRandomShips = BattleshipExtensions.GetRandomShips(numberOfShips);
            SortedDictionary<Coordinate, Segment> ships =
                this.shipRandomiser.GetRandomisedShipCoordinates(getRandomShips);

            // serialise to save into database

            string shipCoordinates =
                JsonConvert.SerializeObject(ships.ToArray(), Formatting.Indented, this.jsonSerializerSettings);
            return await this.gameRepository.CreatePlayer(name, surname, shipCoordinates);
        }

        public async Task<PlayerStats> UserInput(Coordinate coordinate, string sessionToken)
        {
            bool result = false;

            if (coordinate.X == 0 || coordinate.Y == 0 || string.IsNullOrEmpty(sessionToken))
                throw new ArgumentException();

            string coordinates = await this.gameRepository.GetPayerShipCoordinates(sessionToken);

            Dictionary<Coordinate, Segment> shipCoordinates = JsonConvert
                .DeserializeObject<KeyValuePair<Coordinate, Segment>[]>(coordinates, this.jsonSerializerSettings)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            string statistics = await this.gameRepository.GetPayerStatistics(sessionToken);
            PlayerStats playerStats = JsonConvert.DeserializeObject<PlayerStats>(statistics);

            KeyValuePair<Coordinate, Segment> shipCoordinate =
                shipCoordinates.FirstOrDefault(q => q.Key.X == coordinate.X && q.Key.Y == coordinate.Y);
            if (shipCoordinate.Value != null)
            {
                playerStats.Hit++;
                shipCoordinate.Value.Ship.CoordinateStatus++;
                result = true;
                int shipIndex = shipCoordinate.Value.Ship.ShipIndex;

                int shipLength = shipCoordinate.Value.Ship.ShipLength;
                int shipHitCounter = shipCoordinates.Where(q => q.Value.Ship.ShipIndex == shipIndex)
                    .Sum(q => q.Value.Ship.CoordinateStatus);

                if (shipLength == shipHitCounter)
                    playerStats.Sunk++;

                string updateShipCoordinates = JsonConvert.SerializeObject(shipCoordinates.ToArray(),
                    Formatting.Indented, this.jsonSerializerSettings);
                await this.gameRepository.UpdatePlayerShipCoordinates(updateShipCoordinates, sessionToken);
            }
            else
            {
                playerStats.Miss++;
            }

            playerStats.Status = result;

            string serializePlayerStats = JsonConvert.SerializeObject(playerStats);
            await this.gameRepository.UpdatePlayerStatistics(sessionToken, serializePlayerStats);

            return playerStats;
        }

        public bool IsPlayerValid(string token)
        {
            return this.gameRepository.IsPlayerValid(token);
        }

        public static GamePlayer Instance()
        {
            if (instance == null)
                lock (SyncObject)
                {
                    if (instance == null) instance = new GamePlayer();
                }

            return instance;
        }
    }
}