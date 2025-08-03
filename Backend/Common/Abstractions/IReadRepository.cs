using System.Linq.Expressions;

namespace Common.Abstractions;

public interface IReadRepository<T, K> where T : AggregateRoot<K>
{
    Task<T> GetByIdAsync(K id);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<T>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        OrderByRequest? orderByRequest,
        CancellationToken cancellationToken = default);
    Task<PagedResult<T>> GetFilteredAsync(
        Expression<Func<T, bool>> filter,
        OrderByRequest? orderByRequest,
        CancellationToken cancellationToken = default);

    Task<PagedResult<T>> GetFilteredPagedAsync(
        Expression<Func<T, bool>> filter,
        int pageNumber,
        int pageSize,
        OrderByRequest? orderByRequest,
        CancellationToken cancellationToken = default);
}
