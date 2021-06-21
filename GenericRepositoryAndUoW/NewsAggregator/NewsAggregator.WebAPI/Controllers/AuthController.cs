using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.WebAPI.Auth;
using NewsAggregator.WebAPI.Requests;
using NewsAggregators.Services.Implementation;
using Serilog;

namespace NewsAggregator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthController(IJwtAuthManager jwtAuthManager, 
            IUserService userService, IRefreshTokenService refreshTokenService)
        {
            _jwtAuthManager = jwtAuthManager;
            _userService = userService;
            _refreshTokenService = refreshTokenService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (await _userService.GetUserByEmail(request.Email) != null)
                {
                    return BadRequest("User with this email already existed");
                }

                var passwordHash = _userService.GetPasswordHash(request.Password);

                var isRegistrationSucceed = await _userService.RegisterUser(new UserDto()
                {
                    Email = request.Email,
                    PasswordHash = passwordHash
                });

                if (isRegistrationSucceed)
                {
                    var jwtAuthResult = await GetJwt(request.Email);

                    return Ok(jwtAuthResult);
                }

                return BadRequest("Unsuccessful registration");
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (await _userService.GetUserByEmail(request.Email) == null)
            {
                return BadRequest("No user");
            }

            var passwordHash = _userService.GetPasswordHash(request.Password);

            if (await _userService.CheckAuthIsValid(new UserDto() { Email = request.Email, PasswordHash = passwordHash}))
            {
                var jwtAuthResult = await GetJwt(request.Email);
                return Ok(jwtAuthResult);
            }

            return BadRequest("Email or password is incorrect");
        }

        /// <summary>
        /// refresh token
        /// </summary>
        /// <param name="request">object includes refresh token string value</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            if (! await _refreshTokenService.CheckIsRefreshTokenIsValid(request.Token))
            {
                return BadRequest("Invalid Refresh Token");
            }

            var userEmail = await _userService.GetUserEmailByRefreshToken(request.Token);
            if (!string.IsNullOrEmpty(userEmail))
            {
                var jwtAuthResult = await GetJwt(userEmail);
                return Ok(jwtAuthResult);
            }

            return BadRequest("Email or password is incorrect");
        }

        private async Task<JwtAuthResult> GetJwt(string email)
        {
            JwtAuthResult jwtResult;
            var roleName = await _userService.GetUserRoleNameByEmail(email);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, roleName)
            };

            jwtResult = await _jwtAuthManager.GenerateTokens(email, claims);
            return jwtResult;
        }
    }
}
