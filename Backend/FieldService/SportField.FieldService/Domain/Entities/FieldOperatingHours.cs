using Common.Abstractions;
using SportField.FieldService.Domain.Events;

namespace SportField.FieldService.Domain.Entities;

/// <summary>
/// Quản lý giờ mở/đóng cửa của sân theo từng ngày trong tuần
/// </summary>
public class FieldOperatingHours : BaseEntity
{
    public required Guid FieldId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly OpenTime { get; set; }
    public TimeOnly CloseTime { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsClosed { get; set; } = false; // Đóng cửa hoàn toàn trong ngày này
    public string? Notes { get; set; }

    // Navigation property
    public virtual Field Field { get; set; } = null!;

    public static FieldOperatingHours Create(Guid FieldId, DayOfWeek dayOfWeek, TimeOnly openTime, TimeOnly closeTime, string? notes = null)
    {
        var operatingHours = new FieldOperatingHours
        {
            FieldId = FieldId,
            DayOfWeek = dayOfWeek,
            OpenTime = openTime,
            CloseTime = closeTime,
            Notes = notes
        };

        operatingHours.AddDomainEvent(new FieldOperatingHoursUpdatedEvent(FieldId, dayOfWeek, openTime, closeTime, true));
        return operatingHours;
    }

    public void UpdateHours(TimeOnly openTime, TimeOnly closeTime, string? notes = null)
    {
        OpenTime = openTime;
        CloseTime = closeTime;
        Notes = notes;

        AddDomainEvent(new FieldOperatingHoursUpdatedEvent(FieldId, DayOfWeek, openTime, closeTime, IsActive));
    }

    public void CloseForDay(string? reason = null)
    {
        IsClosed = true;
        Notes = reason;

        AddDomainEvent(new FieldClosedForDayEvent(FieldId, DayOfWeek, reason));
    }

    public void OpenForDay()
    {
        IsClosed = false;

        AddDomainEvent(new FieldOpenedForDayEvent(FieldId, DayOfWeek));
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public bool IsWithinOperatingHours(TimeOnly time)
    {
        if (!IsActive || IsClosed)
            return false;

        return time >= OpenTime && time <= CloseTime;
    }

    public bool IsOperatingOnDate(DateTime date)
    {
        return IsActive && !IsClosed && date.DayOfWeek == DayOfWeek;
    }

    public TimeSpan GetOperatingDuration()
    {
        return CloseTime.ToTimeSpan() - OpenTime.ToTimeSpan();
    }
}
