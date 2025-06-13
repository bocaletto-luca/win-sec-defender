// File: Defender.Service/Program.cs
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading;

[assembly: System.STAThread]  // ensure COM for toasts
namespace Defender.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Setup Serilog early so toasts can log if needed
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/defender-.log", rollingInterval: RollingInterval.Day)
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
                .UseWindowsService()
                .ConfigureAppConfiguration((ctx, cfg) =>
                    cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                )
                .UseSerilog()
                .ConfigureServices((ctx, services) =>
                {
                    services.Configure<DefenderConfig>(ctx.Configuration.GetSection("Defender"));
                    services.AddHostedService<DefenderService>();
                });
    }
}
