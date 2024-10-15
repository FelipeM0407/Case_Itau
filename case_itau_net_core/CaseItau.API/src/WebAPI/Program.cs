using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CaseItau.API.Infrastructure.Logging;
using CaseItau.API.Infrastructure.Filters;
using Microsoft.Extensions.DependencyInjection;
using CaseItau.API.src.WebAPI;

namespace CaseItau.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
                    {
                        LogLevel = LogLevel.Error
                    }));
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(services =>
                {
                    services.AddControllers(options =>
                    {
                        options.Filters.Add(typeof(ApiExceptionFilter));
                    });
                });
    }
}
