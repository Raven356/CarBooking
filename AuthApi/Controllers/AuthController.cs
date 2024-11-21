using AuthApi.Mappers;
using AuthApi.Models;
using AuthBLL.Interfaces;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService authService;
        private readonly ITokenService tokenService;

        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            this.authService = authService;
            this.tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(UserModel user)
        {
            authService.Register(AuthMapper.Map(user));

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginModel user) 
        {
            if (authService.Login(AuthMapper.Map(user)))
            {
                var accessToken = await tokenService.CreateAccessToken(AuthMapper.Map(user));

                return Ok(new { token = accessToken.Token });
            }

            return Unauthorized();
        }
    }
}
