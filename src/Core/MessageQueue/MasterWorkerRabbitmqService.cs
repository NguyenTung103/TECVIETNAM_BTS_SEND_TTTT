using Core.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text.Json;
using System.Threading;
using Core.Interfaces;

namespace Core.MessageQueue
{
    public class MasterRabbitmqService : IMasterMessageQueueService
    {
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(initialCount: 1, maxCount: 1);
        private readonly MasterRabbitmqConnection _masterRabbitmqConnection;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IZipHelper _zipHelper;
        private readonly ILogger<MasterRabbitmqService> _logger;
        private readonly IModel _subtribeUpdateMemoryChannel;
        private readonly IModel _publishUpdateMemoryChannel;
        private const string _updateMemoryExchangeName = "TOPIC_BTS_GATEWAY_SERVICE";

        public MasterRabbitmqService(
            ILogger<MasterRabbitmqService> logger,
            IZipHelper zipHelper,
            IOptions<MasterRabbitmqConnection> options)
        {
            _masterRabbitmqConnection = options.Value;
            _zipHelper = zipHelper;
            _logger = logger;
            _connectionFactory = new ConnectionFactory()
            {
                HostName = _masterRabbitmqConnection.HostName,
                Port = _masterRabbitmqConnection.Port,
                UserName = _masterRabbitmqConnection.UserName,
                Password = _masterRabbitmqConnection.Password,
                VirtualHost = _masterRabbitmqConnection.VirtualHost,
                ContinuationTimeout = new TimeSpan(0, 0, 0, _masterRabbitmqConnection.ContinuationTimeout)
            };            
            _connection = _connectionFactory.CreateConnection();

            _subtribeUpdateMemoryChannel = _connection.CreateModel();
            _subtribeUpdateMemoryChannel.ExchangeDeclare(exchange: _updateMemoryExchangeName, type: ExchangeType.Fanout);

            _publishUpdateMemoryChannel = _connection.CreateModel();
            _publishUpdateMemoryChannel.ExchangeDeclare(exchange: _updateMemoryExchangeName, type: ExchangeType.Fanout);
        }

        public virtual void BroadcastUpdateMemoryTask(QueueMessage queueMessage)
        {
            GuardClauses.Null(queueMessage, nameof(queueMessage));
            var body = _zipHelper.ZipByte(JsonSerializer.SerializeToUtf8Bytes(queueMessage));

            _semaphoreSlim.Wait();
            try
            {
                _publishUpdateMemoryChannel.BasicPublish(exchange: _updateMemoryExchangeName, routingKey: "", basicProperties: null, body: body);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public virtual void SubscribeUpdateMemoryTask(Action<QueueMessage> func)
        {
            GuardClauses.Null(func, nameof(func));

            var queueName = _subtribeUpdateMemoryChannel.QueueDeclare().QueueName;
            _subtribeUpdateMemoryChannel.QueueBind(queue: queueName, exchange: _updateMemoryExchangeName, routingKey: "");
            var consumer = new EventingBasicConsumer(_subtribeUpdateMemoryChannel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var message = JsonSerializer.Deserialize<QueueMessage>(_zipHelper.UnZipByte(ea.Body.ToArray()));
                    func(message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Run SubscribeUpdateMemoryTask error");
                }
                finally
                {
                    _subtribeUpdateMemoryChannel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };
            _subtribeUpdateMemoryChannel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }

        public virtual void Dispose()
        {
            _connection.Dispose();
            _publishUpdateMemoryChannel.Dispose();
            _subtribeUpdateMemoryChannel.Dispose();
        }
    }
}
