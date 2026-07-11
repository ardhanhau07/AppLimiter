using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppLimit.Models;
using AppLimit.Services;
using AppLimit.Helpers;
using System.Windows;

namespace AppLimit.ViewModels;

public partial class RuleDialogViewModel : ObservableObject
{
    private readonly AppRule? editingRule;
    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private string exePath = "";

    [ObservableProperty]
    private string processName = "";

    [ObservableProperty]
    private int timeLimit = 120;

    [ObservableProperty]
    private bool enabled = true;

    public RuleDialogViewModel(AppRule? rule = null)
    {
        editingRule = rule;

        if (rule == null)
            return;

        Name = rule.Name;
        ExePath = rule.ExePath;
        ProcessName = rule.ProcessName;
        TimeLimit = rule.TimeLimit;
        Enabled = rule.Enabled;
    }

    [RelayCommand]
    private void Browse()
    {
        string? file = FilePickerHelper.PickExe();

        if (file == null)
            return;

        ExePath = file;

        Name = ExeHelper.GetAppName(file);

        ProcessName = ExeHelper.GetProcessName(file);
    }

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            NotificationService.ShowWarning("Silakan pilih aplikasi.");
            return;
        }

        if (TimeLimit <= 0)
        {
            NotificationService.ShowWarning("Limit harus lebih dari 0.");
            return;
        }

        if (editingRule == null)
        {
            DatabaseService.AddRule(new AppRule
            {
                Name = Name,
                ExePath = ExePath,
                ProcessName = ProcessName,
                TimeLimit = TimeLimit,
                Enabled = Enabled
            });
        }
        else
        {
            editingRule.Name = Name;
            editingRule.ExePath = ExePath;
            editingRule.ProcessName = ProcessName;
            editingRule.TimeLimit = TimeLimit;
            editingRule.Enabled = Enabled;

            DatabaseService.UpdateRule(editingRule);
        }

        RequestClose?.Invoke();
    }

    public event Action? RequestClose;
}