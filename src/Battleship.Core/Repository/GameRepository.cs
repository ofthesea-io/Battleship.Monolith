using System.Threading;

namespace Battleship.Core.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Battleship.Core.Models;
    using Components;
    using Microsoft.Data.Sqlite;
    using Newtonsoft.Json;

    internal class GameRepository : ComponentBase, IGameRepository
    {
        private static volatile GameRepository instance;

        private SqliteConnection databaseConnection = new SqliteConnection("Filename=../Data/Battleship.db");


        public static GameRepository Instance()
        {
            if (instance == null)
            {
                lock (SyncObject)
                {
                    if (instance == null)
                    {
                        instance = new GameRepository();
                        instance.Initialise();
                    }
                }
            }

            return instance;
        }


        public async Task<string> CreatePlayer(string name, string surname, string serialisedShips)
        {
            string authorisation = string.Empty;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) ||  string.IsNullOrEmpty(serialisedShips))
            {
                throw new ArgumentException();
            }

            using (databaseConnection)  
            {
                try {

                    databaseConnection.Open();

                    // create a 'session' token
                    string session = Guid.NewGuid().ToString();
                    var sessionBytes = System.Text.Encoding.UTF8.GetBytes(session);
                    string sessionToken = Convert.ToBase64String(sessionBytes);

                    string sessionExpiry = DateTime.Now.AddHours(2).ToString();

                    SqliteCommand playerCommand = new SqliteCommand();
                    playerCommand.Connection = databaseConnection;

                    // Use parameterized query to prevent SQL injection attacks
                    playerCommand.CommandText = "INSERT INTO Player(Firstname, Lastname, SessionToken, SessionExpiry,ShipCoordinates ) " +
                                                "VALUES (@Firstname, @Lastname, @SessionToken, @SessionExpiry, @ShipCoordinates);";
                    playerCommand.Parameters.AddWithValue("@Firstname", name);
                    playerCommand.Parameters.AddWithValue("@Lastname", surname);
                    playerCommand.Parameters.AddWithValue("@SessionToken", sessionToken);
                    playerCommand.Parameters.AddWithValue("@SessionExpiry", sessionExpiry);
                    playerCommand.Parameters.AddWithValue("@ShipCoordinates", serialisedShips);
                    await playerCommand.ExecuteReaderAsync();
                  
                    // insert the stats
                    SqliteCommand statisticsCommand = new SqliteCommand();
                    statisticsCommand.Connection = databaseConnection;
                    string statistics = JsonConvert.SerializeObject(new PlayerStats());
                    statisticsCommand.CommandText = "INSERT INTO PlayerStatistics(SessionToken, Statistics) " +
                                                "VALUES (@SessionToken, @Statistics);";
                    statisticsCommand.Parameters.AddWithValue("@SessionToken", sessionToken);
                    statisticsCommand.Parameters.AddWithValue("@Statistics", statistics);
                    await statisticsCommand.ExecuteReaderAsync();


                    databaseConnection.Close();
                    authorisation = sessionToken;

                }
                catch(Exception)
                {
                    authorisation = string.Empty;
                }

                return authorisation;
            }
        }

        public async Task<string> GetPayerStatistics(string session)
        {
            string coordinates = string.Empty;
            if (string.IsNullOrEmpty(session))
            {
                throw new ArgumentException();
            }
            try
            {
                using (databaseConnection)
                {
                    databaseConnection.Open();

                    string sql = $"select Statistics from PlayerStatistics where SessionToken = '{session}'";
                    SqliteCommand selectCommand = new SqliteCommand(sql, databaseConnection);
                    SqliteDataReader reader = await selectCommand.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        coordinates = reader.GetString(0);
                    }
                    reader.Close();
                    databaseConnection.Close();
                }
            }
            catch (Exception)
            {
                coordinates = string.Empty;
            }

            return coordinates;
        }

        public async Task UpdatePlayerShipCoordinates(string serialisedShips, string sessionToken)
        {
            try
            {
                using (databaseConnection)
                {
                    databaseConnection.Open();

                    string sql = $"UPDATE Player SET ShipCoordinates = '{serialisedShips}' where SessionToken = '{sessionToken}'";
                    SqliteCommand updateCommand = new SqliteCommand(sql, databaseConnection);
                    await updateCommand.ExecuteReaderAsync();

                    databaseConnection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdatePlayerStatistics(string session, string statistics)
        {
            try
            {
                using (databaseConnection)
                {
                    databaseConnection.Open();

                    string sql = $"UPDATE PlayerStatistics SET Statistics = '{statistics}' where SessionToken = '{session}'";
                    SqliteCommand updateCommand = new SqliteCommand(sql, databaseConnection);
                    await updateCommand.ExecuteReaderAsync();
                   
                    databaseConnection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SortedDictionary<Player,PlayerStats> GetPlayersDetails()
        {
            throw new NullReferenceException();
        }

        public bool IsPlayerValid(string sessionToken)
        {
            bool result = false;
            if (string.IsNullOrEmpty(sessionToken))
            {
                throw new ArgumentException();
            }

            try
            {
                using (databaseConnection)
                {
                    databaseConnection.Open();

                    string sql = $"SELECT * FROM Player where SessionToken = '{sessionToken}'";
                    SqliteCommand selectCommand = new SqliteCommand(sql, databaseConnection);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    if (query.HasRows)
                    {
                        result = true;
                    }
                }
                databaseConnection.Close();
            }
            catch (Exception)
            {
                result = false;
            }
            

            return result;
        }

        public async Task<string> GetPayerShipCoordinates(string sessionToken)
        {
            string coordinates = string.Empty;
            if (string.IsNullOrEmpty(sessionToken))
            {
                throw new ArgumentException();
            }
            try
            {
                using (databaseConnection)
                {
                    databaseConnection.Open();

                    string sql = $"SELECT ShipCoordinates FROM Player where SessionToken = '{sessionToken}'";
                    SqliteCommand selectCommand = new SqliteCommand(sql, databaseConnection);
                    SqliteDataReader query = await selectCommand.ExecuteReaderAsync();
                    if(query.HasRows)
                    {
                        coordinates = query.GetString(0);
                    }

                    databaseConnection.Close();
                }
            }
            catch (Exception)
            {
                coordinates = string.Empty;
            }

            return coordinates;
        }

        private void Initialise()
        {
            using (databaseConnection)
            {
                databaseConnection.Open();

                //should really be read in, but going to keep it simple for the moment
                string createdDatabase = @"BEGIN TRANSACTION;
                                        CREATE TABLE IF NOT EXISTS Player (
                                            SessionToken    TEXT NOT NULL,
                                            SessionExpiry   TEXT NOT NULL,
                                            Firstname       TEXT NOT NULL,
                                            Lastname        TEXT NOT NULL,
                                            ShipCoordinates BLOB NOT NULL
                                        );
                                         CREATE TABLE IF NOT EXISTS PlayerStatistics (
                                            SessionToken TEXT NOT NULL,
                                            Statistics   BLOB NOT NULL
                                        );
                                        COMMIT TRANSACTION;";

                SqliteCommand createTable = new SqliteCommand(createdDatabase, databaseConnection);

                createTable.ExecuteReader();

                databaseConnection.Close();
            }
        }
    }
}