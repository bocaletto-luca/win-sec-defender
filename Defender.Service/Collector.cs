// File: Defender.Service/Collector.cs
using System;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Quartz;
using Serilog;

namespace Defender.Service
{
    [DisallowConcurrentExecution]
    public class CollectorJob : IJob
    {
        private readonly DefenderConfig _cfg;
        public CollectorJob(IOptions<DefenderConfig> opt) 
            => _cfg = opt.Value;

        public async Task Execute(IJobExecutionContext context)
        {
            Log.Information("CollectorJob started");
            var report = new StringBuilder();

            // Event Log example
            try
            {
                var evts = new EventLog("Security").Entries;
                report.AppendLine($"Security events in last interval: {evts.Count}");
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Failed to read Security event log");
            }

            // Watch specific processes
            foreach (var procName in _cfg.ProcessesToWatch)
            {
                foreach (var p in Process.GetProcessesByName(procName))
                {
                    report.AppendLine($"Watched process: {p.ProcessName} (PID {p.Id})");
                    VisualNotifier.ShowTrayBalloon("Security Alert",
                        $"Process {p.ProcessName} started (PID {p.Id})");
                    VisualNotifier.ShowToast("Defender Alert",
                        $"Process {p.ProcessName} running");
                }
            }

            // Send email
            try
            {
                await Notifier.SendAsync(_cfg.Email, report.ToString());
                Log.Information("Alert email sent");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to send alert email");
            }
        }
    }
}
