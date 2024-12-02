using System.Text.Json.Serialization;

namespace RabbitMqLibrary.Events
{
    public class EndOrderEvent : BaseEvent
    {
        public EndOrderEvent() : base()
        {

        }

        [JsonConstructor]
        public EndOrderEvent(int carId, Guid eventId) : base(eventId)
        {
            CarId = carId;
        }

        public int CarId { get; set; }
    }
}
