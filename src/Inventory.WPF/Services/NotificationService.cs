using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace Inventory.WPF.Services;

public class NotificationService
{
    private readonly TaskbarIcon _trayIcon;

    public NotificationService()
    {
        _trayIcon = new TaskbarIcon
        {
            Icon = System.Drawing.Icon.ExtractAssociatedIcon(
                System.Windows.Forms.Application.ExecutablePath),
            ToolTipText = "Inventory System"
        };
    }

    public void ShowUpdateNotification(string version)
    {
        _trayIcon.ShowBalloonTip(
            "Atualização disponível! 🎉",
            $"Nova versão {version} está disponível.\nClique aqui para atualizar.",
            BalloonIcon.Info
        );
    }

    public void ShowErrorNotification(string message)
    {
        _trayIcon.ShowBalloonTip(
            "Erro no Inventory System",
            message,
            BalloonIcon.Error
        );
    }

    public void ShowSuccessNotification(string message)
    {
        _trayIcon.ShowBalloonTip(
            "Inventory System",
            message,
            BalloonIcon.Info
        );
    }

    public void Dispose()
    {
        _trayIcon.Dispose();
    }
}