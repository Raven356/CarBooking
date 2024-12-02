using RabbitMqLibrary.Events;

namespace RabbitMqLibrary
{
    public class TimedEventAction
    {
        public BaseEvent Event { get; set; }

        public bool ShouldBeHandled { get; set; }
    }
}
