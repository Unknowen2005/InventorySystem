using System.Threading.Tasks;
using Inventory.Application.DTOs.Requests;
using Inventory.Application.DTOs.Responses;
using Inventory.Application.Interfaces.Services;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Inventory.Domain.Interfaces.Repositories;

namespace Inventory.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _uow;

    public AuthService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        // Placeholder - implementar validação real de senha e busca no banco
        // Por enquanto, sempre retorna sucesso para fins de teste
        return new LoginResponseDto
        {
            Sucesso = true,
            Mensagem = "Login realizado com sucesso.",
            NomeUsuario = "Usuário Teste",
            Perfil = "Administrador"
        };
    }

    public async Task<LoginResponseDto> RegistrarAsync(LoginRequestDto request)
    {
        // Placeholder
        return new LoginResponseDto
        {
            Sucesso = true,
            Mensagem = "Usuário registrado com sucesso."
        };
    }
}