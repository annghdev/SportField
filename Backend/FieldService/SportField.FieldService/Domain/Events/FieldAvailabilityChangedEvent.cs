using Common.Abstractions;
using System.Collections.Generic;

namespace SportField.FieldService.Domain.Events;

/// <summary>
/// Event raised when the availability of specific time slots for a field is blocked or unblocked by an admin.
/// This is typically used for special events or temporary closures.
/// </summary>
/// <param name="FieldId">The ID of the field whose availability changed.</param>
/// <param name="TimeSlotIds">The list of time slot IDs affected by the change.</param>
/// <param name="FromDate">The start date of the availability block.</param>
/// <param name="ToDate">The end date of the availability block.</param>
/// <param name="Reason">The reason for the availability change.</param>
/// <param name="IsBlocked">A boolean indicating whether the slots are being blocked (true) or unblocked (false).</param>
public record FieldAvailabilityChangedEvent(
    Guid FieldId,
    IReadOnlyList<string> TimeSlotIds,
    DateTime FromDate,
    DateTime? ToDate,
    string? Reason,
    bool IsBlocked
) : BaseDomainEvent;
