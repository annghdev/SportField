namespace SportField.FieldManagement.Domain.Entities;

public class Facility : AuditableEntity, IAggregateRoot
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? Location { get; set; } // Tọa độ
    public TimeOnly OpenTime { get; set; } // Giờ mở cửa
    public TimeOnly CloseTime { get; set; } // Giờ đóng cửa
    public string? ImageUrls { get; set; } // Json pattern
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Field> Fields { get; set; } = [];

    // Factory method
    public static Facility Create(
        string name,
        string? description,
        string? address,
        string? location,
        TimeOnly openTime,
        TimeOnly closeTime,
        string? imageUrls,
        bool isActive)
    {
        var facility = new Facility
        {
            Name = name,
            Description = description,
            Address = address,
            Location = location,
            OpenTime = openTime,
            CloseTime = closeTime,
            ImageUrls = imageUrls,
            IsActive = isActive
        };
        return facility;
    }
    public void UpdateInformation(
        string name,
        string? description,
        string? address,
        string? location,
        TimeOnly openTime,
        TimeOnly closeTime,
        string? imageUrls)
    {
        if (OpenTime != openTime || CloseTime != closeTime)
        {
            AddDomainEvent(new FacilityOperatingHoursChangeEvent(Id, openTime, closeTime));
        }

        Name = name;
        Description = description;
        Address = address;
        Location = location;
        OpenTime = openTime;
        CloseTime = closeTime;
        ImageUrls = imageUrls;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new FacilityInfoChangeEvent(Id, Name, Description, Address, Location, ImageUrls));
    }
    public void SetActive(bool isActive)
    {
        if (IsActive != isActive)
        {
            IsActive = isActive;
            ModifiedDate = DateTime.UtcNow;
            AddDomainEvent(new FacilityActiveChangeEvent(Id, isActive));
        }
    }
    public void MarkAsDeleted()
    {
        DeletedDate = DateTime.UtcNow;
        foreach (var field in Fields)
        {
            field.MarkAsDeleted();
        }
    }
}
