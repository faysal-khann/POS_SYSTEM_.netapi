using POS_SYSTEM.Domain.Entities;

namespace POS_SYSTEM.Domain.Interfaces;

public interface IPermissionRepository
{
    Task<List<Permission>> GetModulesAsync();
    Task<List<Permission>> SearchChildPermissionsAsync(string? search, string? module);
    Task<Permission?> GetByIdAsync(int id);
    Task<Permission?> GetModuleByNameAsync(string moduleName);

    Task AddAsync(Permission permission);
    void Delete(Permission permission);
    Task<bool> SaveChangesAsync();
}