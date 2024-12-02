using System.Text.Json.Serialization;

namespace RabbitMqLibrary.Events
{
    public class GetOrderWithCarEvent : BaseEvent
    {
        public GetOrderWithCarEvent() : base() { }

        [JsonConstructor]
        public GetOrderWithCarEvent(int carId, Guid eventId) : base(eventId)
        {
            CarId = carId;
        }

        public int CarId { get; set; }
    }
}
