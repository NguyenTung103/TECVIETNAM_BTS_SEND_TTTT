using bts.udpgateway.integration;
using BtsGetwayService;
using Core.Entities;
using Core.Interfaces;
using Core.PushMessage;
using Core.Setting;
using Infrastructure.Udp;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CheckDeviceService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly IUdpService _udpService;
        private AppSettingUDP _appSettingUDP;
        private readonly IPushMessageService _pushMessageService;
        public Worker(ILogger<Worker> logger,
            IUdpService udpService,
            IPushMessageService pushMessageService,
            IOptions<AppSettingUDP> options
            )
        {
            _logger = logger;
            _udpService = udpService;
            _appSettingUDP = options.Value;
            _pushMessageService = pushMessageService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);

            }
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _pushMessageService.SendMessageAsync("UDP Service is started.");
            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await _udpService.CheckDeviceS10();                
                await Task.Delay(1200000, cancellationToken);
            }
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Worker stopping at: {0}", DateTimeOffset.Now.ToString("dd/MM/yyyy HH:mm"));
            return base.StopAsync(cancellationToken);
        }
    }
}