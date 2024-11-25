using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace RabbitMqLibrary
{
    public abstract class RabbitMQPublisherBase : IDisposable
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string exchange;

        public RabbitMQPublisherBase(string exchange)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            // Declare exchange for order events
            channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout);
            this.exchange = exchange;
        }

        public void Publish<T>(T @event)
        {
            var jsonMessage = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            channel.BasicPublish(exchange: exchange,
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);
        }

        public void Dispose()
        {
            channel?.Close();
            connection?.Close();
        }
    }
}
