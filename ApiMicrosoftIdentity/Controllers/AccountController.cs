using ApiMicrosoftIdentity.Dto.Users;
using ApiMicrosoftIdentity.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMicrosoftIdentity.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(
            ILogger<AccountController> logger,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        //[HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    var rng = new Random();
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        [HttpPost]
        [Route("Login")]
        public async Task<UserDto> Login([FromBody] UserRegister user)
        {
            var userToLogin = await _userManager.FindByNameAsync(user.Username);

            if(user == null)
            {
                //login
                var signInResult = await _signInManager.PasswordSignInAsync(userToLogin, user.Password, false, false);

                if (signInResult.Succeeded)
                {
                    return new UserDto() { Username = userToLogin.UserName };
                }
            }
            
            throw new Exception("User not found");
        }

        [HttpPost]
        [Route("Register")]
        public async Task<UserDto> Register([FromBody] UserRegister user)
        {
            var userToRegister = new IdentityUser
            {
                UserName = user.Username
            };

            var result = await _userManager.CreateAsync(userToRegister, user.Password);
            
            if(result.Succeeded)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(userToRegister, user.Password, false, false);

                if (signInResult.Succeeded)
                {
                    return new UserDto() { Username = userToRegister.UserName };
                }
                else
                {
                    throw new Exception("Enable to login");
                }
            }
            else
            {
                throw new Exception("Enable to register");
            }
        }

        [HttpPost]
        [Route("LogOut")]
        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
