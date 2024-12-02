
namespace RabbitMqLibrary.TimedRoutine
{
    public class TimedEventHistoryPublisher : RabbitMQPublisherBase
    {
        public TimedEventHistoryPublisher() : base("timed_events", "direct", null)
        {
        }
    }
}
