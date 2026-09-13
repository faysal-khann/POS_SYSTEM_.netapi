using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using POS_SYSTEM.Infrastructure.Persistence;

namespace POS_SYSTEM.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context) => _context = context;

        public async Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail) =>
            await _context.Users
                .Include(u => u.Role)
                .Include(u => u.PrimaryBranch)
                .FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);

        public async Task<Branch?> GetBranchByIdAsync(int branchId) =>
            await _context.Branches.FirstOrDefaultAsync(b => b.BranchId == branchId);

        public async Task<Company?> GetCompanyByIdAsync(int companyId) =>
            await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == companyId);

        public async Task<List<string>> GetPermissionKeysForUserAsync(int userId) =>
            await _context.UserPermissions
                .Where(up => up.UserId == userId)
                .Join(_context.Permissions, up => up.PermissionId, p => p.PermissionId, (up, p) => p.PermissionKey)
                .ToListAsync();

        public async Task UpdateLastLoginAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
