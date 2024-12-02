
using System.Text.Json.Serialization;

namespace RabbitMqLibrary.Events
{
    public class OrderStartedEvent : BaseEvent
    {
        public OrderStartedEvent() : base() { }

        [JsonConstructor]
        public OrderStartedEvent(int carId, int userId, Guid eventId) : base(eventId)
        {
            CarId = carId;
            UserId = userId;
        }

        public int CarId { get; set; }

        public int UserId { get; set; }
    }
}
