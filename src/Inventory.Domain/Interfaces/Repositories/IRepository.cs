
using Inventory.Domain.Specifications;

namespace Inventory.Domain.Interfaces.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAsync(ISpecification<T> spec); // Para Specifications
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}