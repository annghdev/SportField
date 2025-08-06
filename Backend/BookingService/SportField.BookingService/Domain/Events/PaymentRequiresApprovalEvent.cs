using Common.Abstractions;
using Common.Enums;

namespace SportField.BookingService.Domain.Events;

public record PaymentRequiresApprovalEvent(
    Guid BookingId,
    Guid PaymentId,
    decimal Amount,
    PaymentMethod Method,
    Guid? UserId,
    string? ProofImageUrl,
    DateTime RequestedDate
) : BaseDomainEvent;