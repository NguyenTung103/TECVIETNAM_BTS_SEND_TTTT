using Core.PushMessage;
using Infrastructure.Udp;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UdpService.Service
{    
    public class CheckStatusDevice : BackgroundService
    {
        private readonly IUdpService _udpService;
        private readonly IPushMessageService _pushMessageService;
        public CheckStatusDevice(
            IUdpService udpService,
            IPushMessageService pushMessageService
            )
        {
            _udpService = udpService;
            _pushMessageService = pushMessageService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _pushMessageService.SendMessageAsync("Check status device is started.");
            // Run the cleanup task every hour (adjust as needed)
            while (!stoppingToken.IsCancellationRequested)
            {
                await _udpService.CheckDeviceS10();
                await Task.Delay(1200000, stoppingToken);
            }
        }    
    }
}
