using POS_SYSTEM.Domain.Entities;

namespace POS_SYSTEM.Domain.Interfaces;

public interface IRoleRepository
{
    Task<List<Role>> SearchAsync(string? search);
    Task<Role?> GetByIdAsync(int id);
    Task<Role?> GetByNameAsync(string roleName, int? excludeRoleId = null);

    Task<int> GetUserCountForRoleAsync(int roleId);
    Task<List<int>> GetPermissionIdsForRoleAsync(int roleId);
    Task ReplaceRolePermissionsAsync(int roleId, List<int> permissionIds);

    Task AddAsync(Role role);
    void Delete(Role role);
    Task<bool> SaveChangesAsync();
}