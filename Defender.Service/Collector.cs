// Defender.Service/Collector.cs
using Quartz;
using Serilog;
using System.Diagnostics;
using System.Net.Mail;

[DisallowConcurrentExecution]
public class CollectorJob : IJob
{
    private readonly DefenderConfig _cfg;
    public CollectorJob(IOptions<DefenderConfig> opt)
        => _cfg = opt.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        Log.Information("CollectorJob executing");
        var report = new StringBuilder();

        // 1) Event Log check (esempio: Security log)
        var evts = new EventLog("Security").Entries
            .Cast<EventLogEntry>()
            .Where(e => e.TimeGenerated > DateTime.Now.AddMinutes(-30));
        report.AppendLine($"Recent Security Events: {evts.Count()}");

        // 2) Process watch
        var procs = Process.GetProcesses();
        foreach (var p in procs)
            if (_cfg.ProcessesToWatch.Contains(p.ProcessName, StringComparer.OrdinalIgnoreCase))
                report.AppendLine($"Watched process running: {p.ProcessName} (PID={p.Id})");

        // 3) Network ports (esempi via netstat o SharpPcap…)

        // 4) Compose & send email
        await Notifier.SendAsync(_cfg.Email, report.ToString());
    }
}
