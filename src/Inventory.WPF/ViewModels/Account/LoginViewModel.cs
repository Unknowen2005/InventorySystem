using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Inventory.Application.DTOs.Requests;
using Inventory.Application.Interfaces.Services;
using Inventory.WPF.Commands;
using Inventory.WPF.Services;
using Inventory.WPF.ViewModels;

namespace Inventory.WPF.ViewModels.Account;

public class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;

    private string _email = "";
    public string Email
    {
        get => _email;
        set => SetField(ref _email, value);
    }

    private string _mensagemErro = "";
    public string MensagemErro
    {
        get => _mensagemErro;
        set => SetField(ref _mensagemErro, value);
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set => SetField(ref _isLoading, value);
    }

    public ICommand LoginCommand { get; }

    public LoginViewModel(IAuthService authService, INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;

        LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
    }

    private bool CanExecuteLogin(object? parameter)
    {
        return !IsLoading && !string.IsNullOrWhiteSpace(Email);
    }

    private async void ExecuteLogin(object? parameter)
    {
        if (parameter is not PasswordBox passwordBox) return;

        IsLoading = true;
        MensagemErro = "";
        (LoginCommand as RelayCommand)?.RaiseCanExecuteChanged();

        try
        {
            var request = new LoginRequestDto
            {
                Email = Email,
                Senha = passwordBox.Password
            };

            var result = await _authService.LoginAsync(request);

            if (result.Sucesso)
            {
                // Login bem-sucedido: vai para a MainPage
                _navigationService.NavigateTo<MainViewModel>();
            }
            else
            {
                MensagemErro = result.Mensagem;
            }
        }
        catch (Exception ex)
        {
            MensagemErro = $"Erro ao realizar login: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
            (LoginCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }
}