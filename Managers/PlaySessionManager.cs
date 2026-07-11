using PlayLimit.Models;
using PlayLimit.Services;

namespace PlayLimit.Managers;

public class PlaySessionManager
{
    private readonly UsageTrackerService _tracker = new();

    public async Task Update(IEnumerable<AppRule> rules)
    {
        foreach (var rule in rules)
        {
            if (!rule.Enabled)
                continue;

            if (!ProcessMonitorService.IsRunning(rule.ProcessName))
                continue;

            _tracker.AddSecond(rule.ProcessName);

            rule.UsageSeconds =
                _tracker.GetUsageSeconds(rule.ProcessName);

            int remaining =
                (10) - rule.UsageSeconds;

            // Sisa 5 menit
            if (remaining <= 300 && !rule.Warning5MinutesShown)
            {
                rule.Warning5MinutesShown = true;

                NotificationService.ShowWarning(
                    $"{rule.Name} akan ditutup dalam 5 menit.");
            }

            // Sisa 1 menit
            if (remaining <= 60 && !rule.Warning1MinuteShown)
            {
                rule.Warning1MinuteShown = true;

                NotificationService.ShowWarning(
                    $"{rule.Name} akan ditutup dalam 1 menit.");
            }

            // Limit habis
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

        // Reset waktu penggunaan
        _tracker.Reset(rule.ProcessName);

        // Reset tampilan
        rule.UsageSeconds = 0;

        // Izinkan warning muncul lagi saat aplikasi dibuka ulang
        rule.Warning5MinutesShown = false;
        rule.Warning1MinuteShown = false;
        rule.Closing = false;
    }
}