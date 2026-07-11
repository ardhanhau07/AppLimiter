using System.Diagnostics;

namespace PlayLimit.Services;

public static class ProcessKillerService
{
    public static void CloseProcess(string processName)
    {
        var processes = Process.GetProcessesByName(processName);

        foreach (var process in processes)
        {
            try
            {
                // Coba tutup secara normal terlebih dahulu
                if (process.CloseMainWindow())
                {
                    process.WaitForExit(5000);

                    if (process.HasExited)
                        continue;
                }

                // Kalau masih hidup, paksa tutup
                process.Kill(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}