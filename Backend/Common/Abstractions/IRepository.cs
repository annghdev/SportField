namespace Common.Abstractions;

public interface IRepository<T, K> : IReadRepository<T, K>, IWriteRepository<T, K>
    where T : AggregateRoot<K>
{
}
