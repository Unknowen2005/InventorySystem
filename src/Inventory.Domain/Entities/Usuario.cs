using Inventory.Domain.Enums;
using Inventory.Shared;

namespace Inventory.Domain.Entities;

public class Usuario : BaseEntity
{
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string SenhaHash { get; private set; }
    public PerfilAcesso Perfil { get; private set; }
    public bool Ativo { get; private set; }
    private Usuario() { } // Para EF Core

    public Usuario(string nome, string email, string senhaHash, PerfilAcesso perfil)
    {
        Nome = nome;
        Email = email;
        SenhaHash = senhaHash;
        Perfil = perfil;
        Ativo = true;
    }

    public void Desativar() => Ativo = false;
    public void Ativar() => Ativo = true;
    public void AlterarPerfil(PerfilAcesso novoPerfil) => Perfil = novoPerfil;
}