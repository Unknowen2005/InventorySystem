using System.Threading.Tasks;

namespace Inventory.Application.Interfaces.External;

public interface ILicenseValidator
{
    Task<bool> ValidarLicencaAsync(string chave, string hardwareId);
}