using OrderApi.Mappers;
using OrderBLL.Interfaces;
using RabbitMQ.Client;
using RabbitMqLibrary;
using RabbitMqLibrary.Events;
using System.Text;
using System.Text.Json;

namespace OrderApi.EventConsumers
{
    public class OrderRpcConsumer : RabbitMQConsumerBase<GetUserOrdersEvent>
    {
        private readonly IModel _channel;
        private readonly IOrderService orderService;
        private readonly ILogger<OrderRpcConsumer> logger;

        public OrderRpcConsumer(ILogger<OrderRpcConsumer> logger, IOrderService orderService) 
            : base(logger, "car_events", "get_user_orders", "direct")
        {
            this.orderService = orderService;
            this.logger = logger;
            _channel = CreateChannel();
        }

        protected override async Task HandleMessageAsync(GetUserOrdersEvent message)
        {
            var orders = await orderService.GetRentOrdersByUserIdAsync(message.UserId);

            var response = JsonSerializer.Serialize(OrderMapper.Map(orders));

            // Publish the response back to the `ReplyTo` queue
            var properties = GetMessageProperties();
            var replyTo = properties.ReplyTo;
            var correlationId = properties.CorrelationId;

            if (!string.IsNullOrWhiteSpace(replyTo) && !string.IsNullOrWhiteSpace(correlationId))
            {
                var replyProps = _channel.CreateBasicProperties();
                replyProps.CorrelationId = correlationId;

                var responseBytes = Encoding.UTF8.GetBytes(response);

                _channel.BasicPublish(
                    exchange: "",
                    routingKey: replyTo,
                    basicProperties: replyProps,
                    body: responseBytes);

                logger.LogInformation($"Replied to message with CorrelationId: {correlationId}");
            }
            else
            {
                logger.LogWarning("ReplyTo or CorrelationId is missing. Cannot send response.");
            }

            await Task.CompletedTask;
        }
    }
}
