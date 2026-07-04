using Inventory.Domain.Enums;
using Inventory.Domain.Exceptions;
using Inventory.Shared;

namespace Inventory.Domain.Entities;

public class Movimentacao : BaseEntity
{
    public Guid ProdutoId { get; private set; }
    public Guid UsuarioId { get; private set; }
    public TipoMovimentacao Tipo { get; private set; }
    public int Quantidade { get; private set; }
    public string? Observacao { get; private set; }
    public DateTime DataHora { get; private set; }

    // Navegação
    public virtual Produto? Produto { get; private set; }
    public virtual Usuario? Usuario { get; private set; }
    private Movimentacao() { } // Para EF Core

    public Movimentacao(Guid produtoId, Guid usuarioId, TipoMovimentacao tipo, int quantidade, string? observacao = null)
    {
        if (quantidade <= 0)
            throw new DomainException("A quantidade da movimentação deve ser positiva.");

        ProdutoId = produtoId;
        UsuarioId = usuarioId;
        Tipo = tipo;
        Quantidade = quantidade;
        Observacao = observacao;
        DataHora = DateTime.Now;
    }
}