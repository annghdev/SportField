using Common.Abstractions;

namespace SportField.IdentityService.Domain.Entities;

public class RolePermission : AuditableEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    public bool IsGranted { get; set; } = true; // Can be used to explicitly deny permissions
    public bool IsActive { get; set; } = true;
    public string? GrantedBy { get; set; } // Who granted this permission
    public string? GrantReason { get; set; } // Why this permission was granted

    // Navigation properties
    public virtual Role Role { get; set; } = null!;
    public virtual Permission Permission { get; set; } = null!;

    // Computed properties
    public bool IsEffective => IsActive && IsGranted;

    // Domain methods
    public void Grant()
    {
        IsGranted = true;
        IsActive = true;
    }

    public void Deny()
    {
        IsGranted = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void SetGrantInfo(string grantedBy, string? reason = null)
    {
        GrantedBy = grantedBy;
        GrantReason = reason;
    }

    // Static factory methods
    public static RolePermission Create(Guid roleId, Guid permissionId, string? grantedBy = null, string? reason = null)
    {
        return new RolePermission
        {
            RoleId = roleId,
            PermissionId = permissionId,
            IsGranted = true,
            IsActive = true,
            GrantedBy = grantedBy,
            GrantReason = reason
        };
    }

    public static RolePermission CreateDenied(Guid roleId, Guid permissionId, string? grantedBy = null, string? reason = null)
    {
        return new RolePermission
        {
            RoleId = roleId,
            PermissionId = permissionId,
            IsGranted = false,
            IsActive = true,
            GrantedBy = grantedBy,
            GrantReason = reason
        };
    }
}
