using Inventory.WPF.ViewModels;

namespace Inventory.WPF.ViewModels;

public class MainViewModel : BaseViewModel
{
    private string _titulo = "Dashboard - Inventory System";
    public string Titulo
    {
        get => _titulo;
        set => SetField(ref _titulo, value);
    }

    public MainViewModel()
    {
        // Aqui você vai carregar dados do estoque, gráficos, etc.
    }
}