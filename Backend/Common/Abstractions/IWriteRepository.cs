namespace Common.Abstractions;

public interface IWriteRepository<T, K> where T : AggregateRoot<K>
{
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task DeleteAsync(K id);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
