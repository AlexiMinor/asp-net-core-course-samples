using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NewsAggregator.WebAPI.Auth;

namespace NewsAggregator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtAuthManager _jwtAuthManager;

        public TokenController(IJwtAuthManager jwtAuthManager)
        {
            _jwtAuthManager = jwtAuthManager;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody]LoginRequest request)
        {
            JwtAuthResult jwtResult;
            if (request.Email=="admin@ema.il")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Email, request.Email),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                jwtResult = _jwtAuthManager.GenerateTokens(request.Email, claims);
            }
            else
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Email, request.Email),
                    new Claim(ClaimTypes.Role, "User")
                };
                jwtResult = _jwtAuthManager.GenerateTokens(request.Email, claims);
            }
            return Ok(jwtResult);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RefreshToken")]
        public IActionResult RefreshToken([FromBody]string refresh)
        {
            JwtAuthResult jwtResult=null;
            //if (request.Email == "admin@ema.il")
            //{
            //    var claims = new[]
            //    {
            //        new Claim(ClaimTypes.Email, request.Email),
            //        new Claim(ClaimTypes.Role, "Admin")
            //    };

            //    jwtResult = _jwtAuthManager.GenerateTokens(request.Email, claims);
            //}
            //else
            //{
            //    var claims = new[]
            //    {
            //        new Claim(ClaimTypes.Email, request.Email),
            //        new Claim(ClaimTypes.Role, "User")
            //    };
            //    jwtResult = _jwtAuthManager.GenerateTokens(request.Email, claims);
            //}
            return Ok(jwtResult);
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
