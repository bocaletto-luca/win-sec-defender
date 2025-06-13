// Defender.Service/Program.cs
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()  // install as service
    .ConfigureAppConfiguration((ctx, cfg) =>
    {
        cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .WriteTo.File("logs/defender-.log", rollingInterval: RollingInterval.Day)
    )
    .ConfigureServices((ctx, services) =>
    {
        services.Configure<DefenderConfig>(ctx.Configuration.GetSection("Defender"));
        services.AddHostedService<DefenderService>();
    })
    .Build();

await host.RunAsync();
