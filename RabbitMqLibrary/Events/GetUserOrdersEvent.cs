namespace RabbitMqLibrary.Events
{
    public class GetUserOrdersEvent : BaseEvent
    {
        public int UserId { get; set; }
    }
}
