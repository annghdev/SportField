using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record FieldAvailableEvent(
    string FieldId
) : BaseDomainEvent; 