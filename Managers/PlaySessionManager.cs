using AppLimit.Models;
using AppLimit.Services;

namespace AppLimit.Managers;

public class PlaySessionManager
{
    private readonly UsageTrackerService _tracker = new();

    public async Task Update(IEnumerable<AppRule> rules)
    {
        foreach (var rule in rules)
        {
            if (!rule.Enabled)
                continue;

            rule.isRunning =
                ProcessMonitorService.IsRunning(rule.ProcessName);

            if (!rule.isRunning)
                continue;

            _tracker.AddSecond(rule.ProcessName);

            rule.UsageSeconds =
                _tracker.GetUsageSeconds(rule.ProcessName);

            int remaining =
                (rule.TimeLimit * 60) - rule.UsageSeconds;

            if (!rule.WarningShown &&
    rule.PreviousRemaining > rule.WarningSeconds &&
    remaining <= rule.WarningSeconds)
            {
                rule.WarningShown = true;

                NotificationService.ShowWarning(
                    $"{rule.Name} akan ditutup dalam {rule.WarningSeconds} detik.");
            }

            if (remaining <= 0 && !rule.Closing)
            {
                rule.Closing = true;

                await CloseRule(rule);
            }

            // Simpan remaining sebelumnya
            rule.PreviousRemaining = remaining;

            // ==========================
            // Force Close
            // ==========================
            if (remaining <= 0 && !rule.Closing)
            {
                rule.Closing = true;

                await CloseRule(rule);
            }
        }
    }
    private Task CloseRule(AppRule rule)
    {
        ProcessKillerService.CloseProcess(rule.ProcessName);

        _tracker.Reset(rule.ProcessName);

        rule.UsageSeconds = 0;
        rule.IsRunning = false;

        rule.WarningShown = false;
        rule.Closing = false;
        rule.PreviousRemaining = int.MaxValue;

        return Task.CompletedTask;
    }
}