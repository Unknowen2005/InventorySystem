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
    public partial class App :  System.Windows.Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        protected override async void OnStartup(StartupEventArgs e)
        {
            // Isso deve rodar ANTES de qualquer outra lógica do app.
            // Se o Velopack estiver apenas instalando/atualizando o app em background,
            // ele vai processar os ganchos aqui e fechar o processo imediatamente.
            VelopackApp.Build().Run();
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
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            // Repositórios
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();

            // Serviços
            services.AddScoped<IEstoqueService, EstoqueService>();
            services.AddScoped<IAuthService, AuthService>();

            // ViewModels
            services.AddScoped<LoginViewModel>();
            services.AddScoped<MainViewModel>();

            // Views
            services.AddSingleton<Views.Account.LoginPage>();
            services.AddSingleton<Views.MainPage>();

            // Navegação
            services.AddSingleton<INavigationService, NavigationService>();

            // Janela principal
            services.AddSingleton<MainWindow>();

            ServiceProvider = services.BuildServiceProvider();

            // 3. Aplicar migrações
            using (var scope = ServiceProvider.CreateScope())
            {
                try
                {
                    var dbContext = scope.ServiceProvider
                        .GetRequiredService<AppDbContext>();

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

            // 4. Verificar atualizações
            await InitializeVelopackAsync();

            // 5. Abrir janela principal
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        /// <summary>
        /// Verifica e instala atualizações usando Velopack.
        /// </summary>
        private async Task InitializeVelopackAsync()
        {
            try
            {
                // CORREÇÃO: URL limpa do repositório, sem o ".git" no final
                var source = new GithubSource(
                    "https://github.com/Unknowen2005/InventorySystem",
                    accessToken: null,
                    prerelease: false);

                var updateManager = new UpdateManager(source);

                var update = await updateManager.CheckForUpdatesAsync();

                if (update == null)
                {
                    System.Diagnostics.Debug.WriteLine("Nenhuma atualização encontrada.");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"Nova versão encontrada: {update.TargetFullRelease.Version}");

                // DICA: Use .Version para exibir um texto mais limpo (ex: "1.0.1") no MessageBox
                var result = MessageBox.Show(
                    $"Foi encontrada a versão {update.TargetFullRelease.Version}.\n\nDeseja atualizar agora?",
                    "Atualização disponível",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Baixar atualização
                    await updateManager.DownloadUpdatesAsync(update);

                    // Aplicar atualização e reiniciar
                    updateManager.ApplyUpdatesAndRestart(update);

                    // CORREÇÃO: Força o fechamento da instância atual para que o Velopack 
                    // possa substituir os arquivos sem erros de "arquivo em uso".
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao verificar atualizações: {ex.Message}");
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