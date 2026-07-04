using Inventory.WPF.Services;
using Inventory.WPF.ViewModels.Account;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Inventory.WPF;

public partial class MainWindow : Window
{
    private readonly INavigationService _navigationService;

    public MainWindow(INavigationService navigationService)
    {
        InitializeComponent();
        _navigationService = navigationService;

        // Inicializa a navegação com o Frame
        _navigationService.Initialize(MainFrame);

        // Navega para a página de Login
        _navigationService.NavigateTo<LoginViewModel>();
    }
}