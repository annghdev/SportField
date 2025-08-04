using Common.Abstractions;
using Common.Enums;
using SportField.FieldService.Domain.Events;

namespace SportField.FieldService.Domain.Entities;

public class FieldMaintenance : BaseEntity<string>
{
    public required string FieldId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Scheduled;
    public MaintenanceType Type { get; set; }
    public string? AssignedTo { get; set; }
    public decimal? EstimatedCost { get; set; }
    public decimal? ActualCost { get; set; }
    public string? Notes { get; set; }
    public bool IsRecurring { get; set; }
    public string? RecurrencePattern { get; set; } // JSON for recurring pattern

    // Navigation property
    public virtual Field Field { get; set; } = null!;

    public static FieldMaintenance Schedule(string FieldId, string title, DateTime startTime, DateTime endTime,
        MaintenanceType type, string? description = null, string? assignedTo = null)
    {
        var maintenance = new FieldMaintenance
        {
            FieldId = FieldId,
            Title = title,
            Description = description,
            StartTime = startTime,
            EndTime = endTime,
            Type = type,
            AssignedTo = assignedTo
        };

        maintenance.AddDomainEvent(new FieldMaintenanceScheduledEvent(FieldId, maintenance.Id, startTime, endTime, type));
        return maintenance;
    }

    public void Start()
    {
        Status = MaintenanceStatus.InProgress;

        AddDomainEvent(new FieldMaintenanceStartedEvent(FieldId, Id, Title));
    }

    public void Complete(decimal? actualCost = null, string? notes = null)
    {
        Status = MaintenanceStatus.Completed;
        ActualCost = actualCost;
        Notes = notes;

        AddDomainEvent(new FieldMaintenanceCompletedEvent(FieldId, Id, Title, actualCost));
    }

    public void Cancel(string? reason = null)
    {
        Status = MaintenanceStatus.Cancelled;
        Notes = reason;

        AddDomainEvent(new FieldMaintenanceCancelledEvent(FieldId, Id, Title, reason));
    }

    public void Reschedule(DateTime newStartTime, DateTime newEndTime, string? reason = null)
    {
        var oldStartTime = StartTime;
        var oldEndTime = EndTime;

        StartTime = newStartTime;
        EndTime = newEndTime;
        Notes = reason;

        AddDomainEvent(new FieldMaintenanceRescheduledEvent(FieldId, Id, oldStartTime, oldEndTime, newStartTime, newEndTime));
    }

    public bool IsActiveOnDate(DateTime date)
    {
        return Status != MaintenanceStatus.Cancelled
               && date.Date >= StartTime.Date
               && date.Date <= EndTime.Date;
    }

    public bool ConflictsWith(DateTime startTime, DateTime endTime)
    {
        return Status != MaintenanceStatus.Cancelled
               && StartTime < endTime
               && EndTime > startTime;
    }

    public TimeSpan GetDuration()
    {
        return EndTime - StartTime;
    }
}
