using System.Windows;

namespace AppLimit.Services;

public static class NotificationService
{
    public static void ShowWarning(string message)
    {
        MessageBox.Show(
            message,
            "AppLimit",
            MessageBoxButton.OK,
            MessageBoxImage.Warning);
    }

    public static void ShowError(string message)
    {
        MessageBox.Show(
            message,
            "AppLimit",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }
}