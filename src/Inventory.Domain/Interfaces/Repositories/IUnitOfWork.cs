using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IProdutoRepository ProdutoRepository { get; }
    IRepository<Categoria> CategoriaRepository { get; }
    IRepository<Movimentacao> MovimentacaoRepository { get; }
    IRepository<Usuario> UsuarioRepository { get; }

    Task<int> CommitAsync();
    Task RollbackAsync();
}