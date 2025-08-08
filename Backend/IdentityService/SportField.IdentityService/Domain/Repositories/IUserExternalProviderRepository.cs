using Common.Abstractions;
using SportField.IdentityService.Domain.Entities;

namespace SportField.IdentityService.Domain.Repositories;

public interface IUserExternalProviderRepository
{
    Task<UserExternalProvider?> GetAsync(Guid userId, Guid providerId, CancellationToken cancellationToken = default);
    Task<UserExternalProvider?> GetByExternalUserIdAsync(Guid providerId, string externalUserId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserExternalProvider>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserExternalProvider>> GetByProviderIdAsync(Guid providerId, CancellationToken cancellationToken = default);
    Task<UserExternalProvider?> GetByExternalEmailAsync(Guid providerId, string externalEmail, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserExternalProvider>> GetExpiredTokensAsync(CancellationToken cancellationToken = default);
    Task<UserExternalProvider> CreateAsync(UserExternalProvider userProvider, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserExternalProvider userProvider, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid userId, Guid providerId, CancellationToken cancellationToken = default);
    Task DeleteByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task DeleteByProviderIdAsync(Guid providerId, CancellationToken cancellationToken = default);
}
