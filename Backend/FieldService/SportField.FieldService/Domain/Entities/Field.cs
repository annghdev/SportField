using Common.Abstractions;
using Common.Enums;

namespace SportField.FieldService.Domain.Entities;

public class Field : AggregateRoot<string>
{
    public required string Name { get; set; }
    public required string FacilityId { get; set; }
    public FieldType Type { get; set; }
    public decimal BasePrice { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public int Capacity { get; set; } = 10; // Số người tối đa
    // Navigation properties
    public virtual Facility Facility { get; set; } = null!;
    public virtual ICollection<TimeSlot> TimeSlots { get; set; } = [];
    public virtual ICollection<FieldPricing> FieldPricings { get; set; } = [];
    public virtual ICollection<FieldOperatingHours> OperatingHours { get; set; } = [];
    public virtual ICollection<FieldMaintenance> MaintenanceSchedules { get; set; } = [];
    public virtual ICollection<FieldAvailability> BlockedAvailabilities { get; set; } = [];
}
