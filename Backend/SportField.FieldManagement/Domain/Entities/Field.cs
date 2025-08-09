namespace SportField.FieldManagement.Domain.Entities;

public class Field : AuditableEntity, IAggregateRoot
{
    public Guid FacilityId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public FieldType Type { get; set; }
    public string? ImageUrls { get; set; } // Json pattern
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Facility? Facility { get; set; }
    public virtual ICollection<FieldPricing> FieldPricings { get; set; } = [];
    public virtual ICollection<FieldMaintenance> FieldMaintenances { get; set; } = [];

    // Factory method
    public static Field Create(
        Guid facilityId,
        string name,
        string? description,
        FieldType fieldType,
        string? imageUrls,
        bool isActive)
    {
        var field = new Field
        {
            Name = name,
            FacilityId = facilityId,
            Description = description,
            Type = fieldType,
            ImageUrls = imageUrls,
            IsActive = isActive
        };
        field.AddDomainEvent(new FieldCreateEvent(field.FacilityId, field.Id, field.Name, field.Type, isActive));
        return field;
    }
    public void UpdateInformation(
        string name,
        string? description,
        string? imageUrls,
        bool isActive)
    {
        if (Name != name)
        {
            Name = name;
            AddDomainEvent(new FieldNameChangeEvent(Id, name));
        }

        Description = description;
        ImageUrls = imageUrls;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new FieldInfoChangeEvent(Id, name, description, imageUrls, isActive));
    }
    public void SetActive(bool isActive)
    {
        if (IsActive != isActive)
        {
            IsActive = isActive;
            ModifiedDate = DateTime.UtcNow;
            AddDomainEvent(new FieldActiveChangeEvent(Id, isActive));
        }
    }
    public void MarkAsDeleted()
    {
        DeletedDate = DateTime.UtcNow;
        AddDomainEvent(new FieldDeleteEvent(Id));
    }
}

//public enum FieldType
//{
//    FootballMini = 0,
//    Badminton = 1,
//    PickleBall = 2,
//    Tennis = 3,
//    Volleyball = 4,
//    Basketball = 5,
//    TableTennis = 6,
//    MultiPurpose = 7
//}