using RabbitMqLibrary;
using RabbitMqLibrary.Events;
using System;

namespace TimedEventsApi.EventConsumers
{
    public class MakeTimedEventNotHandledConsumer : RabbitMQConsumerBase<MakeTimedEventNotHandleEvent>
    {
        public MakeTimedEventNotHandledConsumer(ILogger<RabbitMQConsumerBase<MakeTimedEventNotHandleEvent>> logger) 
            : base(logger, "timed_events", "make_not_consumed", "direct", null)
        {
        }

        protected override async Task HandleMessageAsync(MakeTimedEventNotHandleEvent message)
        {
            if (RabbitMQTimeoutEventStatuses.Statuses.TryGetValue(message.Event.GetType(), out IEnumerable<TimedEventAction> actions))
            {
                if (message.Event.GetType() == typeof(OrderStartedEvent)) 
                {
                    var messageEvent = (OrderStartedEvent)message.Event;
                    var @event = actions.First(action => messageEvent.CarId == ((OrderStartedEvent)action.Event).CarId 
                        && messageEvent.UserId == ((OrderStartedEvent)action.Event).UserId);
                    @event.ShouldBeHandled = false;
                }
            }

            await Task.CompletedTask;
        }
    }
}
