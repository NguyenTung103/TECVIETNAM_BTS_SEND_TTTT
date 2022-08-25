using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Topshelf;

namespace BtsGetwayService
{
    class Program
    {        
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<BtsGetway>();
            var exitCode = HostFactory.Run(x => // Setup các thông tin cần thiết
            {
                x.Service<BtsGetway>(s => // Khai báo các callBack
                {
                    s.ConstructUsing(name => new BtsGetway()); // Cách mà service của tôi được khởi tạo
                    s.WhenStarted(tc => tc.Start());  // Khi service được start
                    s.WhenStopped(tc => tc.Stop());  // Khi service được stop
                });
                x.RunAsLocalService(); // Service sẽ được chạy với quyền gì                
                // Thông tin service, bạn có thể không khai báo các thông tin này vì Topshelf cho phép bạn
                // khai báo khi install thông qua cmd
                x.SetDescription("This is service send file to Trung Tam Thong Tin");
                x.SetDisplayName("BtsGetwaySendFile");
                x.SetServiceName("BtsGetwaySendFile");
            });
            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole())
            .AddTransient<BtsGetway>();
        }
    }
}
