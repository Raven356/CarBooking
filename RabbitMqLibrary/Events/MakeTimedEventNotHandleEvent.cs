using System.Text.Json.Serialization;

namespace RabbitMqLibrary.Events
{
    public class MakeTimedEventNotHandleEvent : BaseEvent
    {
        public MakeTimedEventNotHandleEvent() : base() { }

        [JsonConstructor]
        public MakeTimedEventNotHandleEvent(BaseEvent @event, Guid eventId) : base(eventId)
        {
            Event = @event;
        }

        public BaseEvent Event { get; set; }

        public Comparer<OrderStartedEvent> OrderStartedEventComparer { get; } = Comparer<OrderStartedEvent>
            .Create((oldEvent, newEvent) => oldEvent.UserId == newEvent.UserId 
            && oldEvent.CarId == newEvent.CarId ? 1 : 0);
    }
}
