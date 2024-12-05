using RabbitMqLibrary;

namespace CarCatalogApi.EventPublishers
{
    public class CarEventsPubisher : RabbitMQPublisherBase
    {
        public CarEventsPubisher() : base("car_events", "direct")
        {
        }
    }
}
