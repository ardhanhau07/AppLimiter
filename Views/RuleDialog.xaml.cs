using System.Windows;
using PlayLimit.ViewModels;
using PlayLimit.Models;

namespace PlayLimit.Views;

public partial class RuleDialog : Window
{
    public RuleDialog(AppRule? rule = null)
    {
        InitializeComponent();

        var vm = new RuleDialogViewModel(rule);

        vm.RequestClose += () =>
        {
            DialogResult = true;
            Close();
        };

        DataContext = vm;
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}