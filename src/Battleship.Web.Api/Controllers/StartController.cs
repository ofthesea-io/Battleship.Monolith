namespace Battleship.Web.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class StartController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "Battleship API started.";
        }
    }
}