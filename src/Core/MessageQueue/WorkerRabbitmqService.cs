using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text.Json;
using System.Threading;

namespace Core.MessageQueue
{
    public class WorkerRabbitmqService : IWorkerMessageQueueService
    {
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(initialCount: 1, maxCount: 1);
        private readonly WorkerRabbitmqConnection _workerRabbitmqConnection;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IZipHelper _zipHelper;
        private readonly ILogger<WorkerRabbitmqService> _logger;
        private readonly IModel _workerChannel;
        private readonly IBasicProperties _publishWorkerProperties;
        private const string _workerQueueName = "UdpGateway";

        public WorkerRabbitmqService(
            ILogger<WorkerRabbitmqService> logger,
            IZipHelper zipHelper,
            IOptions<WorkerRabbitmqConnection> options)
        {
            _workerRabbitmqConnection = options.Value;
            _zipHelper = zipHelper;
            _logger = logger;
            
            
            _connectionFactory = new ConnectionFactory()
            {
                HostName = _workerRabbitmqConnection.HostName,
                Port = _workerRabbitmqConnection.Port,
                UserName = _workerRabbitmqConnection.UserName,
                Password = _workerRabbitmqConnection.Password,
                VirtualHost = _workerRabbitmqConnection.VirtualHost,
                ContinuationTimeout = new TimeSpan(0, 0, 0, _workerRabbitmqConnection.ContinuationTimeout)
            };
            _connection = _connectionFactory.CreateConnection();
            _workerChannel = _connection.CreateModel();
            _workerChannel.QueueDeclare(queue: _workerQueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);            

            _publishWorkerProperties = _workerChannel.CreateBasicProperties();
            _publishWorkerProperties.Persistent = true;
        }

        public virtual void PublishWorkerTask(QueueMessage queueMessage)
        {
            GuardClauses.Null(queueMessage, nameof(queueMessage));
            var body = _zipHelper.ZipByte(JsonSerializer.SerializeToUtf8Bytes(queueMessage));
            _semaphoreSlim.Wait();
            try
            {
                _workerChannel.BasicPublish(exchange: "", routingKey: _workerQueueName, basicProperties: _publishWorkerProperties, body: body);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public virtual void SubscribeWorkerTask(Action<QueueMessage> func)
        {
            GuardClauses.Null(func, nameof(func));

            var consumer = new EventingBasicConsumer(_workerChannel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var message = JsonSerializer.Deserialize<QueueMessage>(_zipHelper.UnZipByte(ea.Body.ToArray()));
                    func(message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Run SubscribeWorkerTask error");
                }
                finally
                {
                    _workerChannel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };
            _workerChannel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            _workerChannel.BasicConsume(queue: _workerQueueName, autoAck: false, consumer: consumer);
        }

        public virtual void Dispose()
        {
            _connection.Dispose();
            _workerChannel.Dispose();
        }
    }
}
