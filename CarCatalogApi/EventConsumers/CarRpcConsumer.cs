using RabbitMqLibrary;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using RabbitMqLibrary.Events;
using CarBookingBLL.Interfaces;
using CarCatalogApi.Mappers;

namespace CarCatalogApi.EventConsumers
{
    public class CarRpcConsumer : RabbitMQConsumerBase<GetOrderWithCarEvent>
    {
        private readonly ILogger<CarRpcConsumer> _logger;
        private readonly ICarService carService;
        private readonly IModel _channel;

        public CarRpcConsumer(ILogger<CarRpcConsumer> logger, ICarService carService)
            : base(logger, "order_events", "car_service_queue")
        {
            _logger = logger;
            this.carService = carService;
            _channel = CreateChannel();
        }

        protected override async Task HandleMessageAsync(GetOrderWithCarEvent @event)
        {
            var car = await carService.GetByIdAsync(@event.CarId);

            // Serialize the response
            var response = JsonSerializer.Serialize(CarMapper.Map(car));

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

                _logger.LogInformation($"Replied to message with CorrelationId: {correlationId}");
            }
            else
            {
                _logger.LogWarning("ReplyTo or CorrelationId is missing. Cannot send response.");
            }

            await Task.CompletedTask;
        }

        private IModel CreateChannel()
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
            return factory.CreateConnection().CreateModel();
        }
    }
}
