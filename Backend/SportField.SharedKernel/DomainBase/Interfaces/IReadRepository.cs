using SportField.SharedKernel.DomainBase.Entities;
using SportField.SharedKernel.Utils;
using System.Linq.Expressions;

namespace SportField.SharedKernel.DomainBase.Interfaces;

public interface IReadRepository<T, K> where T : class
{
    Task<T> GetSingleAsync(Expression<Func<T, bool>> filter);
    Task<PagedResult<T>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        IEnumerable<OrderByOption> orderByOptions,
        CancellationToken cancellationToken = default);
    Task<PagedResult<T>> GetFilteredAsync(
        Expression<Func<T, bool>> filter,
        IEnumerable<OrderByOption> orderByOptions,
        CancellationToken cancellationToken = default);

    Task<PagedResult<T>> GetFilteredPagedAsync(
        Expression<Func<T, bool>> filter,
        int pageNumber,
        int pageSize,
        IEnumerable<OrderByOption> orderByOptions,
        CancellationToken cancellationToken = default);
}
