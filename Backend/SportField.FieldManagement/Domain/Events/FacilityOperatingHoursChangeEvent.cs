using SportField.SharedKernel.DomainBase;

namespace SportField.FieldManagement.Domain.Events;

public record FacilityOperatingHoursChangeEvent(Guid FacilityId, TimeOnly OpenTime, TimeOnly CloseTime) : BaseDomainEvent
{
}
