using System.Windows;
using PlayLimit.ViewModels;

namespace PlayLimit.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}