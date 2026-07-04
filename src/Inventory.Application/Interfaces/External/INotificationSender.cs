using System.Threading.Tasks;

namespace Inventory.Application.Interfaces.External;

public interface INotificationSender
{
    Task EnviarEmailAsync(string destinatario, string assunto, string corpo);
}