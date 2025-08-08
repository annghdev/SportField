using SportField.SharedKernel.DomainBase.Entities;

namespace SportField.SharedKernel.DomainBase.Interfaces;

public interface IBulkRepository<T, K>
    where T : IAggregateRoot
{
    Task BulkAddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task BulkUpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task BulkDeleteAsync(IEnumerable<K> ids, CancellationToken cancellationToken = default);
    Task BulkUpsertAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}
