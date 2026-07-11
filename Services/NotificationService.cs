using System.Windows;

namespace PlayLimit.Services;

public static class NotificationService
{
    public static void ShowWarning(string message)
    {
        MessageBox.Show(
            message,
            "PlayLimit",
            MessageBoxButton.OK,
            MessageBoxImage.Warning);
    }

    public static void ShowError(string message)
    {
        MessageBox.Show(
            message,
            "PlayLimit",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }
}