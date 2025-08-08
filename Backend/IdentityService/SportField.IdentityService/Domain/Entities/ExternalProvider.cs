using Common.Abstractions;

namespace SportField.IdentityService.Domain.Entities;

public class ExternalProvider : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = string.Empty; // e.g., "Google", "Facebook"
    public string DisplayName { get; set; } = string.Empty; // e.g., "Google", "Facebook"
    public string? Description { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public string? ClientSecret { get; set; } // Should be encrypted
    public string AuthorizeUrl { get; set; } = string.Empty;
    public string TokenUrl { get; set; } = string.Empty;
    public string? UserInfoUrl { get; set; }
    public string? Scope { get; set; } // OAuth scopes
    public string? RedirectUri { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsSystemProvider { get; set; } = false;
    public string? LogoUrl { get; set; }
    public int SortOrder { get; set; } = 0;

    // Additional configuration as JSON
    public string? AdditionalConfig { get; set; }

    // Navigation properties
    public virtual ICollection<UserExternalProvider> UserProviders { get; set; } = new HashSet<UserExternalProvider>();

    // Constants for system providers
    public static class SystemProviders
    {
        public const string Google = "Google";
        public const string Facebook = "Facebook";
        public const string Microsoft = "Microsoft";
        public const string Apple = "Apple";
    }

    // Domain methods
    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void UpdateConfiguration(string clientId, string? clientSecret, string? scope, string? redirectUri)
    {
        ClientId = clientId;
        if (!string.IsNullOrEmpty(clientSecret))
            ClientSecret = clientSecret;
        Scope = scope;
        RedirectUri = redirectUri;
    }

    public void UpdateUrls(string authorizeUrl, string tokenUrl, string? userInfoUrl)
    {
        AuthorizeUrl = authorizeUrl;
        TokenUrl = tokenUrl;
        UserInfoUrl = userInfoUrl;
    }

    public void SetSortOrder(int order)
    {
        SortOrder = order;
    }

    public void SetLogo(string? logoUrl)
    {
        LogoUrl = logoUrl;
    }

    // Helper methods to create system providers
    public static ExternalProvider CreateGoogleProvider(string clientId, string? clientSecret, string? redirectUri)
    {
        return new ExternalProvider
        {
            Name = SystemProviders.Google,
            DisplayName = "Google",
            Description = "Sign in with Google",
            ClientId = clientId,
            ClientSecret = clientSecret,
            AuthorizeUrl = "https://accounts.google.com/o/oauth2/v2/auth",
            TokenUrl = "https://oauth2.googleapis.com/token",
            UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo",
            Scope = "openid profile email",
            RedirectUri = redirectUri,
            IsSystemProvider = true,
            LogoUrl = "/images/providers/google-logo.png",
            SortOrder = 1
        };
    }

    public static ExternalProvider CreateFacebookProvider(string clientId, string? clientSecret, string? redirectUri)
    {
        return new ExternalProvider
        {
            Name = SystemProviders.Facebook,
            DisplayName = "Facebook",
            Description = "Sign in with Facebook",
            ClientId = clientId,
            ClientSecret = clientSecret,
            AuthorizeUrl = "https://www.facebook.com/v18.0/dialog/oauth",
            TokenUrl = "https://graph.facebook.com/v18.0/oauth/access_token",
            UserInfoUrl = "https://graph.facebook.com/me",
            Scope = "email public_profile",
            RedirectUri = redirectUri,
            IsSystemProvider = true,
            LogoUrl = "/images/providers/facebook-logo.png",
            SortOrder = 2
        };
    }

    public static ExternalProvider CreateMicrosoftProvider(string clientId, string? clientSecret, string? redirectUri)
    {
        return new ExternalProvider
        {
            Name = SystemProviders.Microsoft,
            DisplayName = "Microsoft",
            Description = "Sign in with Microsoft",
            ClientId = clientId,
            ClientSecret = clientSecret,
            AuthorizeUrl = "https://login.microsoftonline.com/common/oauth2/v2.0/authorize",
            TokenUrl = "https://login.microsoftonline.com/common/oauth2/v2.0/token",
            UserInfoUrl = "https://graph.microsoft.com/v1.0/me",
            Scope = "openid profile email",
            RedirectUri = redirectUri,
            IsSystemProvider = true,
            LogoUrl = "/images/providers/microsoft-logo.png",
            SortOrder = 3
        };
    }
}
