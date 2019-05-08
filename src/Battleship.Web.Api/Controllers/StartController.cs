using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Battleship.Web.Api.Controllers
{
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