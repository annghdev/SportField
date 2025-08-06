namespace Common.Abstractions;

public interface IFullRepository<T, K> : IReadRepository<T, K>, IBulkRepository<T, K>
    where T : IAggregateRoot
{
}
