using Microsoft.EntityFrameworkCore;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using POS_SYSTEM.Infrastructure.Persistence;

namespace POS_SYSTEM.Infrastructure.Repositories;

public class PermissionRepository : IPermissionRepository
{
    private readonly AppDbContext _context;

    public PermissionRepository(AppDbContext context) => _context = context;

    public async Task<List<Permission>> GetModulesAsync() =>
        await _context.Permissions
            .Where(p => p.ParentPermissionId == null)
            .OrderBy(p => p.SortOrder)
            .AsNoTracking()
            .ToListAsync();

    public async Task<List<Permission>> SearchChildPermissionsAsync(string? search, string? module)
    {
        var query = _context.Permissions.Where(p => p.ParentPermissionId != null);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.PermissionName.Contains(search));

        if (!string.IsNullOrWhiteSpace(module) && module != "All")
            query = query.Where(p => p.Module == module);

        return await query.OrderBy(p => p.PermissionId).ToListAsync();
    }

    public async Task<Permission?> GetByIdAsync(int id) =>
        await _context.Permissions.FirstOrDefaultAsync(p => p.PermissionId == id);

    public async Task<Permission?> GetModuleByNameAsync(string moduleName) =>
        await _context.Permissions.FirstOrDefaultAsync(p =>
            p.ParentPermissionId == null && p.PermissionName == moduleName);

    public async Task AddAsync(Permission permission) => await _context.Permissions.AddAsync(permission);
    public void Delete(Permission permission) => _context.Permissions.Remove(permission);
    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() >= 0;
}