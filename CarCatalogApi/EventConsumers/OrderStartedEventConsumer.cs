using CarBookingBLL.Interfaces;
using RabbitMqLibrary;
using RabbitMqLibrary.EventConverter;
using RabbitMqLibrary.Events;
using RabbitMqLibrary.TimedRoutine;
using System.Text.Json;

namespace CarCatalogApi.EventConsumers
{
    public class OrderStartedEventConsumer : RabbitMQConsumerBase<OrderStartedEvent>
    {
        private readonly ICarService carService;
        private readonly TimedEventHistoryPublisher timedEventHistoryPublisher;

        public OrderStartedEventConsumer(ILogger<RabbitMQConsumerBase<OrderStartedEvent>> logger, ICarService carService,
            TimedEventHistoryPublisher timedEventHistoryPublisher) 
            : base(logger, "order_events_delayed", "create_order", "x-delayed-message", 
                  new Dictionary<string, object> { { "x-delayed-type", "direct" } })
        {
            this.carService = carService;
            this.timedEventHistoryPublisher = timedEventHistoryPublisher;
        }

        protected override async Task HandleMessageAsync(OrderStartedEvent message)
        {
            var timedHistoryRequest = new GetTimedEventHistoryEvent
            {
                Event = message,
                EventId = message.EventId
            };

            var options = new JsonSerializerOptions
            {
                Converters = { new BaseEventConverter() },
                PropertyNameCaseInsensitive = true
            };


            var response = await timedEventHistoryPublisher.PublishWithReply(timedHistoryRequest, "get_history_event");
            var timedHistory = JsonSerializer.Deserialize<TimedEventAction>(response, options);
            if (timedHistory.ShouldBeHandled)
            {
                await carService.MakeCarBooked(message.CarId, message.UserId);
            }
        }
    }
}
