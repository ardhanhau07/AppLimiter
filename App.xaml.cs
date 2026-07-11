using System.Windows;
using AppLimit.Services;

namespace AppLimit;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        try
        {
            base.OnStartup(e);

            DatabaseService.Initialize();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
    }

    public App()
    {
        this.DispatcherUnhandledException += (s, e) =>
        {
            MessageBox.Show(e.Exception.ToString(), "Unhandled Exception");
            e.Handled = true;
        };
    }
}