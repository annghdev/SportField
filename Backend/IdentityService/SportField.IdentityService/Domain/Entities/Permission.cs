using Common.Abstractions;

namespace SportField.IdentityService.Domain.Entities;

public class Permission : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Module { get; set; } // = PermissionModule.FieldService;
    public string? Resource { get; set; } // = PermissionResource.Fields; 
    public string? Action { get; set; } // = PermissionAction.Create;
    public bool IsActive { get; set; } = true;
    public bool IsSystemPermission { get; set; } = false;

    // Navigation properties
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new HashSet<RolePermission>();

    // Domain methods
    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        if (IsSystemPermission)
        {
            throw new InvalidOperationException("Cannot deactivate system permissions");
        }
        IsActive = false;
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
    }
}
public static class PermissionAction
{
    public const string Create = "Create";
    public const string Update = "Update";
    public const string Delete = "Delete";
    public const string ReadOwn = "ReadOwn";
    public const string ReadAll = "ReadAll";
    public const string Upload = "Upload";
    public const string Download = "Download";
}
public static class PermissionModule
{
    public const string FieldService = "FieldService";
    public const string BookingService = "FieldService";
    public const string FileService = "FileService";
    public const string NotificationService = "NotificationService";
    public const string PaymentService = "PaymentService";
    public const string IdentityService = "IdentityService";

    // Future modules
    public const string DiscountService = "DiscountService";
    public const string TrainingService = "TrainingService";

    public const string InvoiceService = "InvoiceService";
    public const string POSService = "POSService";
    public const string ECommerceService = "ECommerceService";
    public const string PurchaseService = "PurchaseService";
    public const string AccountantService = "AccountantService";
    public const string CommunityService = "CommunityService";
    public const string ContentService = "ContentService";
}
public static class PermissionResource
{
    public const string Fields = "Fields";
    public const string Facilities = "Facilities";
    public const string TimeSlots = "TimeSlots";
    public const string Users = "Users";
    public const string Roles = "Roles";
    public const string Bookings = "Bookings";
    public const string Calendar = "Calendar";
    public const string Files = "Files";
    public const string NotificationTemplates = "NotificationTemplates";
    public const string PaymentGateways = "Payments";

}