using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces.Repositories;
using Inventory.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;

        ProdutoRepository = new ProdutoRepository(_context);
        CategoriaRepository = new Repository<Categoria>(_context);
        MovimentacaoRepository = new Repository<Movimentacao>(_context);
        UsuarioRepository = new Repository<Usuario>(_context);
    }

    public IProdutoRepository ProdutoRepository { get; }
    public IRepository<Categoria> CategoriaRepository { get; }
    public IRepository<Movimentacao> MovimentacaoRepository { get; }
    public IRepository<Usuario> UsuarioRepository { get; }

    public async Task<int> CommitAsync()
        => await _context.SaveChangesAsync();

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}