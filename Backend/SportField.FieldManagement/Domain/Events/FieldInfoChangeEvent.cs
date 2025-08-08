using SportField.SharedKernel.DomainBase;

namespace SportField.FieldManagement.Domain.Events;

public record FieldInfoChangeEvent (
    Guid FieldId, 
    string Name, 
    string? Description, 
    string? imgUrls, 
    bool IsActive) : BaseDomainEvent
{
}
