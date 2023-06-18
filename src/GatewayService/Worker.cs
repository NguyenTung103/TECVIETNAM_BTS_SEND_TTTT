using Core.Interfaces;
using Core.Setting;
using Infrastructure.Udp;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GatewayService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IWorkerMessageQueueService _workerMessageQueueService;
        private readonly IMasterMessageQueueService _masterMessageQueueService;
        private readonly IUdpService _udpService;
        public Worker(ILogger<Worker> logger,
            IWorkerMessageQueueService workerMessageQueueService,
            IUdpService udpService,
            IMasterMessageQueueService masterMessageQueueService)
        {
            _logger = logger;
            _workerMessageQueueService = workerMessageQueueService;
            _masterMessageQueueService = masterMessageQueueService;
            _udpService = udpService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
            }
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker starting at: {0}", DateTimeOffset.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            _workerMessageQueueService.SubscribeWorkerTask(async (message) =>
            {
                var json = JsonSerializer.Serialize(message.Datas);
                var data = message.Datas["Message"];
                if (!string.IsNullOrEmpty(data))
                {
                    await _udpService.Insert(data);
                }
            });
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Worker stopping at: {0}", DateTimeOffset.Now.ToString("dd/MM/yyyy HH:mm"));
            return base.StopAsync(cancellationToken);
        }
    }
}
