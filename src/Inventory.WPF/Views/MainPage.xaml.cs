using System.Windows;
using System.Windows.Controls;

namespace Inventory.WPF.Views;

public partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void Sair_Click(object sender, RoutedEventArgs e)
    {
       System.Windows.Application.Current.Shutdown();
    }
}