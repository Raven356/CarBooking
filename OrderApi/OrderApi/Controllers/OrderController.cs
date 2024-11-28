using Microsoft.AspNetCore.Mvc;
using OrderApi.EventPublisher;
using OrderApi.Models.Responses;
using OrderBLL.Interfaces;
using RabbitMqLibrary.Events;
using System.Text.Json;

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

        [HttpGet("GetOrderById")]
        public async Task<IActionResult> GetById([FromQuery] int orderId)
        {
            var order = await orderService.GetById(orderId);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            var rpcPublisher = HttpContext.RequestServices.GetRequiredService<OrderEventsPublisher>(); // Replace with dependency injection
            var carRequest = new GetOrderWithCarEvent { CarId = order.RentInfo.CarId };

            var carResponse = await rpcPublisher.PublishWithReply(carRequest, "car_service_queue");

            var car = JsonSerializer.Deserialize<CarResponse>(carResponse);

            var response = new OrderCarResponse
            {
                Id = orderId,
                CarPlate = car.CarPlate,
                IsAcepted = order.IsAcepted,
                RentBy = order.RentInfo.RentBy,
                RentFromUTC = order.RentInfo.RentFromUTC,
                RentToUTC = order.RentInfo.RentToUTC,
                //CarImage = new byte[] {1, 1}
            };

            return Ok(response);
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
