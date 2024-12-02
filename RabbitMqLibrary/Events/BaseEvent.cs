using System.Text.Json.Serialization;

namespace RabbitMqLibrary.Events
{
    public abstract class BaseEvent
    {
        public Guid EventId { get; set; }

        protected BaseEvent()
        {
            EventId = Guid.NewGuid();
        }

        [JsonConstructor]
        protected BaseEvent(Guid eventId)
        {
            EventId = eventId;
        }
    }
}
