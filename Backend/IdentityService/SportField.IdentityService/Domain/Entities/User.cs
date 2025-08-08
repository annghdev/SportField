using Common.Abstractions;

namespace SportField.IdentityService.Domain.Entities;

public class User : AuditableEntity, IAggregateRoot
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
    public bool IsEmailConfirmed { get; set; } = false;
    public bool IsPhoneConfirmed { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public DateTime? LastLoginDate { get; set; }
    public int FailedLoginAttempts { get; set; } = 0;
    public DateTime? LockoutEndDate { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public string? EmailConfirmationToken { get; set; }
    public string? PhoneConfirmationToken { get; set; }
    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpiry { get; set; }

    // Navigation properties
    public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    public virtual ICollection<UserExternalProvider> ExternalProviders { get; set; } = new HashSet<UserExternalProvider>();

    // Computed properties
    public string FullName => $"{FirstName} {LastName}".Trim();
    public bool IsLockedOut => LockoutEndDate.HasValue && LockoutEndDate > DateTime.UtcNow;

    // Domain methods
    public void UpdateLoginInfo(DateTime loginDate)
    {
        LastLoginDate = loginDate;
        FailedLoginAttempts = 0;
    }

    public void RecordFailedLogin()
    {
        FailedLoginAttempts++;
        
        // Lock account after 5 failed attempts for 30 minutes
        if (FailedLoginAttempts >= 5)
        {
            LockoutEndDate = DateTime.UtcNow.AddMinutes(30);
        }
    }

    public void UnlockAccount()
    {
        FailedLoginAttempts = 0;
        LockoutEndDate = null;
    }

    public void ConfirmEmail()
    {
        IsEmailConfirmed = true;
        EmailConfirmationToken = null;
    }

    public void ConfirmPhone()
    {
        IsPhoneConfirmed = true;
        PhoneConfirmationToken = null;
    }

    public void SetPasswordResetToken(string token, TimeSpan expiry)
    {
        PasswordResetToken = token;
        PasswordResetTokenExpiry = DateTime.UtcNow.Add(expiry);
    }

    public void ClearPasswordResetToken()
    {
        PasswordResetToken = null;
        PasswordResetTokenExpiry = null;
    }

    public void SetRefreshToken(string token, DateTime expiry)
    {
        RefreshToken = token;
        RefreshTokenExpiryTime = expiry;
    }

    public void ClearRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiryTime = null;
    }

    public void Deactivate()
    {
        IsActive = false;
        ClearRefreshToken();
    }

    public void Activate()
    {
        IsActive = true;
        UnlockAccount();
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        IsActive = false;
        DeletedDate = DateTime.UtcNow;
        ClearRefreshToken();
    }
}
