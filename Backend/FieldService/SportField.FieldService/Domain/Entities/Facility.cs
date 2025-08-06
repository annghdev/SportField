using Common.Abstractions;
using SportField.FieldService.Domain.Events;

namespace SportField.FieldService.Domain.Entities;

/// <summary>
/// Đại diện cho một cơ sở thể thao (trung tâm, sân bóng, câu lạc bộ) chứa nhiều sân
/// </summary>
public class Facility : AggregateRoot
{
    public required string Name { get; set; }
    public required string Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Description { get; set; }
    public TimeOnly OpenTime { get; set; } = new(6, 0);    // 6:00 AM
    public TimeOnly CloseTime { get; set; } = new(22, 0);  // 10:00 PM
    public bool IsActive { get; set; } = true;
    public string? ManagerId { get; set; }
    public virtual ICollection<Field> Fields { get; set; } = [];

    public static Facility Create(string name, string address, string? phoneNumber = null, string? email = null,
        string? description = null, TimeOnly? openTime = null, TimeOnly? closeTime = null, string? managerId = null)
    {
        var facility = new Facility
        {
            Name = name,
            Address = address,
            PhoneNumber = phoneNumber,
            Email = email,
            Description = description,
            OpenTime = openTime ?? new TimeOnly(6, 0),
            CloseTime = closeTime ?? new TimeOnly(22, 0),
            ManagerId = managerId
        };

        facility.AddDomainEvent(new FacilityCreatedEvent(facility.Id, facility.Name, facility.Address));
        return facility;
    }

    public void UpdateInfo(string name, string address, string? phoneNumber, string? email, string? description)
    {
        Name = name;
        Address = address;
        PhoneNumber = phoneNumber;
        Email = email;
        Description = description;
        ModifiedDate = DateTime.UtcNow;
    }

    public void SetOperatingHours(TimeOnly openTime, TimeOnly closeTime)
    {
        OpenTime = openTime;
        CloseTime = closeTime;
        ModifiedDate = DateTime.UtcNow;
    }

    public void SetManager(string managerId, string modifiedBy)
    {
        ManagerId = managerId;
        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
    }

    // Hủy kích hoạt
    public void Deactivate(string deactivatedBy)
    {
        IsActive = false;
        ModifiedBy = deactivatedBy;
        ModifiedDate = DateTime.UtcNow;
    }
}
