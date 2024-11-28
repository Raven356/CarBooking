using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

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

            // Declare exchange
            channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct);
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

        public async Task<string> PublishWithReply<T>(T @event, string routingKey)
        {
            var replyQueue = channel.QueueDeclare().QueueName;
            var correlationId = Guid.NewGuid().ToString();

            var props = channel.CreateBasicProperties();
            props.ReplyTo = replyQueue;
            props.CorrelationId = correlationId;

            var jsonMessage = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            // Publish the request
            channel.BasicPublish(exchange: exchange,
                                 routingKey: routingKey,
                                 basicProperties: props,
                                 body: body);

            var tcs = new TaskCompletionSource<string>();

            // Consume the reply
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    var replyMessage = Encoding.UTF8.GetString(ea.Body.ToArray());
                    tcs.SetResult(replyMessage);
                }
            };

            channel.BasicConsume(queue: replyQueue, autoAck: true, consumer: consumer);
            return await tcs.Task;
        }

        public void Dispose()
        {
            channel?.Close();
            connection?.Close();
        }
    }
}
