using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record FieldUnavailableEvent(
    Guid FieldId,
    string? Reason
) : BaseDomainEvent; 