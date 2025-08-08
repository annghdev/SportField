using Common.Abstractions;

namespace SportField.IdentityService.Domain.Entities;

public class UserExternalProvider : AuditableEntity
{
    public Guid UserId { get; set; }
    public Guid ExternalProviderId { get; set; }
    public string ExternalUserId { get; set; } = string.Empty; // ID from external provider
    public string? ExternalUserName { get; set; } // Username from external provider
    public string? ExternalEmail { get; set; } // Email from external provider
    public string? AccessToken { get; set; } // Should be encrypted
    public string? RefreshToken { get; set; } // Should be encrypted
    public DateTime? TokenExpiryDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime LastLoginDate { get; set; }
    public string? AdditionalData { get; set; } // JSON for additional provider-specific data

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ExternalProvider ExternalProvider { get; set; } = null!;

    // Computed properties
    public bool IsTokenExpired => TokenExpiryDate.HasValue && TokenExpiryDate <= DateTime.UtcNow;

    // Domain methods
    public void UpdateTokens(string? accessToken, string? refreshToken, DateTime? expiryDate)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        TokenExpiryDate = expiryDate;
        LastLoginDate = DateTime.UtcNow;
    }

    public void UpdateExternalUserInfo(string externalUserId, string? externalUserName, string? externalEmail)
    {
        ExternalUserId = externalUserId;
        ExternalUserName = externalUserName;
        ExternalEmail = externalEmail;
    }

    public void UpdateLastLogin()
    {
        LastLoginDate = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void ClearTokens()
    {
        AccessToken = null;
        RefreshToken = null;
        TokenExpiryDate = null;
    }

    public void SetAdditionalData(string? additionalData)
    {
        AdditionalData = additionalData;
    }

    // Static factory methods
    public static UserExternalProvider Create(
        Guid userId, 
        Guid externalProviderId, 
        string externalUserId, 
        string? externalUserName = null, 
        string? externalEmail = null)
    {
        return new UserExternalProvider
        {
            UserId = userId,
            ExternalProviderId = externalProviderId,
            ExternalUserId = externalUserId,
            ExternalUserName = externalUserName,
            ExternalEmail = externalEmail,
            IsActive = true,
            LastLoginDate = DateTime.UtcNow
        };
    }

    public static UserExternalProvider CreateWithTokens(
        Guid userId,
        Guid externalProviderId,
        string externalUserId,
        string? accessToken,
        string? refreshToken,
        DateTime? tokenExpiryDate,
        string? externalUserName = null,
        string? externalEmail = null)
    {
        return new UserExternalProvider
        {
            UserId = userId,
            ExternalProviderId = externalProviderId,
            ExternalUserId = externalUserId,
            ExternalUserName = externalUserName,
            ExternalEmail = externalEmail,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            TokenExpiryDate = tokenExpiryDate,
            IsActive = true,
            LastLoginDate = DateTime.UtcNow
        };
    }
}
