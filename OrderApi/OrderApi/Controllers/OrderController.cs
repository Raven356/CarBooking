using Microsoft.AspNetCore.Mvc;
using OrderBLL.Interfaces;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet("GetByUserId")]
        public async Task<IActionResult> GetByUserId([FromQuery] int userId)
        {
            var orders = await orderService.GetRentOrdersByUserIdAsync(userId);

            return Json(orders);
        }
    }
}
