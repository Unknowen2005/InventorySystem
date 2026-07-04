using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<IEnumerable<Produto>> GetByCategoriaAsync(Guid categoriaId);
    Task<IEnumerable<Produto>> GetLowStockAsync(int threshold);
    Task<Produto?> GetByNomeAsync(string nome);
}