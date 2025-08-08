using SportField.SharedKernel.DomainBase.Entities;
using System.Linq.Expressions;

namespace SportField.SharedKernel.DomainBase.Interfaces;

public interface IRepository<T, K>
    where T : IAggregateRoot
{
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null!, CancellationToken cancellationToken = default);
    Task<T> GetByIdAsync(K id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(K id);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
