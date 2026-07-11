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

            // ==========================
            // Warning 5 menit
            // ==========================
            if (rule.TimeLimit > 5 &&
                rule.PreviousRemaining > 300 &&
                remaining <= 300 &&
                !rule.Warning5MinutesShown)
            {
                rule.Warning5MinutesShown = true;

                NotificationService.ShowWarning(
                    $"{rule.Name} akan ditutup dalam 5 menit.");
            }

            // ==========================
            // Warning 1 menit
            // ==========================
            if (rule.TimeLimit > 1 &&
                rule.PreviousRemaining > 60 &&
                remaining <= 60 &&
                !rule.Warning1MinuteShown)
            {
                rule.Warning1MinuteShown = true;

                NotificationService.ShowWarning(
                    $"{rule.Name} akan ditutup dalam 1 menit.");
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
    private async Task CloseRule(AppRule rule)
    {
        NotificationService.ShowError(
            $"{rule.Name} akan ditutup dalam 5 detik.");

        await Task.Delay(5000);

        ProcessKillerService.CloseProcess(rule.ProcessName);

        _tracker.Reset(rule.ProcessName);

        rule.UsageSeconds = 0;
        rule.isRunning = false;

        rule.Warning5MinutesShown = false;
        rule.Warning1MinuteShown = false;
        rule.Closing = false;

        // Reset threshold
        rule.PreviousRemaining = int.MaxValue;
    }
}