namespace PlayLimit.Models;

public class AppRule
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ExePath { get; set; } = string.Empty;

    public string ProcessName { get; set; } = string.Empty;

    public int UsageSeconds { get; set; }

    public int TimeLimit { get; set; }

    public bool Enabled { get; set; }

    public int RemainingMinutes =>
        Math.Max(0, TimeLimit - (UsageSeconds / 60));

    public double Progress =>
        TimeLimit == 0
            ? 0
            : (double)UsageSeconds / (TimeLimit * 60);

    public string UsageText =>
        TimeSpan.FromSeconds(UsageSeconds).ToString(@"hh\:mm\:ss");

    public bool Warning5MinutesShown { get; set; } = false;

    public bool Warning1MinuteShown { get; set; } = false;

    public bool Closing { get; set; } = false;
}