using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.Models.ViewModels.Account;
using NewsAggregators.Services.Implementation;

namespace NewsAggregator.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AccountController(IUserService userService, 
            IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var passValidationMessage = PasswordValidation(model.Password);

            if (ModelState.IsValid && string.IsNullOrEmpty(passValidationMessage))
            {
                var passwordHash = _userService.GetPasswordHash(model.Password);

                var result = await _userService.RegisterUser(new UserDto()
                {
                    Id = Guid.NewGuid(),
                    Email = model.Email,
                    PasswordHash = passwordHash
                });

                if (result)
                {
                    return RedirectToAction("Index", "Home");
                }
                return BadRequest(model);
            }

            ModelState.AddModelError("", passValidationMessage);

            return View(model);
        }

        private string PasswordValidation(string password)
        {
            //todo logic
            return "1231231";
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            return (await _userService.GetUserByEmail(email)) != null 
                ? Json(false) 
                : Json(true);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var model = new LoginViewModel() {ReturnUrl = returnUrl};
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userFromDb = await _userService.GetUserByEmail(model.Email);
                if (userFromDb != null)
                {
                    var passwordHash = _userService.GetPasswordHash(model.Password);
                    if (passwordHash.Equals(userFromDb.PasswordHash))
                    {
                        await Authenticate(userFromDb);

                        return string.IsNullOrEmpty(model.ReturnUrl)
                            ? (IActionResult) RedirectToAction("Index", "Home")
                            : Redirect(model.ReturnUrl);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

      

        private async Task Authenticate(UserDto dto)
        {
            try
            {
                const string authType = "ApplicationCookie";
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, dto.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, (await _roleService.GetUserRoles(dto.Email)).Name),
                    new Claim("age", dto.Age.ToString()),
                };

                var identity = new ClaimsIdentity(claims,
                    authType,
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            //Claim & ClaimsIdentity & ClaimsPrinciple
           
        }
    }
}
