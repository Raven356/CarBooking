using RabbitMqLibrary;
using RabbitMqLibrary.Events;

namespace TimedEventsApi.EventConsumers
{
    public class AddTimedEventHistoryConsumer : RabbitMQConsumerBase<TimedHistoryEvent>
    {
        public AddTimedEventHistoryConsumer(ILogger<RabbitMQConsumerBase<TimedHistoryEvent>> logger) 
            : base(logger, "timed_events", "add_history", "direct", null)
        {
        }

        protected override async Task HandleMessageAsync(TimedHistoryEvent message)
        {
            var eventType = message.Action.Event.GetType();
            if (RabbitMQTimeoutEventStatuses.Statuses.TryGetValue(eventType, out IEnumerable<TimedEventAction> actions))
            {
                RabbitMQTimeoutEventStatuses.Statuses[eventType] = actions.Append(new TimedEventAction { Event = message.Action.Event, ShouldBeHandled = true });
            }
            else
            {
                RabbitMQTimeoutEventStatuses.Statuses.Add(eventType, [new() { Event = message.Action.Event, ShouldBeHandled = true }]);
            }

            await Task.CompletedTask;
        }
    }
}
