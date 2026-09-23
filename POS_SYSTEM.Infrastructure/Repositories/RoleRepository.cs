using Microsoft.EntityFrameworkCore;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using POS_SYSTEM.Infrastructure.Persistence;

namespace POS_SYSTEM.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context) => _context = context;

    public async Task<List<Role>> SearchAsync(string? search)
    {
        var query = _context.Roles.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(r => r.RoleName.Contains(search));

        return await query.OrderBy(r => r.RoleId).ToListAsync();
    }

    public async Task<Role?> GetByIdAsync(int id) =>
        await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == id);

    public async Task<Role?> GetByNameAsync(string roleName, int? excludeRoleId = null)
    {
        var query = _context.Roles.Where(r => r.RoleName == roleName);
        if (excludeRoleId.HasValue)
            query = query.Where(r => r.RoleId != excludeRoleId.Value);
        return await query.FirstOrDefaultAsync();
    }

    public async Task<int> GetUserCountForRoleAsync(int roleId) =>
        await _context.Users.CountAsync(u => u.RoleId == roleId);

    public async Task<List<int>> GetPermissionIdsForRoleAsync(int roleId) =>
        await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .Select(rp => rp.PermissionId)
            .ToListAsync();

    public async Task ReplaceRolePermissionsAsync(int roleId, List<int> permissionIds)
    {
        var existing = _context.RolePermissions.Where(rp => rp.RoleId == roleId);
        _context.RolePermissions.RemoveRange(existing);

        foreach (var pid in permissionIds)
            await _context.RolePermissions.AddAsync(new RolePermission { RoleId = roleId, PermissionId = pid });
    }

    public async Task AddAsync(Role role) => await _context.Roles.AddAsync(role);
    public void Delete(Role role) => _context.Roles.Remove(role);
    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() >= 0;
}