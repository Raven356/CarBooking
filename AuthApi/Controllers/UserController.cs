using AuthApi.Mappers;
using AuthApi.Models;
using AuthBLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : Controller
    {
        private readonly ITokenService tokenService;
        private readonly IUserService userService;
        private readonly ILogger<UserController> logger;

        public UserController(ITokenService tokenService, IUserService userService, ILogger<UserController> logger)
        {
            this.tokenService = tokenService;
            this.userService = userService;
            this.logger = logger;
        }

        [HttpGet("GetUserIdByToken")]
        public IActionResult GetUserIdByToken([FromQuery] string token)
        {
            if (token == null) 
            {
                logger.LogInformation("Users token was empty");
                return Unauthorized();
            }

            int userId = tokenService.GetUserIdFromToken(token);
            return Ok(new { userId });
        }

        [HttpGet("GetUserByToken")]
        public async Task<IActionResult> GetUserByToken([FromQuery] string token)
        {
            if (token == null)
            {
                logger.LogInformation("Users token was empty");
                return Unauthorized();
            }

            int userId = tokenService.GetUserIdFromToken(token);

            var user = await userService.GetUserById(userId);

            return Ok(UserMapper.Map(user));
        }

        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById([FromQuery] int userId)
        {
            var user = await userService.GetUserById(userId);

            return Ok(UserMapper.Map(user));
        }

        [HttpPost("EditUser")]
        public async Task<IActionResult> EditUser([FromBody] EditUserRequest editUserRequest)
        {
            await userService.EditUserAsync(UserMapper.Map(editUserRequest));

            return Ok();
        }
    }
}
