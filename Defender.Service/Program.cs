// File: Defender.Service/Program.cs
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Prometheus;

namespace Defender.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/defender-.log", rollingInterval: RollingInterval.Day)
                .WriteTo.EventLog("DefenderService", manageEventSource: true)
                .CreateLogger();

            try
            {
                Log.Information("Starting Defender Service");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Service terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService() // runs under LocalService by default
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://0.0.0.0:8000");
                    webBuilder.ConfigureServices(services =>
                    {
                        services.AddHealthChecks();
                    });
                    webBuilder.Configure(app =>
                    {
                        app.UseRouting();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapHealthChecks("/healthz");
                            endpoints.MapMetrics(); // Prometheus metrics at /metrics
                        });
                    });
                })
                .UseSerilog()
                .ConfigureServices((ctx, services) =>
                {
                    services.Configure<DefenderConfig>(ctx.Configuration.GetSection("Defender"));
                    services.AddHostedService<DefenderService>();
                });
    }
}
