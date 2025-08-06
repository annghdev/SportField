using System;
using System.ComponentModel.DataAnnotations;

namespace SportField.BookingService.Application.ReadModels;

/// <summary>
/// Represents a read-only, simplified copy of a TimeSlot from the FieldService.
/// This data is synchronized via domain events and stored locally in the BookingService
/// database to optimize read operations and provide necessary information for the calendar.
/// </summary>
public class TimeSlotProjection
{
    /// <summary>
    /// The unique identifier of the time slot (e.g., "08:00-09:00").
    /// This ID is synchronized from the original TimeSlot entity in FieldService.
    /// </summary>
    [Key]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The start time of the slot.
    /// </summary>
    public TimeOnly StartTime { get; set; }

    /// <summary>
    /// The end time of the slot.
    /// </summary>
    public TimeOnly EndTime { get; set; }

    /// <summary>
    /// The display name for the time slot, e.g., "08:00 - 09:00".
    /// This can be pre-calculated and stored here for display efficiency.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;
}
