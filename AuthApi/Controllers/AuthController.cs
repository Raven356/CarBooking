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
        private readonly ILogger<AuthController> logger;

        public AuthController(IAuthService authService, ITokenService tokenService, ILogger<AuthController> logger)
        {
            this.authService = authService;
            this.tokenService = tokenService;
            this.logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterModel user)
        {
            try
            {
                await authService.Register(AuthMapper.Map(user));

                logger.LogInformation("User registered successfully!");
                return Ok();
            }
            catch (Exception ex) 
            {
                logger.LogError($"Something went wrong when registering user, error: {ex.Message}");
                return BadRequest(ex);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginModel user) 
        {
            if (await authService.Login(AuthMapper.Map(user)))
            {
                var accessToken = await tokenService.CreateAccessToken(AuthMapper.Map(user));

                logger.LogInformation("Logged in successfully!");
                return Ok(new { token = accessToken.Token });
            }

            logger.Log(LogLevel.Information, "Provided user credentials were wrong!");
            return Unauthorized();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromQuery] int userId)
        {
            await authService.Logout(userId);

            logger.LogInformation("User logouted successfully!");
            return Ok();
        }
    }
}
