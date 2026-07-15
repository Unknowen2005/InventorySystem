using System;
using System.Windows;
using Velopack;

namespace Inventory.WPF.Views
{
    public partial class UpdateAvailableWindow : Window
    {
        private readonly UpdateManager _updateManager;
        private readonly UpdateInfo _update; // CORREÇÃO: Tipo correto é UpdateInfo

        public string Mensagem { get; }

        public UpdateAvailableWindow(UpdateManager updateManager, UpdateInfo update)
        {
            InitializeComponent();
            _updateManager = updateManager;
            _update = update;

            // CORREÇÃO: Velopack armazena os dados da versão em TargetFullRelease
            Mensagem = $"Nova versão {_update.TargetFullRelease.Version} está disponível.\n" +
                       $"Tamanho: {FormatSize(_update.TargetFullRelease.Size)}\n" +
                       $"Deseja instalar agora?";

            DataContext = this;
        }

        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            BtnUpdate.IsEnabled = false;
            BtnLater.IsEnabled = false;

            if (ProgressBar != null)
                ProgressBar.Visibility = Visibility.Visible;

            try
            {
                // CORREÇÃO: Velopack usa Action<int> para progresso de 0 a 100
                Action<int> progressCallback = p =>
                {
                    // Como a Action pode rodar fora da Thread principal da UI, 
                    // usamos o Dispatcher para atualizar a ProgressBar com segurança.
                    Dispatcher.Invoke(() =>
                    {
                        if (ProgressBar != null)
                            ProgressBar.Value = p;
                    });
                };

                // Baixar a atualização usando a assinatura correta do Velopack
                await _updateManager.DownloadUpdatesAsync(_update, progressCallback);

                // Aplicar e reiniciar usando o mesmo objeto UpdateInfo
                _updateManager.ApplyUpdatesAndRestart(_update);

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar: {ex.Message}", "Erro",
                               MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
            }
        }

        private void BtnLater_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private string FormatSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}