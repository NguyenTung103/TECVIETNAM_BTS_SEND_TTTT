using bts.udpgateway;
using BtsGetwayService;
using BtsGetwayService.Interface;
using BtsGetwayService.Service;
using Core;
using Core.Logging;
using Core.MongoDb.Data.Interface;
using Core.MSSQL.Responsitory.Interface;
using Core.Setting;
using ES_CapDien.AppCode;
using Infrastructure.Udp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Test
{
    public class BaseTest
    {
        public IHost TestHost { get; }
        public BaseTest()
        {
            TestHost = CreateHostBuilder().Build();
            Task.Run(() => TestHost.RunAsync());
        }
        public static IHostBuilder CreateHostBuilder(string[] args = null) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true);
                config.AddEnvironmentVariables();

                if (args != null)
                {
                    config.AddCommandLine(args);
                }
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<Connections>(hostContext.Configuration.GetSection("Connections"));
                services.Configure<AppSettingUDP>(hostContext.Configuration.GetSection("AppSettingUDP"));                             
                services.AddSingleton<IGroupData, GroupData>();
                services.AddSingleton<IReportS10Data, ReportS10Data>();
                services.AddSingleton<ISiteData, SiteData>();
                services.AddSingleton<IReportDailyNhietDoData, ReportDailyNhietDoData>();
                services.AddSingleton<IReportDailyDoAmData, ReportDailyDoAmData>();
                services.AddSingleton<IReportDailyLuongMuaData, ReportDailyLuongMuaData>();
                services.AddSingleton<IReportDailyApSuatData, ReportDailyApSuatData>();
                services.AddSingleton<IReportDailyTocDoDongChayData, ReportDailyTocDoDongChayData>();
                services.AddSingleton<IReportDailyMucNuocData, ReportDailyMucNuocData>();
                services.AddSingleton<IReportDailyLuuLuongDongChayData, ReportDailyLuuLuongDongChayData>();
                services.AddSingleton<IReportDailyBucXaMatTroiData, ReportDailyBucXaMatTroiData>();
                services.AddSingleton<IReportDailyHuongGioData, ReportDailyHuongGioData>();
                services.AddSingleton<IReportDailyTocDoGioData, ReportDailyTocDoGioData>();
                services.AddSingleton<IRegisterSMSData, RegisterSMSData>();
                services.AddSingleton<IObservationData, ObservationData>();
                services.AddSingleton<ISMSServerData, SMSServerData>();
                services.AddSingleton<IDataObservationMongoData, DataObservationMongoData>();
                services.AddSingleton<IDataAlarmData, DataAlarmData>();
                services.AddSingleton<ICommandData, CommandData>();

                services.AddTransient<Helper>();
                services.AddSingleton<IZipHelper, GZipHelper>();               
                services.AddLog4net();

                services.AddSingleton<IReportS10Service, ReportS10Service>();
                services.AddTransient<IReportDailyService, ReportDailyService>();
                services.AddTransient<IUdpService, Infrastructure.Udp.UdpService>();
                services.AddTransient<IDataObservationService, DataObservationMongoService>();
                services.AddTransient<IDataAlarmService, DataAlarmMongoService>();
            })
            .ConfigureLogging((hostingContext, logging) =>
            {
                //logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                //logging.AddConsole();
            });
    }
}
