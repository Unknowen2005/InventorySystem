using System;
using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Inventory.Infrastructure.Persistence.Context;
using Inventory.Infrastructure.Persistence.Repositories;
using Inventory.Domain.Interfaces.Repositories;
using Inventory.Application.Services;
using Inventory.Application.Interfaces.Services;
using Inventory.WPF.Services;
using Inventory.WPF.ViewModels;
using Inventory.WPF.ViewModels.Account;

namespace Inventory.WPF;

public partial class App : System.Windows.Application
{
    public static IServiceProvider ServiceProvider { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // 1. Carregar configurações
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        var configuration = builder.Build();

        // 2. Configurar DI
        var services = new ServiceCollection();

        services.AddSingleton<IConfiguration>(configuration);

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();

        services.AddScoped<IEstoqueService, EstoqueService>();
        services.AddScoped<IAuthService, AuthService>();

        // ---- NOVOS REGISTROS ----
        services.AddScoped<LoginViewModel>();
        services.AddScoped<MainViewModel>();

        services.AddSingleton<Views.Account.LoginPage>();
        services.AddSingleton<Views.MainPage>();

        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<MainWindow>();

        ServiceProvider = services.BuildServiceProvider();

        // 3. Migrações
        using (var scope = ServiceProvider.CreateScope())
        {
            try
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();
                System.Diagnostics.Debug.WriteLine("Migrações aplicadas com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao aplicar migrações: {ex.Message}",
                                "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 4. Abrir MainWindow
        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        (ServiceProvider as IDisposable)?.Dispose();
        base.OnExit(e);
    }
}