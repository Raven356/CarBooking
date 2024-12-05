using AuthApi.Mappers;
using AuthApi.Models;
using AuthBLL.Interfaces;
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
        public async Task<IActionResult> Register(UserRegisterModel user)
        {
            await authService.Register(AuthMapper.Map(user));

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginModel user) 
        {
            if (await authService.Login(AuthMapper.Map(user)))
            {
                var accessToken = await tokenService.CreateAccessToken(AuthMapper.Map(user));

                return Ok(new { token = accessToken.Token });
            }

            return Unauthorized();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromQuery] int userId)
        {
            await authService.Logout(userId);

            return Ok();
        }
    }
}
