using System.ComponentModel.DataAnnotations;

namespace SportField.BookingService.Application.ReadModels;

/// <summary>
/// Represents a read-only, simplified copy of a Field from the FieldService.
/// This data is synchronized via domain events and stored locally in the BookingService
/// database to optimize read operations for the calendar and avoid cross-service queries.
/// </summary>
public class FieldProjection
{
    /// <summary>
    /// The unique identifier of the field, which is the primary key.
    /// This ID is synchronized from the original Field entity in FieldService.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the field, used for display purposes in the booking calendar.
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the facility this field belongs to, used for filtering and grouping.
    /// </summary>
    public Guid FacilityId { get; set; }
    
    /// <summary>
    /// The active status of the field. If false, the field cannot be booked.
    /// This is synchronized from FieldService.
    /// </summary>
    public bool IsActive { get; set; }
}
