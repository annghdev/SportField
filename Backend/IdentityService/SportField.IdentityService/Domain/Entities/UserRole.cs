using Common.Abstractions;

namespace SportField.IdentityService.Domain.Entities;

public class UserRole : AuditableEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? ExpiryDate { get; set; } // For temporary role assignments
    public string? AssignedBy { get; set; } // Who assigned this role
    public string? AssignmentReason { get; set; } // Why this role was assigned
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;

    // Computed properties
    public bool IsExpired => ExpiryDate.HasValue && ExpiryDate <= DateTime.UtcNow;
    public bool IsEffective => IsActive && !IsExpired;

    // Domain methods
    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void SetExpiry(DateTime expiryDate)
    {
        ExpiryDate = expiryDate;
    }

    public void RemoveExpiry()
    {
        ExpiryDate = null;
    }

    public void SetAssignmentInfo(string assignedBy, string? reason = null)
    {
        AssignedBy = assignedBy;
        AssignmentReason = reason;
    }

    // Static factory methods
    public static UserRole Create(Guid userId, Guid roleId, string? assignedBy = null, string? reason = null)
    {
        return new UserRole
        {
            UserId = userId,
            RoleId = roleId,
            AssignedBy = assignedBy,
            AssignmentReason = reason,
            IsActive = true
        };
    }

    public static UserRole CreateTemporary(Guid userId, Guid roleId, DateTime expiryDate, string? assignedBy = null, string? reason = null)
    {
        return new UserRole
        {
            UserId = userId,
            RoleId = roleId,
            ExpiryDate = expiryDate,
            AssignedBy = assignedBy,
            AssignmentReason = reason,
            IsActive = true
        };
    }
}
