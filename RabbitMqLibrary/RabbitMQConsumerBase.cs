using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace RabbitMqLibrary
{
    public abstract class RabbitMQConsumerBase<T> : BackgroundService, IDisposable where T : class
    {
        private readonly string _exchangeName;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<RabbitMQConsumerBase<T>> _logger;

        protected RabbitMQConsumerBase(ILogger<RabbitMQConsumerBase<T>> logger, string exchangeName)
        {
            _exchangeName = exchangeName;
            _logger = logger;

            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                Port = 5672
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare exchange and queue
            _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Fanout);
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName, exchange: _exchangeName, routingKey: "");

            // Set up consumer
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation($"Received message from {_exchangeName}: {message}");

                var eventData = JsonSerializer.Deserialize<T>(message);
                if (eventData != null)
                {
                    await HandleMessageAsync(eventData);
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.CompletedTask;

        protected abstract Task HandleMessageAsync(T message);

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
