using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record FieldUnavailableEvent(
    string FieldId,
    string? Reason
) : BaseDomainEvent; 