using Common.Abstractions;
using SportField.IdentityService.Domain.Entities;

namespace SportField.IdentityService.Domain.Repositories;

public interface IExternalProviderRepository : IRepository<ExternalProvider, Guid>
{
    Task<ExternalProvider?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExternalProvider>> GetActiveProvidersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ExternalProvider>> GetSystemProvidersAsync(CancellationToken cancellationToken = default);
    Task<ExternalProvider> CreateAsync(ExternalProvider provider, CancellationToken cancellationToken = default);
    Task UpdateAsync(ExternalProvider provider, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid providerId, CancellationToken cancellationToken = default);
}
