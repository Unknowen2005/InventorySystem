using Inventory.Shared;

namespace Inventory.Domain.Entities;

public class Categoria : BaseEntity
{
    public string Nome { get; private set; }
    public string? Descricao { get; private set; }

    private Categoria() { } // Para EF Core
    public Categoria(string nome, string? descricao = null)
    {
        Nome = nome;
        Descricao = descricao;
    }

    public void Atualizar(string nome, string? descricao)
    {
        Nome = nome;
        Descricao = descricao;
        DataModificacao = DateTime.Now;
    }
}