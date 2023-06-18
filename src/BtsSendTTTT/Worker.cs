using BtsGetwayService;
using Core.MSSQL.Responsitory.Interface;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BtsSendTTTT
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;        
        public readonly BtsGetway _btsGetway;
        public readonly Helper _helperUlti;        
        public Worker(ILogger<Worker> logger,            
            BtsGetway btsGetway,
            Helper helperUlti)
        {
            _helperUlti = helperUlti;
            _btsGetway = btsGetway;
            _logger = logger;           
        }        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {            
            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime from = _helperUlti.RoundDown(DateTime.Now, TimeSpan.FromMinutes(10));
                DateTime to = from.AddMinutes(10);                
                _logger.LogInformation("Time start: {0}", from.ToString("dd/MM/yyyy HH:mm"));
               _btsGetway.SendFile(to, from);
                await Task.Delay(600000, stoppingToken);
            }
        }
        public override async Task<Task> StartAsync(CancellationToken cancellationToken)
        {
            var dateTimeNow = DateTime.Now;
            var milisecondDelay = _helperUlti.ThoiGianDelayDeBatDauChayService(dateTimeNow,0);
            _logger.LogInformation("Worker starting at: {0}", DateTimeOffset.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            //await Task.Delay(milisecondDelay);
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Worker stopping at: {0}", DateTimeOffset.Now.ToString("dd/MM/yyyy HH:mm"));
            return base.StopAsync(cancellationToken);
        }
    }
}
