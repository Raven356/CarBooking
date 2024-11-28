using CarBookingBLL.Interfaces;
using RabbitMqLibrary;
using RabbitMqLibrary.Events;

namespace CarCatalogApi.EventConsumers
{
    public class OrderStartedEventConsumer : RabbitMQConsumerBase<OrderStartedEvent>
    {
        private readonly ICarService carService;

        public OrderStartedEventConsumer(ILogger<RabbitMQConsumerBase<OrderStartedEvent>> logger, ICarService carService) : base(logger, "order_events", "")
        {
            this.carService = carService;
        }

        protected override async Task HandleMessageAsync(OrderStartedEvent message)
        {
            await carService.MakeCarBooked(message.CarId, message.UserId);
        }
    }
}
