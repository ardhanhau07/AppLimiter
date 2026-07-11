using CommunityToolkit.Mvvm.ComponentModel;
using PlayLimit.Models;
using PlayLimit.Services;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace PlayLimit.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<AppRule> rules = new();

    public MainViewModel()
    {
        LoadRules();
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
        
    }
}