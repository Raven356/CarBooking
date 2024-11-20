using EfficiencyBLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EfficiencyApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EfficiencyController : Controller
    {
        private readonly IEfficiencyService efficiencyService;

        public EfficiencyController(IEfficiencyService efficiencyService)
        {
            this.efficiencyService = efficiencyService;
        }

        [HttpGet("GetProfitsByCarId")]
        public async Task<IActionResult> GetProfitsByCarId([FromQuery] int carId)
        {
            var profits = await efficiencyService.GetProfitsByCarAsync(carId);

            return Json(profits);
        }
    }
}
