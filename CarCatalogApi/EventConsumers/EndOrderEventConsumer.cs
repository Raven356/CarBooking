using CarBookingBLL.Interfaces;
using RabbitMQ.Client;
using RabbitMqLibrary;
using RabbitMqLibrary.Events;

namespace CarCatalogApi.EventConsumers
{
    public class EndOrderEventConsumer : RabbitMQConsumerBase<EndOrderEvent>
    {
        private readonly ICarService carService;

        public EndOrderEventConsumer(ILogger<RabbitMQConsumerBase<EndOrderEvent>> logger, ICarService carService) 
            : base(logger, "order_events", "end_order", ExchangeType.Direct)
        {
            this.carService = carService;
        }

        protected override async Task HandleMessageAsync(EndOrderEvent message)
        {
            await carService.MakeCarAvailable(message.CarId);
        }
    }
}
