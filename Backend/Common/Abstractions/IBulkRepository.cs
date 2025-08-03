namespace Common.Abstractions;

public interface IBulkRepository<T, K> : IWriteRepository<T, K>
    where T : AggregateRoot<K>
{
    Task BulkAddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task BulkUpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task BulkDeleteAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task BulkUpsertAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}
