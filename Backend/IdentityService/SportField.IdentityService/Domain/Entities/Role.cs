using Common.Abstractions;

namespace SportField.IdentityService.Domain.Entities;

public class Role : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsSystemRole { get; set; } = false; // Roles created by system vs custom roles
    public bool IsActive { get; set; } = true;
    public int Priority { get; set; } = 0; // Higher priority roles have more precedence
    
    // Navigation properties
    public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new HashSet<RolePermission>();

    // Constants for system roles
    public static class SystemRoles
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string Receptionist = "Receptionist";
        public const string Cashier = "Cashier";
        public const string Accountant = "Accountant";
        public const string Customer = "Customer";
        public const string Guest = "Guest";
        public const string Organization = "Organization";
        public const string TeamLead = "TeamLead";
        public const string TeamMember = "TeamMember";
        public const string Coach = "Coach";
        public const string Student = "Student";
    }

    public static class RolePriorities
    {
        public const int Admin = 1000;
        public const int Manager = 900;
        public const int Receptionist = 800;
        public const int Cashier = 800;
        public const int Accountant = 800;
        public const int Organization = 600;
        public const int TeamLead = 500;
        public const int Coach = 400;
        public const int Customer = 300;
        public const int TeamMember = 200;
        public const int Student = 200;
        public const int Guest = 100;
    }

    // Domain methods
    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        if (IsSystemRole)
        {
            throw new InvalidOperationException("Cannot deactivate system roles");
        }
        IsActive = false;
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
    }

    public void UpdatePriority(int priority)
    {
        Priority = priority;
    }

    // Helper methods to create system roles
    public static Role CreateAdminRole()
    {
        return new Role
        {
            Name = SystemRoles.Admin,
            Description = "System administrator with full access",
            IsSystemRole = true,
            Priority = RolePriorities.Admin
        };
    }

    public static Role CreateManagerRole()
    {
        return new Role
        {
            Name = SystemRoles.Manager,
            Description = "Manager with administrative privileges",
            IsSystemRole = true,
            Priority = RolePriorities.Manager
        };
    }

    public static Role CreateReceptionistRole()
    {
        return new Role
        {
            Name = SystemRoles.Receptionist,
            Description = "Receptionist handling customer service",
            IsSystemRole = true,
            Priority = RolePriorities.Receptionist
        };
    }

    public static Role CreateCashierRole()
    {
        return new Role
        {
            Name = SystemRoles.Cashier,
            Description = "Cashier handling payments and transactions",
            IsSystemRole = true,
            Priority = RolePriorities.Cashier
        };
    }

    public static Role CreateAccountantRole()
    {
        return new Role
        {
            Name = SystemRoles.Accountant,
            Description = "Accountant managing financial records",
            IsSystemRole = true,
            Priority = RolePriorities.Accountant
        };
    }

    public static Role CreateCustomerRole()
    {
        return new Role
        {
            Name = SystemRoles.Customer,
            Description = "Regular customer",
            IsSystemRole = true,
            Priority = RolePriorities.Customer
        };
    }

    public static Role CreateGuestRole()
    {
        return new Role
        {
            Name = SystemRoles.Guest,
            Description = "Guest user with limited access",
            IsSystemRole = true,
            Priority = RolePriorities.Guest
        };
    }

    public static Role CreateOrganizationRole()
    {
        return new Role
        {
            Name = SystemRoles.Organization,
            Description = "Organization with multiple members",
            IsSystemRole = true,
            Priority = RolePriorities.Organization
        };
    }

    public static Role CreateTeamLeadRole()
    {
        return new Role
        {
            Name = SystemRoles.TeamLead,
            Description = "Team lead managing team bookings",
            IsSystemRole = true,
            Priority = RolePriorities.TeamLead
        };
    }

    public static Role CreateTeamMemberRole()
    {
        return new Role
        {
            Name = SystemRoles.TeamMember,
            Description = "Team member participating in team bookings",
            IsSystemRole = true,
            Priority = RolePriorities.TeamMember
        };
    }

    public static Role CreateCoachRole()
    {
        return new Role
        {
            Name = SystemRoles.Coach,
            Description = "Coach managing training sessions",
            IsSystemRole = true,
            Priority = RolePriorities.Coach
        };
    }

    public static Role CreateStudentRole()
    {
        return new Role
        {
            Name = SystemRoles.Student,
            Description = "Student attending training sessions",
            IsSystemRole = true,
            Priority = RolePriorities.Student
        };
    }
}
