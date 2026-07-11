using System.Collections.Generic;

namespace PlayLimit.Services;

public class UsageTrackerService
{
    private readonly Dictionary<string, int> _usageSeconds = new();

    public void AddSecond(string processName)
    {
        if (!_usageSeconds.ContainsKey(processName))
            _usageSeconds[processName] = 0;

        _usageSeconds[processName]++;
    }

    public int GetUsageSeconds(string processName)
    {
        if (_usageSeconds.TryGetValue(processName, out int seconds))
            return seconds;

        return 0;
    }

    public void Reset(string processName)
    {
        if (_usageSeconds.ContainsKey(processName))
        {
            _usageSeconds.Remove(processName);
        }
    }

    public void Reset()
    {
        _usageSeconds.Clear();
    }
}