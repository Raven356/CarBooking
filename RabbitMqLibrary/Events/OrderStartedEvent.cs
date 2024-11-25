namespace RabbitMqLibrary.Events
{
    public class OrderStartedEvent
    {
        public int CarId { get; set; }

        public int UserId { get; set; }
    }
}
