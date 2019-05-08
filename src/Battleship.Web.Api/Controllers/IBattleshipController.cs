namespace Battleship.Web.Api.Controllers
{
    using System.Threading.Tasks;
    using Battleship.Core.Models;
    using Microsoft.AspNetCore.Mvc;

    public interface IBattleshipController
    {
        ActionResult<GamingGrid> GetGamingGrid();
        Task<ActionResult> StartGame([FromBody] Player player);
        Task<ActionResult<PlayerStats>> UserInput([FromBody] Coordinate coordinate);
    }
}