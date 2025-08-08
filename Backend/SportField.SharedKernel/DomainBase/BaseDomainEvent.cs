using MediatR;

namespace SportField.SharedKernel.DomainBase;

public abstract record BaseDomainEvent : INotification
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
