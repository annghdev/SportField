using Common.Abstractions;
using Common.Enums;

namespace SportField.BookingService.Domain.Events;

public record PaymentCompletedEvent(
    Guid BookingId,
    Guid PaymentId,
    decimal Amount,
    PaymentMethod Method,
    string? TransactionId,
    DateTime CompletedDate
) : BaseDomainEvent;