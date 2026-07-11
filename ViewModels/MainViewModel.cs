using CommunityToolkit.Mvvm.ComponentModel;
using AppLimit.Models;
using AppLimit.Services;
using AppLimit.Views;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Threading;
using AppLimit.Managers;
using System.Diagnostics;

namespace AppLimit.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly DispatcherTimer _timer = new();
    private readonly UsageTrackerService _tracker = new();
    private readonly PlaySessionManager _manager = new();

    [ObservableProperty]
    private ObservableCollection<AppRule> rules = new();

    public MainViewModel()
    {
        LoadRules();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }

    private async void Timer_Tick(object? sender, EventArgs e)
    {
        await _manager.Update(Rules);

        Rules = new ObservableCollection<AppRule>(Rules);
    }

    private void LoadRules()
    {
        Rules.Clear();
        foreach (var rule in DatabaseService.GetAllRules())
        {
            Rules.Add(rule);
        }
    }

    [RelayCommand]
    private void AddRule()
    {
        var dialog = new RuleDialog();
        if (dialog.ShowDialog() == true)
        {
            LoadRules();
        }
    }

    [RelayCommand]
    private void ToggleRule(AppRule? rule)
    {
        if (rule == null)
            return;

        DatabaseService.UpdateRule(rule);
    }

    [RelayCommand]
    private void EditRule(AppRule? rule)
    {
        if (rule == null)
            return;

        var dialog = new RuleDialog(rule);

        if (dialog.ShowDialog() == true)
        {
            LoadRules();
        }
    }

    [RelayCommand]
    private void DeleteRule(AppRule? rule)
    {
        if (rule == null)
            return;

        if (MessageBox.Show(
            $"Hapus {rule.Name} ?",
            "AppLimit",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question)
            != MessageBoxResult.Yes)
            return;

        DatabaseService.DeleteRule(rule.Id);

        LoadRules();
    }

    [RelayCommand]
    private void TestProcess()
    {
        bool running = ProcessMonitorService.IsRunning("steam");

        MessageBox.Show(running ? "Steam sedang berjalan" : "Steam tidak berjalan");
    }
}