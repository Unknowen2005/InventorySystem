using System;
using System.IO;
using System.Threading.Tasks;
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
using Velopack;
using Velopack.Sources;

namespace Inventory.WPF
{
    public partial class App : System.Windows.Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            // 1. Velopack obrigatoriamente primeiro (processa ganchos de instalação/atualização)
            VelopackApp.Build().Run();
            base.OnStartup(e);

            // 2. Carregar configurações
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();

            // 3. Configurar Injeção de Dependência (DI)
            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Repositórios e Serviços (Transient para evitar retenção de estado incorreta)
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IProdutoRepository, ProdutoRepository>();
            services.AddTransient<IEstoqueService, EstoqueService>();
            services.AddTransient<IAuthService, AuthService>();

            // Navegação e Notificação (Singleton pois mantêm estado global do app)
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<NotificationService>();

            // ViewModels e Views como Transient para evitar vazamento de dados entre telas
            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<Views.Account.LoginPage>();
            services.AddTransient<Views.MainPage>();

            // Janela principal
            services.AddTransient<MainWindow>();

            ServiceProvider = services.BuildServiceProvider();

            // 4. Aplicar migrações do Banco de Dados
            ApplyMigrations();

            // 5. Inicializar rotina assíncrona de boot (Verificação de update e abertura de tela)
            RunBootstrapperAsync();
        }

        private void ApplyMigrations()
        {
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
                    MessageBox.Show(
                        $"Erro ao aplicar migrações:\n\n{ex.Message}",
                        "Erro",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private async void RunBootstrapperAsync()
        {
            // Primeiro verifica atualização em background sem bloquear a UI imediatamente
            await InitializeVelopackAsync();

            // Abre a MainWindow após a rotina de atualização terminar (se não houver reinicialização)
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private async Task InitializeVelopackAsync()
        {
            try
            {
                var source = new GithubSource(
                    "https://github.com/Unknowen2005/InventorySystem",
                    accessToken: null,
                    prerelease: false);

                // No Velopack, o UpdateManager não é IDisposable.
                var updateManager = new UpdateManager(source);
                var update = await updateManager.CheckForUpdatesAsync();

                if (update == null)
                {
                    System.Diagnostics.Debug.WriteLine("Nenhuma atualização encontrada.");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"✅ Nova versão disponível: {update.TargetFullRelease.Version}");

                // 1. Mostrar notificação na bandeja do sistema
                var notificationService = ServiceProvider.GetRequiredService<NotificationService>();
                notificationService.ShowUpdateNotification(update.TargetFullRelease.Version.ToString());

                // 2. Aguardar 5 segundos de forma assíncrona antes de exibir o pop-up
                await Task.Delay(5000);

                // 3. Exibir a janela personalizada (ShowDialog bloqueia a execução aqui até fechar)
                var updateWindow = new Views.UpdateAvailableWindow(updateManager, update);
                var result = updateWindow.ShowDialog();

                if (result == true)
                {
                    System.Diagnostics.Debug.WriteLine("Baixando atualizações...");
                    await updateManager.DownloadUpdatesAsync(update);

                    System.Diagnostics.Debug.WriteLine("Aplicando atualizações e reiniciando...");
                    updateManager.ApplyUpdatesAndRestart(update);

                    // Força o encerramento imediato para que o Velopack substitua os executáveis sem travar
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Erro no Velopack: {ex.Message}");
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (ServiceProvider is IDisposable disposable)
                disposable.Dispose();

            base.OnExit(e);
        }
    }
}