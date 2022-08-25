using bts.udpgateway;
using BtsGetwayService;
using BtsGetwayService.Interface;
using BtsGetwayService.Service;
using Core.Logging;
using Core.MongoDb.Data.Interface;
using Core.MSSQL.Responsitory.Interface;
using Core.Setting;
using ES_CapDien.AppCode;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GetDataByMonth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    //services.AddHttpClient();                   
                    services.Configure<Connections>(hostContext.Configuration.GetSection("Connections"));
                    services.Configure<AppSetting>(hostContext.Configuration.GetSection("AppSetting"));
                    services.AddLog4net();
                    services.AddSingleton<IGroupData, GroupData>();
                    services.AddSingleton<IReportDailyHuongGioData, ReportDailyHuongGioData>();
                    services.AddSingleton<IReportDailyTocDoGioData, ReportDailyTocDoGioData>();
                    services.AddSingleton<ISiteData, SiteData>();
                    services.AddSingleton<IDataAlarmData, DataAlarmData>();
                    services.AddSingleton<IDataObservationMongoData, DataObservationMongoData>();
                    services.AddSingleton<IDataAlarmService, DataAlarmMongoService>();
                    services.AddSingleton<IDataObservationService, DataObservationMongoService>();
                    services.AddTransient<BtsGetway>();
                    services.AddHostedService<Worker>();
                })
                .UseWindowsService();
    }
}
