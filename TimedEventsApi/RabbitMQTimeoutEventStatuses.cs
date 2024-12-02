using RabbitMqLibrary;

namespace TimedEventsApi
{
    public static class RabbitMQTimeoutEventStatuses
    {
        public static Dictionary<Type, IEnumerable<TimedEventAction>> Statuses { get; set; } = [];
    }
}
