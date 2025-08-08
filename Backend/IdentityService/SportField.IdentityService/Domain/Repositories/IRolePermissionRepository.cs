using SportField.IdentityService.Domain.Entities;

namespace SportField.IdentityService.Domain.Repositories;

public interface IRolePermissionRepository
{
    Task<RolePermission?> GetAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<RolePermission>> GetByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<RolePermission>> GetByPermissionIdAsync(Guid permissionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<RolePermission>> GetActiveByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default);
    Task<RolePermission> CreateAsync(RolePermission rolePermission, CancellationToken cancellationToken = default);
    Task UpdateAsync(RolePermission rolePermission, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default);
    Task DeleteByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default);
    Task DeleteByPermissionIdAsync(Guid permissionId, CancellationToken cancellationToken = default);
}
