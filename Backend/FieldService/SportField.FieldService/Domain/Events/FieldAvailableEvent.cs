using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record FieldAvailableEvent(
    Guid FieldId
) : BaseDomainEvent; 