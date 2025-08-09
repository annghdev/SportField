using SportField.SharedKernel.DomainBase;

namespace SportField.FieldManagement.Domain.Events;

public record TimeFrameActiveChangeEvent(string TimeSlotId, bool IsActive) : BaseDomainEvent;