using System.Text.Json.Serialization;

namespace RabbitMqLibrary.Events
{
    public class TimedHistoryEvent : BaseEvent
    {
        public TimedHistoryEvent() : base() { }

        [JsonConstructor]
        public TimedHistoryEvent(TimedEventAction action, Guid eventId) : base(eventId)
        {
            Action = action;
        }

        public TimedEventAction Action { get; set; }
    }
}
