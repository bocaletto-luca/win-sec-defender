// Defender.Service/Service.cs
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using Serilog;

public class DefenderService : IHostedService
{
    private readonly DefenderConfig _cfg;
    private IScheduler? _scheduler;

    public DefenderService(IOptions<DefenderConfig> opt) 
        => _cfg = opt.Value;

    public async Task StartAsync(CancellationToken ct)
    {
        Log.Information("DefenderService starting");
        var factory = new StdSchedulerFactory();
        _scheduler = await factory.GetScheduler(ct);

        var job = JobBuilder.Create<CollectorJob>()
            .WithIdentity("collectJob")
            .Build();
        var trigger = TriggerBuilder.Create()
            .WithCronSchedule(_cfg.CronSchedule)
            .Build();

        await _scheduler.ScheduleJob(job, trigger, ct);
        await _scheduler.Start(ct);
    }

    public async Task StopAsync(CancellationToken ct)
    {
        Log.Information("DefenderService stopping");
        if (_scheduler != null)
            await _scheduler.Shutdown(ct);
    }
}
