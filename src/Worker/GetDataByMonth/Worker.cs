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
        public readonly BtsGatewayS10 _btsGatewayS10;
        public Worker(ILogger<Worker> logger, BtsGetway btsGetway, BtsGatewayS10 btsGatewayS10)
        {
            _logger = logger;
            _btsGetway = btsGetway;
            _btsGatewayS10 = btsGatewayS10;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Enter year : ");
            var yearStr = Console.ReadLine();
            int year = Int32.Parse(yearStr);
            Console.WriteLine("Enter month : ");
            var monthStr = Console.ReadLine();
            int month = Int32.Parse(monthStr);
            Console.WriteLine("Enter day : ");
            var dayStr = Console.ReadLine();
            int day = Int32.Parse(dayStr);
            Console.WriteLine("Type data (1: Report_S10, 2: Report_Normal)");
            var typeReportStr = Console.ReadLine();
            int typeReport = Int32.Parse(typeReportStr);
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            if (typeReport != 0 && typeReport == 1)
                _btsGatewayS10.SendFile(year, month, day);
            else if (typeReport != 0 && typeReport == 2)
                _btsGetway.SendFile(year, month);
            _logger.LogInformation("Worker finished at: {time}", DateTimeOffset.Now);
        }
    }
}
