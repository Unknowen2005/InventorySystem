using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Inventory.WPF.ViewModels;

namespace Inventory.WPF.Services;

public class NavigationService : INavigationService
{
    private Frame? _frame;
    private readonly IServiceProvider _serviceProvider;
    private readonly Stack<BaseViewModel> _history = new();

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Initialize(Frame frame)
    {
        _frame = frame;
    }

    // Método genérico (usado para navegação inicial)
    public void NavigateTo<TViewModel>() where TViewModel : BaseViewModel
    {
        NavigateTo(typeof(TViewModel));
    }

    // Método que aceita Type (usado no GoBack)
    public void NavigateTo(Type viewModelType)
    {
        if (_frame == null) return;

        // Resolve a ViewModel via DI
        var viewModel = _serviceProvider.GetService(viewModelType) as BaseViewModel;
        if (viewModel == null) return;

        // Converte o nome da ViewModel para o nome da Page (convenção)
        var viewModelTypeName = viewModelType.FullName!;
        var pageTypeName = viewModelTypeName.Replace("ViewModels", "Views").Replace("ViewModel", "Page");
        var pageType = Type.GetType(pageTypeName);

        if (pageType == null) return;

        // Resolve a Page via DI
        var page = _serviceProvider.GetService(pageType) as Page;
        if (page == null) return;

        // Conecta ViewModel à View
        page.DataContext = viewModel;

        // Adiciona ao histórico (se não for a primeira navegação)
        if (_history.Count == 0 || _history.Peek() != viewModel)
        {
            _history.Push(viewModel);
        }

        _frame.Navigate(page);
    }

    public void GoBack()
    {
        if (_history.Count > 1)
        {
            _history.Pop(); // Remove a atual
            var previousViewModel = _history.Peek();

            // Usa o novo método com Type
            NavigateTo(previousViewModel.GetType());
        }
    }

    public bool CanGoBack => _history.Count > 1;
}