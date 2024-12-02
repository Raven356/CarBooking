using RabbitMqLibrary;

namespace OrderApi.EventPublisher
{
    public class OrderEventsTimeoutPublisher : RabbitMQPublisherBase
    {
        public OrderEventsTimeoutPublisher() : base("order_events_delayed", "x-delayed-message", new Dictionary<string, object> { { "x-delayed-type", "direct" } })
        {
        }
    }
}
