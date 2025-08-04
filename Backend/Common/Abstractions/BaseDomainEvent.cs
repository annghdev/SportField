using MediatR;
using System;

namespace Common.Abstractions;

public abstract record BaseDomainEvent : INotification
{
    public string Id { get; init; } = Guid.CreateVersion7().ToString();
    public DateTimeOffset OccurredOn { get; init; } = DateTimeOffset.UtcNow;
}
