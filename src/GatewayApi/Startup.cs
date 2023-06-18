using bts.udpgateway;
using BtsGetwayService;
using Core;
using Core.Interfaces;
using Core.MessageQueue;
using Core.MSSQL.Responsitory.Interface;
using Core.Setting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GatewayApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();                     
            SwaggerServiceExtensions.AddSwaggerDocumentation(services);
            services.Configure<WorkerRabbitmqConnection>(Configuration.GetSection("WorkerRabbitmqConnection"));
            services.Configure<MasterRabbitmqConnection>(Configuration.GetSection("MasterRabbitmqConnection"));
            services.AddSingleton<IMasterMessageQueueService, MasterRabbitmqService>();
            services.AddSingleton<IWorkerMessageQueueService, WorkerRabbitmqService>();
            services.AddTransient<Helper>();
            services.AddSingleton<IZipHelper, GZipHelper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            SwaggerServiceExtensions.UseSwaggerDocumentation(app);
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
