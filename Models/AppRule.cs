using CommunityToolkit.Mvvm.ComponentModel;

namespace AppLimit.Models;

public partial class AppRule : ObservableObject
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ExePath { get; set; } = string.Empty;

    public string ProcessName { get; set; } = string.Empty;

    [ObservableProperty]
    private int usageSeconds;

    public int TimeLimit { get; set; }

    [ObservableProperty]
    private bool enabled;

    [ObservableProperty]
    public bool isRunning;

    public int RemainingMinutes =>
        Math.Max(0, TimeLimit - (UsageSeconds / 60));

    public string UsageText =>
        TimeSpan.FromSeconds(UsageSeconds).ToString(@"hh\:mm\:ss");

    public string StatusText =>
        isRunning ? "Apps Running" : "Apps Not Running";

    public string EnabledText =>
    enabled ? "Pembatasan Aktif" : "Pembatasan Nonaktif";

    // Digunakan untuk mendeteksi saat melewati threshold
    public int PreviousRemaining { get; set; } = int.MaxValue;

    public bool Warning5MinutesShown { get; set; }

    public bool Warning1MinuteShown { get; set; }

    public bool Closing { get; set; }
    partial void OnEnabledChanged(bool value)
    {
        OnPropertyChanged(nameof(EnabledText));
    }
}