using MediatR;

namespace Common.Abstractions;

public interface IDomainEvent : INotification
{
    DateTime OccurredDate { get; }
}
