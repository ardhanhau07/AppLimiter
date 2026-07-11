using System.Windows;
using AppLimit.ViewModels;

namespace AppLimit.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}