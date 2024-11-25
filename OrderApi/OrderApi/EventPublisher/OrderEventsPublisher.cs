using RabbitMqLibrary;

namespace OrderApi.EventPublisher
{
    public class OrderEventsPublisher : RabbitMQPublisherBase
    {
        public OrderEventsPublisher() : base("order_events")
        {
        }
    }
}
