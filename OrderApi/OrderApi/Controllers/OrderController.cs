using Microsoft.AspNetCore.Mvc;
using OrderApi.EventPublisher;
using OrderApi.Models.Requests;
using OrderApi.Models.Responses;
using OrderBLL.Interfaces;
using RabbitMqLibrary.Events;
using RabbitMqLibrary.TimedRoutine;
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

            var rpcPublisher = HttpContext.RequestServices.GetRequiredService<OrderEventsPublisher>();
            var carRequest = new GetOrderWithCarEvent { CarId = order.RentInfo.CarId };

            var carResponse = await rpcPublisher.PublishWithReply(carRequest, "car_service_queue");

            var car = JsonSerializer.Deserialize<CarResponse>(carResponse);

            var response = new OrderCarResponse
            {
                Id = orderId,
                CarPlate = car.CarPlate,
                IsAcepted = order.IsAcepted,
                RentFromUTC = order.RentInfo.RentFromUTC,
                RentToUTC = order.RentInfo.RentToUTC,
                RentFinished = order.RentInfo.RentFinished,
                CarId = car.CarId
                //CarImage = new byte[] {1, 1}
            };

            return Ok(response);
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest createOrderRequest)
        {
            var orderEvent = new OrderStartedEvent { CarId = createOrderRequest.CarId, UserId = createOrderRequest.UserId };

            var rabbitMQPublisher = HttpContext.RequestServices.GetRequiredService<OrderEventsTimeoutPublisher>();
            //rabbitMQPublisher.PublishWithTimeout(orderEvent, "create_order", (int)(createOrderRequest.DateFrom - DateTime.UtcNow).TotalMilliseconds);
            rabbitMQPublisher.PublishWithTimeout(orderEvent, "create_order", 60000);

            PublishTimedHistoryEvent(orderEvent);

            var order = new OrderBLL.Models.RentOrder
            {
                IsAcepted = true,
                RentInfo = new OrderBLL.Models.RentInfo
                {
                    CarId = createOrderRequest.CarId,
                    RentBy = createOrderRequest.UserId,
                    RentFromUTC = createOrderRequest.DateFrom,
                    RentToUTC = createOrderRequest.DateTo
                }
            };

            await orderService.SaveOrderAsync(order);

            return Ok();
        }

        [HttpPost("EndOrder")]
        public async Task<IActionResult> EndOrder(EndOrderRequest endOrderRequest)
        {
            var order = await orderService.GetById(endOrderRequest.OrderId);

            var endOrderEvent = new EndOrderEvent { CarId = order.RentInfo.CarId };
            var rabbitMQPublisher = HttpContext.RequestServices.GetRequiredService<OrderEventsPublisher>();
            rabbitMQPublisher.Publish(endOrderEvent, "end_order");

            await orderService.EndOrderAsync(endOrderRequest.OrderId, endOrderRequest.FinishedTime);

            return Ok();
        }

        [HttpPost("EditOrder")]
        public async Task<IActionResult> EditOrder(EditOrderRequest editOrderRequest)
        {
            var newOrder = await orderService.EditOrderAsync(editOrderRequest.Id, editOrderRequest.DateFrom, editOrderRequest.DateTo);

            var orderEvent = new OrderStartedEvent { CarId = newOrder.RentInfo.CarId, UserId = newOrder.RentInfo.RentBy };
            ChangeTimedHistoryOrderEvent(orderEvent);
            PublishTimedHistoryEvent(orderEvent);

            var rabbitMQPublisher = HttpContext.RequestServices.GetRequiredService<OrderEventsTimeoutPublisher>();
            //rabbitMQPublisher.PublishWithTimeout(orderEvent, "create_order", (int)(createOrderRequest.DateFrom - DateTime.UtcNow).TotalMilliseconds);
            rabbitMQPublisher.PublishWithTimeout(orderEvent, "create_order", 5000);

            return Ok();
        }

        private void ChangeTimedHistoryOrderEvent(BaseEvent @event)
        {
            var timeEventsHistoryPublisher = HttpContext.RequestServices.GetRequiredService<TimedEventHistoryPublisher>();

            var makeUnhandledEvent = new MakeTimedEventNotHandleEvent
            {
                Event = @event,
            };

            timeEventsHistoryPublisher.Publish(makeUnhandledEvent, "make_not_consumed");
        }

        private void PublishTimedHistoryEvent(BaseEvent @event)
        {
            var timeEventsHistoryPublisher = HttpContext.RequestServices.GetRequiredService<TimedEventHistoryPublisher>();
            var timeHistoryEvent = new TimedHistoryEvent
            {
                Action = new RabbitMqLibrary.TimedEventAction
                {
                    Event = @event,
                    ShouldBeHandled = true
                }
            };

            timeEventsHistoryPublisher.Publish(timeHistoryEvent, "add_history");
        }
    }
}
