using Common.Abstractions;
using Common.Enums;
using SportField.IdentityService.Domain.Entities;

namespace SportField.IdentityService.Domain.Repositories;

public interface IPermissionReadRepository : IReadRepository<Permission, Guid>
{
    Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Permission?> GetByPermissionTypeAsync(PermissionType permissionType, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetByModuleAsync(string module, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetByResourceAsync(string resource, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetSystemPermissionsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetActivePermissionsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default);
}
