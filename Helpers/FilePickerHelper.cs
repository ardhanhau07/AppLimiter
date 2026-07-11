using Microsoft.Win32;

namespace PlayLimit.Helpers;

public static class FilePickerHelper
{
    public static string? PickExe()
    {
        OpenFileDialog dialog = new();

        dialog.Filter = "Executable (*.exe)|*.exe";
        dialog.Multiselect = false;
        dialog.Title = "Pilih aplikasi";

        if (dialog.ShowDialog() == true)
            return dialog.FileName;

        return null;
    }
}