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
        private readonly ILogger<OrderController> logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            this.orderService = orderService;
            this.logger = logger;
        }

        [HttpGet("GetByUserId")]
        public async Task<IActionResult> GetByUserId([FromQuery] int userId)
        {
            try
            {
                logger.LogInformation("Getting orders by user id");
                var orders = await orderService.GetRentOrdersByUserIdAsync(userId);

                return Json(orders);
            }
            catch (Exception ex) 
            {
                logger.LogError($"Error happened while getting orders by userId, error: {ex.Message}");
                return BadRequest(ex);
            }
        }

        [HttpGet("GetOrderById")]
        public async Task<IActionResult> GetById([FromQuery] int orderId)
        {
            try
            {
                logger.LogInformation($"Getting order by id, orderId: {orderId}");
                var order = await orderService.GetById(orderId);

                if (order == null)
                {
                    logger.LogInformation($"Order with id: {orderId} was not found!");
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
                    CarId = car.CarId,
                    Image = car.Image
                };

                return Ok(response);
            }
            catch (Exception ex) 
            {
                logger.LogError($"Error happened while getting order by id!");
                return BadRequest(ex);
            }
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest createOrderRequest)
        {
            try
            {
                logger.LogInformation("Started creating order");
                var orderEvent = new OrderStartedEvent { CarId = createOrderRequest.CarId, UserId = createOrderRequest.UserId };

                var rabbitMQPublisher = HttpContext.RequestServices.GetRequiredService<OrderEventsTimeoutPublisher>();
                rabbitMQPublisher.PublishWithTimeout(orderEvent, "create_order", (int)(createOrderRequest.DateFrom - DateTime.UtcNow).TotalMilliseconds);

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

                logger.LogInformation("Order created successfully");
                return Ok();
            }
            catch (Exception ex) 
            {
                logger.LogError($"Error happened while creating an order, exception: {ex.Message}");
                return BadRequest(ex);
            }
        }

        [HttpPost("EndOrder")]
        public async Task<IActionResult> EndOrder(EndOrderRequest endOrderRequest)
        {
            try
            {
                logger.LogInformation("Started ending order");
                var order = await orderService.GetById(endOrderRequest.OrderId);

                var endOrderEvent = new EndOrderEvent { CarId = order.RentInfo.CarId };
                var rabbitMQPublisher = HttpContext.RequestServices.GetRequiredService<OrderEventsPublisher>();
                rabbitMQPublisher.Publish(endOrderEvent, "end_order");

                await orderService.EndOrderAsync(endOrderRequest.OrderId, endOrderRequest.FinishedTime);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error happened while ending an order, error: {ex.Message}");
                return BadRequest(ex);
            }
        }

        [HttpPost("EditOrder")]
        public async Task<IActionResult> EditOrder(EditOrderRequest editOrderRequest)
        {
            try
            {
                logger.LogInformation("Started editing order");
                var newOrder = await orderService.EditOrderAsync(editOrderRequest.Id, editOrderRequest.DateFrom, editOrderRequest.DateTo);

                var orderEvent = new OrderStartedEvent { CarId = newOrder.RentInfo.CarId, UserId = newOrder.RentInfo.RentBy };
                ChangeTimedHistoryOrderEvent(orderEvent);
                PublishTimedHistoryEvent(orderEvent);

                var rabbitMQPublisher = HttpContext.RequestServices.GetRequiredService<OrderEventsTimeoutPublisher>();
                rabbitMQPublisher.PublishWithTimeout(orderEvent, "create_order", (int)(editOrderRequest.DateFrom - DateTime.UtcNow).TotalMilliseconds);

                logger.LogInformation("Order edited successfully");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error happened while editing order, error: {ex.Message}");
                return BadRequest(ex);
            }
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
