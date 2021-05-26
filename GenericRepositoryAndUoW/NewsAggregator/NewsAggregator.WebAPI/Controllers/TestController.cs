using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace NewsAggregator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        [HttpGet("getdefault")]
        public IActionResult GetDefault()
        {
            return Ok("Authorized User");
        }

        [HttpGet("getAdmin")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdmin()
        {
            return Ok("Admin User");
        }

        [HttpGet("GetUnauthorized")]
        [AllowAnonymous]
        public IActionResult GetUnauthorized()
        {
            return Ok("unauthorized User");
        }
    }
}
