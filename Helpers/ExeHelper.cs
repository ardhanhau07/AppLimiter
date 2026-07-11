using System.Diagnostics;
using System.IO;

namespace PlayLimit.Helpers;

public static class ExeHelper
{
    public static string GetProcessName(string exePath)
    {
        return Path.GetFileNameWithoutExtension(exePath);
    }

    public static string GetAppName(string exePath)
    {
        try
        {
            var info = FileVersionInfo.GetVersionInfo(exePath);

            if (!string.IsNullOrWhiteSpace(info.ProductName))
                return info.ProductName;

            if (!string.IsNullOrWhiteSpace(info.FileDescription))
                return info.FileDescription;
        }
        catch
        {
        }

        return Path.GetFileNameWithoutExtension(exePath);
    }
}