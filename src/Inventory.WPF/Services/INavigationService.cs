using System;
using System.Windows.Controls;
using Inventory.WPF.ViewModels;

namespace Inventory.WPF.Services;

public interface INavigationService
{
    void Initialize(Frame frame);
    void NavigateTo<TViewModel>() where TViewModel : BaseViewModel;
    void NavigateTo(Type viewModelType); // <-- NOVO MÉTODO
    void GoBack();
    bool CanGoBack { get; }
}