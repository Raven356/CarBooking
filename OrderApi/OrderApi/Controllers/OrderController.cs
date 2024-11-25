using Microsoft.AspNetCore.Mvc;
using OrderApi.EventPublisher;
using OrderBLL.Interfaces;
using RabbitMqLibrary.Events;

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

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromQuery] int carId, [FromQuery] int userId)
        {
            var orderEvent = new OrderStartedEvent { CarId = carId, UserId = userId };
            var rabbitMQPublisher = HttpContext.RequestServices.GetRequiredService<OrderEventsPublisher>();
            rabbitMQPublisher.Publish(orderEvent);

            return Ok();
        }
    }
}
