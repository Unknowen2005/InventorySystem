using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces.Repositories;
using Inventory.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Produto>> GetByCategoriaAsync(Guid categoriaId)
        => await _dbSet.Where(p => p.CategoriaId == categoriaId).ToListAsync();

    public async Task<IEnumerable<Produto>> GetLowStockAsync(int threshold)
        => await _dbSet.Where(p => p.QuantidadeEmEstoque <= threshold).ToListAsync();

    public async Task<Produto?> GetByNomeAsync(string nome)
        => await _dbSet.FirstOrDefaultAsync(p => p.Nome.ToLower() == nome.ToLower());

    public override async Task<Produto?> GetByIdAsync(Guid id)
        => await _dbSet.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.id == id);
}