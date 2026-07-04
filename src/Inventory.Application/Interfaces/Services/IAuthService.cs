using System.Threading.Tasks;
using Inventory.Application.DTOs.Requests;
using Inventory.Application.DTOs.Responses;

namespace Inventory.Application.Interfaces.Services;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<LoginResponseDto> RegistrarAsync(LoginRequestDto request); // Opcional
}