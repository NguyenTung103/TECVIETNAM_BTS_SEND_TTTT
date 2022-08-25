using log4net;
using log4net.Config;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;

namespace Core.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public static class Log4Configure
    {
        /// <summary>
        /// Đăng ký log cho log4net
        /// </summary>
        /// <param name="services">nhà chứa Dịch vụ</param>
        public static void AddLog4net(this IServiceCollection services)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            var fileLog = new FileInfo("log4net.config");
            XmlConfigurator.Configure(logRepository, fileLog);
            //XmlConfigurator.Configure(LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy)), File.OpenRead("log4net.config"));
            services.AddTransient<ILoggingService, Log4Service>();
        }
    }
}
