namespace Battleship.Web.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Battleship.Core.Components.Board;
    using Battleship.Core.Components.Player;
    using Battleship.Core.Models;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    /// <summary>
    ///     The game board generation
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BattleshipController : ControllerBase, IBattleshipController
    {
        private readonly IGamePlayer gamePlayer;
        private readonly IGridGenerator gridGenerator;

        public BattleshipController()
        {
            this.gridGenerator = GridGenerator.Instance();
            this.gamePlayer = GamePlayer.Instance();
        }

        [HttpGet]
        [Route("GetGamingGrid")]
        public ActionResult<GamingGrid> GetGamingGrid()
        {
            try
            {
                if (!this.CheckPlayerStatus(this.HttpContext.Request.Headers["Authorization"]))
                    return this.BadRequest("Authentication failed");

                GamingGrid gamingGrid = new GamingGrid
                {
                    X = this.GetXAxis(),
                    Y = this.GetYAxis()
                };

                return gamingGrid;
            }
            catch (Exception)
            {
                return this.BadRequest();
            }
        }

        [HttpPost]
        [Route("StartGame")]
        public async Task<ActionResult> StartGame([FromBody] Player player)
        {
            try
            {
                string session =
                    await this.gamePlayer.CreatePlayer(player.Firstname, player.Lastname, player.NumberOfShips);
                if (string.IsNullOrEmpty(session)) return this.BadRequest(false);

                string token = JsonConvert.SerializeObject(new {token = session});
                return this.Ok(token);
            }
            catch (Exception exp)
            {
                return this.BadRequest(exp.Message);
            }
        }

        [HttpPost]
        [Route("UserInput")]
        public async Task<ActionResult<PlayerStats>> UserInput([FromBody] Coordinate coordinate)
        {
            try
            {
                string sessionToken = this.HttpContext.Request.Headers["Authorization"];
                PlayerStats playerStats = await this.gamePlayer.UserInput(coordinate, sessionToken);

                return playerStats;
            }
            catch (Exception)
            {
                return this.BadRequest();
            }
        }

        private IEnumerable<string> GetXAxis()
        {
            try
            {
                return this.gridGenerator.GetAlphaColumnChars();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private IEnumerable<int> GetYAxis()
        {
            try
            {
                return this.gridGenerator.GetNumericRows();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private bool CheckPlayerStatus(string token)
        {
            //return gamePlayer.IsPlayerValid(this.HttpContext.Request.Headers["Authorization"]);
            return true;
        }
    }
}