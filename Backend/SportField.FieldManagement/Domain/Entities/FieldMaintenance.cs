namespace SportField.FieldManagement.Domain.Entities;

public class FieldMaintenance : AuditableEntity
{
    public Guid FieldId { get; set; }
    public required string TimeFrameId { get; set; }
    public MaintenanceType Type { get; set; } = MaintenanceType.Cleaning;
    public DateTime Date { get; set; }
    public DateTime? RecurringTo { get; set; }
    public RecurringType RecurringType { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Field? Field { get; set; }
    public TimeFrame? TimeFrame { get; set; }


    // Factory method
    public static FieldMaintenance Create(
        Guid fieldId,
        string timeFrameId,
        DateTime date,
        DateTime? recurringTo,
        RecurringType recurringType)
    {
        var newMaitenance = new FieldMaintenance
        {
            FieldId = fieldId,
            TimeFrameId = timeFrameId,
            Date = date,
            RecurringTo = recurringTo,
            RecurringType = recurringType
        }; ;
        newMaitenance.AddDomainEvent(new FieldMaintenanceCreateEvent(fieldId, newMaitenance.Id, timeFrameId, date, recurringTo, recurringType));
        return newMaitenance;
    }
    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        ModifiedDate = DateTime.UtcNow;
        if (Field != null)
            Field.ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new FieldMaintenanceActiveChangeEvent(FieldId, Id, IsActive));
    }
    public void MarkAsDeleted()
    {
        DeletedDate = DateTime.UtcNow;
        IsActive = false;
        if (Field != null)
            Field.ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new FieldMaintenanceDeleteEvent(FieldId, Id));
    }
}

//public enum MaintenanceType
//{
//    Cleaning = 0, // Dọn dẹp
//    Repair = 1, // Sửa chữa
//    Upgrade = 2, // Nâng cấp
//    Emergency = 3 // Khẩn cấp
//}

//public enum RecurringType
//{
//    None,
//    Daily,
//    Weekly,
//    Monthly,
//    Yearly
//}