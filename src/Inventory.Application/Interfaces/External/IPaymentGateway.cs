using System.Threading.Tasks;

namespace Inventory.Application.Interfaces.External;

public interface IPaymentGateway
{
    Task<string> CriarSessaoCheckoutAsync(string plano, string usuarioId);
}