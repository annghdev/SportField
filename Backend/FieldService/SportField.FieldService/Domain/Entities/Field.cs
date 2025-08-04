using Common.Abstractions;
using Common.Enums;
using SportField.FieldService.Domain.Events;
using SportField.FieldService.Domain.Exceptions;

namespace SportField.FieldService.Domain.Entities;

public class Field : AggregateRoot
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

    public static Field Create(string name, string facilityId, FieldType type, decimal basePrice, int capacity = 10, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidFieldNameException("Field name cannot be empty.");

        if (basePrice < 0)
            throw new ArgumentException("Base price cannot be negative.");

        return new Field
        {
            Name = name,
            FacilityId = facilityId,
            Type = type,
            BasePrice = basePrice,
            Capacity = capacity,
            Description = description,
            IsActive = true
        };
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new InvalidFieldNameException("Field name cannot be empty.");

        if (newName == Name)
            return;

        Name = newName;
    }

    public void SetActive(bool isActive)
    {
        if (IsActive == isActive)
            return;

        IsActive = isActive;
    }

    public void ValidateStatus()
    {
        if (!IsActive) // Simplified condition for invalid status
            throw new InvalidFieldStatusException("Inactive");
    }

    public void UpdateInfo(string name, string? description, int capacity)
    {
        Name = name;
        Description = description;
        Capacity = capacity;
        ModifiedDate = DateTime.UtcNow;
    }

    public void UpdateBasePrice(decimal newPrice, string modifiedBy)
    {
        BasePrice = newPrice;
        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
    }

    public void SetUnavailable(string modifiedBy, string? reason = null)
    {
        IsActive = false;
        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow;

        AddDomainEvent(new FieldUnavailableEvent(Id, reason));
    }

    public void SetAvailable(string modifiedBy)
    {
        IsActive = true;
        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow;

        AddDomainEvent(new FieldAvailableEvent(Id));
    }

    public void SetOperatingHours(DayOfWeek dayOfWeek, TimeOnly openTime, TimeOnly closeTime, string modifiedBy)
    {
        var existingHours = OperatingHours.FirstOrDefault(h => h.DayOfWeek == dayOfWeek);
        if (existingHours != null)
        {
            existingHours.UpdateHours(openTime, closeTime);
        }
        else
        {
            var newHours = FieldOperatingHours.Create(Id, dayOfWeek, openTime, closeTime);
            OperatingHours.Add(newHours);
        }

        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
    }

    public void CloseForDay(DayOfWeek dayOfWeek, string modifiedBy, string? reason = null)
    {
        var operatingHours = OperatingHours.FirstOrDefault(h => h.DayOfWeek == dayOfWeek);
        if (operatingHours != null)
        {
            operatingHours.CloseForDay(reason);
        }

        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
    }

    public void OpenForDay(DayOfWeek dayOfWeek, string modifiedBy)
    {
        var operatingHours = OperatingHours.FirstOrDefault(h => h.DayOfWeek == dayOfWeek);
        if (operatingHours != null)
        {
            operatingHours.OpenForDay();
        }

        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
    }

    public void SetPricing(string timeSlotId, decimal price, string modifiedBy, DayOfWeek? dayOfWeek = null, DateTime? effectiveFrom = null, DateTime? effectiveTo = null)
    {
        var existingPricing = FieldPricings.FirstOrDefault(p => p.TimeSlotId == timeSlotId && p.DayOfWeek == dayOfWeek);

        if (existingPricing != null)
        {
            existingPricing.UpdatePrice(price);
        }
        else
        {
            var newPricing = FieldPricing.Create(Id, timeSlotId, price, dayOfWeek, effectiveFrom, effectiveTo);
            FieldPricings.Add(newPricing);
        }

        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
    }

    public void RemovePricing(string pricingId, string modifiedBy)
    {
        var pricingToRemove = FieldPricings.FirstOrDefault(p => p.Id == pricingId);
        if (pricingToRemove != null)
        {
            // Instead of removing, we deactivate it to keep history.
            pricingToRemove.IsActive = false;
        }

        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
    }

    public FieldMaintenance ScheduleMaintenance(string title, DateTime startTime, DateTime endTime,
        MaintenanceType type, string scheduledBy, string? description = null, string? assignedTo = null)
    {
        // Check for conflicts with existing maintenance
        var conflictingMaintenance = MaintenanceSchedules
            .Where(m => m.ConflictsWith(startTime, endTime))
            .FirstOrDefault();

        if (conflictingMaintenance != null)
        {
            throw new InvalidOperationException($"Maintenance conflicts with existing maintenance: {conflictingMaintenance.Title}");
        }

        var maintenance = FieldMaintenance.Schedule(Id, title, startTime, endTime, type, description, assignedTo);
        MaintenanceSchedules.Add(maintenance);

        ModifiedBy = scheduledBy;
        ModifiedDate = DateTime.UtcNow;

        return maintenance;
    }

    public void StartMaintenance(string maintenanceId, string startedBy)
    {
        var maintenance = MaintenanceSchedules.FirstOrDefault(m => m.Id == maintenanceId);
        if (maintenance == null) throw new KeyNotFoundException("Maintenance schedule not found.");

        maintenance.Start();
        ModifiedBy = startedBy;
        ModifiedDate = DateTime.UtcNow;
    }

    public void CompleteMaintenance(string maintenanceId, string completedBy, decimal? actualCost = null, string? notes = null)
    {
        var maintenance = MaintenanceSchedules.FirstOrDefault(m => m.Id == maintenanceId);
        if (maintenance == null) throw new KeyNotFoundException("Maintenance schedule not found.");

        maintenance.Complete(actualCost, notes);
        ModifiedBy = completedBy;
        ModifiedDate = DateTime.UtcNow;
    }

    public void CancelMaintenance(string maintenanceId, string cancelledBy, string? reason = null)
    {
        var maintenance = MaintenanceSchedules.FirstOrDefault(m => m.Id == maintenanceId);
        if (maintenance == null) throw new KeyNotFoundException("Maintenance schedule not found.");

        maintenance.Cancel(reason);
        ModifiedBy = cancelledBy;
        ModifiedDate = DateTime.UtcNow;
    }

    public void RescheduleMaintenance(string maintenanceId, DateTime newStartTime, DateTime newEndTime, string rescheduledBy, string? reason = null)
    {
        var maintenance = MaintenanceSchedules.FirstOrDefault(m => m.Id == maintenanceId);
        if (maintenance == null) throw new KeyNotFoundException("Maintenance schedule not found.");

        // Optional: Add conflict check here as well before rescheduling
        var conflictingMaintenance = MaintenanceSchedules
            .Where(m => m.Id != maintenanceId && m.ConflictsWith(newStartTime, newEndTime))
            .FirstOrDefault();

        if (conflictingMaintenance != null)
        {
            throw new InvalidOperationException($"Rescheduling conflicts with existing maintenance: {conflictingMaintenance.Title}");
        }

        maintenance.Reschedule(newStartTime, newEndTime, reason);
        ModifiedBy = rescheduledBy;
        ModifiedDate = DateTime.UtcNow;
    }

    public void BlockAvailability(string timeSlotId, DateTime fromDate, string blockedBy, DateTime? toDate = null, string? reason = null)
    {
        // Optional: Check for conflicts with existing bookings if this service has access to that info.
        // For now, we just add the block.

        var existingBlock = BlockedAvailabilities.FirstOrDefault(b => b.TimeSlotId == timeSlotId && b.IsBlockedOnDate(fromDate));
        if (existingBlock != null)
        {
            existingBlock.UpdateBlockPeriod(fromDate, toDate);
        }
        else
        {
            var block = FieldAvailability.CreateBlock(Id, timeSlotId, fromDate, toDate, reason);
            BlockedAvailabilities.Add(block);
        }

        ModifiedBy = blockedBy;
        ModifiedDate = DateTime.UtcNow;
    }

    public void UnblockAvailability(string availabilityId, string unblockedBy)
    {
        var block = BlockedAvailabilities.FirstOrDefault(b => b.Id == availabilityId);
        if (block != null)
        {
            block.Unblock();
        }

        ModifiedBy = unblockedBy;
        ModifiedDate = DateTime.UtcNow;
    }

    public bool IsOperatingOnDateTime(DateTime dateTime)
    {
        if (!IsActive) return false;

        var dayOperatingHours = OperatingHours.FirstOrDefault(h => h.DayOfWeek == dateTime.DayOfWeek);
        if (dayOperatingHours == null || !dayOperatingHours.IsActive || dayOperatingHours.IsClosed)
            return false;

        var timeOnly = TimeOnly.FromDateTime(dateTime);
        return dayOperatingHours.IsWithinOperatingHours(timeOnly);
    }

    public bool HasMaintenanceOnDateTime(DateTime dateTime)
    {
        return MaintenanceSchedules.Any(m => m.ConflictsWith(dateTime, dateTime.AddMinutes(1)));
    }

    public bool IsAvailableForBooking(DateTime dateTime)
    {
        return IsOperatingOnDateTime(dateTime) && !HasMaintenanceOnDateTime(dateTime);
    }

    public List<FieldOperatingHours> GetWeeklyOperatingHours()
    {
        return OperatingHours.Where(h => h.IsActive).OrderBy(h => h.DayOfWeek).ToList();
    }

    public List<FieldMaintenance> GetUpcomingMaintenance(int daysAhead = 30)
    {
        var cutoffDate = DateTime.Now.AddDays(daysAhead);
        return MaintenanceSchedules
            .Where(m => m.StartTime <= cutoffDate && m.Status != MaintenanceStatus.Cancelled)
            .OrderBy(m => m.StartTime)
            .ToList();
    }
}
