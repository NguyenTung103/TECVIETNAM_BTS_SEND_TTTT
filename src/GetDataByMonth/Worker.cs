using BtsGetwayService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GetDataByMonth
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        public readonly BtsGetway _btsGetway;
        public Worker(ILogger<Worker> logger, BtsGetway btsGetway)
        {
            _logger = logger;
            _btsGetway = btsGetway;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {            
            Console.WriteLine("Enter year : ");
            var yearStr = Console.ReadLine();
            int year = Int32.Parse(yearStr);
            Console.WriteLine("Enter month : ");
            var monthStr = Console.ReadLine();
            int month = Int32.Parse(monthStr);
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            _btsGetway.SendFile(year, month);
            _logger.LogInformation("Worker finished at: {time}", DateTimeOffset.Now);            
        }
    }
}
