using AuthBLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : Controller
    {
        private readonly ITokenService tokenService;

        public UserController(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        [HttpGet("GetUserIdByToken")]
        public IActionResult Index([FromQuery] string token)
        {
            if (token == null) 
            {
                return BadRequest();
            }

            int userId = tokenService.GetUserIdFromToken(token);
            return Ok(new { userId });
        }
    }
}
