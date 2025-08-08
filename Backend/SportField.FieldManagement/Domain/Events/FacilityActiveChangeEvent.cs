using SportField.SharedKernel.DomainBase;

namespace SportField.FieldManagement.Domain.Events
{
    public record FacilityActiveChangeEvent(Guid FacilityId, bool IsActive) : BaseDomainEvent
    {
    }
}
