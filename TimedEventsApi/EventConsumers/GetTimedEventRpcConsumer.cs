using RabbitMQ.Client;
using RabbitMqLibrary;
using RabbitMqLibrary.EventConverter;
using RabbitMqLibrary.Events;
using System.Text;
using System.Text.Json;

namespace TimedEventsApi.EventConsumers
{
    public class GetTimedEventRpcConsumer : RabbitMQConsumerBase<GetTimedEventHistoryEvent>
    {
        private readonly IModel _channel;
        private readonly ILogger<GetTimedEventRpcConsumer> _logger;

        public GetTimedEventRpcConsumer(ILogger<GetTimedEventRpcConsumer> logger) 
            : base(logger, "timed_events", "get_history_event", "direct", null)
        {
            _channel = CreateChannel();
            this._logger = logger;
        }

        protected override async Task HandleMessageAsync(GetTimedEventHistoryEvent message)
        {
            var history = RabbitMQTimeoutEventStatuses.Statuses[message.Event.GetType()].First(e => e.Event.EventId == message.EventId);

            var options = new JsonSerializerOptions
            {
                Converters = { new BaseEventConverter() },
                PropertyNameCaseInsensitive = true
            };

            var response = JsonSerializer.Serialize(history, options);

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
