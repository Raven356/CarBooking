using System.Text.Json.Serialization;

namespace RabbitMqLibrary.Events
{
    public class GetTimedEventHistoryEvent : BaseEvent
    {
        public GetTimedEventHistoryEvent() : base() { }

        [JsonConstructor]
        public GetTimedEventHistoryEvent(BaseEvent @event, Guid eventId) : base(eventId)
        {
            Event = @event;
        }

        public BaseEvent Event { get; set; }
    }
}
