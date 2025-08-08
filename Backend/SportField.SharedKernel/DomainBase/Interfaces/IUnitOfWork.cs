namespace SportField.SharedKernel.DomainBase.Interfaces;

public interface IUnitOfWork
{
    Task BeginAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
}
