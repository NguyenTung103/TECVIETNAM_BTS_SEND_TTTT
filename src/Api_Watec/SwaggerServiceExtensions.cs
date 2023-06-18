using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Collections.Generic;
using System.IO;

namespace Api_Watec
{
    /// <summary>
    /// Mở rộng cấu hình title cùng các kiểu
    /// </summary>
    public static class SwaggerServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API APP WATEC", Version = "v1.0" });
                c.IncludeXmlComments(Path.Combine(Directory.GetCurrentDirectory(), "Api_Watec.xml")); // Comment trên web
                //c.OperationFilter<SwaggerAddKeyHeader>();
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Nhập token khi login tài khoản",
                    Name = "ApiKey",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    }
                    },
                    new string[] { }
                }
            });
            });

            return services;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Versioned API v2.0");
                c.DocumentTitle = "API APP WATEC";
                c.DocExpansion(DocExpansion.None);
            });

            return app;
        }
    }
}
