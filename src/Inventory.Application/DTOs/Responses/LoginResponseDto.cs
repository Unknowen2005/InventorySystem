namespace Inventory.Application.DTOs.Responses;

public class LoginResponseDto
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; }
    public string Token { get; set; } // Para futura autenticação JWT
    public string NomeUsuario { get; set; }
    public string Perfil { get; set; }
}