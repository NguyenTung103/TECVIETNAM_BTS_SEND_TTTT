using bts.udpgateway;
using BtsGetwayService.Interface;
using BtsGetwayService.Service;
using BtsGetwayService;
using CheckDeviceService;
using Core.Interfaces;
using Core.Logging;
using Core.MessageQueue;
using Core.MongoDb.Data.Interface;
using Core.MSSQL.Responsitory.Interface;
using Core.Setting;
using Core;
using ES_CapDien.AppCode;
using Infrastructure.Udp;


var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();
IHost host = (IHost)Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.Configure<Connections>(config.GetSection("Connections"));
        services.Configure<AppSettingUDP>(config.GetSection("AppSettingUDP"));
        services.Configure<WorkerRabbitmqConnection>(config.GetSection("WorkerRabbitmqConnection"));
        services.Configure<MasterRabbitmqConnection>(config.GetSection("MasterRabbitmqConnection"));
        services.AddSingleton<IMasterMessageQueueService, MasterRabbitmqService>();
        services.AddSingleton<IWorkerMessageQueueService, WorkerRabbitmqService>();
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
        services.AddHostedService<Worker>();
        services.AddLog4net();

        services.AddSingleton<IReportS10Service, ReportS10Service>();
        services.AddTransient<IReportDailyService, ReportDailyService>();
        services.AddTransient<IUdpService, Infrastructure.Udp.UdpService>();
        services.AddTransient<IDataObservationService, DataObservationMongoService>();
        services.AddTransient<IDataAlarmService, DataAlarmMongoService>();
    }).UseWindowsService();    

await host.RunAsync();
