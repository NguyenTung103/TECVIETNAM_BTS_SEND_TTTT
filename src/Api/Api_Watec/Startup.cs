using bts.udpgateway;
using Core.Caching;
using Core.Logging;
using Core.MSSQL.Responsitory.Interface;
using Core.Setting;
using Infrastructure.Udp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_Watec
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
            services.AddHttpClient();
            services.Configure<JWT>(Configuration.GetSection("JWT"));
            services.Configure<JwtAccountConfig>(Configuration.GetSection("JwtAccountConfig"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:ScretKey"]))
                };
            });
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(Configuration.GetSection("RedisCacheConfig")["Configuration"]));
            services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = Configuration.GetSection("RedisCacheConfig")["Configuration"];
                option.InstanceName = Configuration.GetSection("RedisCacheConfig")["InstanceName"];
            });            
            services.AddControllers();
            services.Configure<Connections>(Configuration.GetSection("Connections"));
            services.Configure<CacheSettings>(Configuration.GetSection("CacheSettings"));            
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddSingleton<IReportS10Data, ReportS10Data>();
            services.AddSingleton<IObservationData, ObservationData>();
            services.AddSingleton<ISiteData, SiteData>();
            services.AddSingleton<IReportS10Service, ReportS10Service>();
            services.AddSingleton<IObservationService, ObservationService>();
            services.AddSingleton<IAsyncCacheService, RedisCacheService>();            
            services.AddLog4net();


            services.AddSingleton<ISiteService, SiteService>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                // Cấu hình Bearer Token cho Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Nhập JWT Token vào ô bên dưới (không cần `Bearer ` prefix)",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
