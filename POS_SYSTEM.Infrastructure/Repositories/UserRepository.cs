using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using POS_SYSTEM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) => _context = context;

        public async Task<List<User>> SearchAsync(string? search, int? roleId, string? status)
        {
            var query = _context.Users.Include(u => u.Role).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    u.FullName.Contains(search) ||
                    u.Username.Contains(search) ||
                    u.Email.Contains(search));
            }

            if (roleId.HasValue)
                query = query.Where(u => u.RoleId == roleId.Value);

            if (!string.IsNullOrWhiteSpace(status) && status != "All")
                query = query.Where(u => u.Status == status);

            return await query.OrderBy(u => u.UserId).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id) =>
            await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);

        public async Task<User?> GetByIdWithRoleAndBranchAsync(int id) =>
            await _context.Users
                .Include(u => u.Role)
                .Include(u => u.PrimaryBranch)
                .FirstOrDefaultAsync(u => u.UserId == id);

        public async Task<User?> FindByUsernameOrEmailAsync(string username, string email, int? excludeUserId = null)
        {
            var query = _context.Users.Where(u => u.Username == username || u.Email == email);
            if (excludeUserId.HasValue)
                query = query.Where(u => u.UserId != excludeUserId.Value);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Role>> GetRolesAsync() =>
            await _context.Roles.AsNoTracking().ToListAsync();

        public async Task<List<Permission>> GetActivePermissionsOrderedAsync() =>
            await _context.Permissions
                .Where(p => p.Status == "Active")
                .OrderBy(p => p.SortOrder)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<int>> GetPermissionIdsForRoleAsync(int roleId) =>
            await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.PermissionId)
                .ToListAsync();

        public async Task<List<int>> GetPermissionIdsForUserAsync(int userId) =>
            await _context.UserPermissions
                .Where(up => up.UserId == userId)
                .Select(up => up.PermissionId)
                .ToListAsync();

        public async Task ReplaceUserPermissionsAsync(int userId, List<int> permissionIds)
        {
            var existing = _context.UserPermissions.Where(up => up.UserId == userId);
            _context.UserPermissions.RemoveRange(existing);

            foreach (var pid in permissionIds)
                await _context.UserPermissions.AddAsync(new UserPermission { UserId = userId, PermissionId = pid });
        }

        public async Task AddAsync(User user) => await _context.Users.AddAsync(user);
        public void Delete(User user) => _context.Users.Remove(user);
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() >= 0;

        public Task<int> GetNewUserIdAfterSaveAsync(User user) => Task.FromResult(user.UserId);
    }
}
