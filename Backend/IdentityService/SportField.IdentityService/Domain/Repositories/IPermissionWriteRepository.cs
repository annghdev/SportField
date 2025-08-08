using Common.Abstractions;
using SportField.IdentityService.Domain.Entities;

namespace SportField.IdentityService.Domain.Repositories;

public interface IPermissionWriteRepository : IWriteRepository<Permission, Guid>
{
    Task<Permission> CreateAsync(Permission permission, CancellationToken cancellationToken = default);
    Task UpdateAsync(Permission permission, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid permissionId, CancellationToken cancellationToken = default);
}
