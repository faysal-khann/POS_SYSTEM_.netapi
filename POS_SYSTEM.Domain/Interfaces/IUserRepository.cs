using POS_SYSTEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> SearchAsync(string? search, int? roleId, string? status);
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByIdWithRoleAndBranchAsync(int id);
        Task<User?> FindByUsernameOrEmailAsync(string username, string email, int? excludeUserId = null);

        Task<List<Role>> GetRolesAsync();
        Task<List<Permission>> GetActivePermissionsOrderedAsync();
        Task<List<int>> GetPermissionIdsForRoleAsync(int roleId);
        Task<List<int>> GetPermissionIdsForUserAsync(int userId);

        Task ReplaceUserPermissionsAsync(int userId, List<int> permissionIds);

        Task AddAsync(User user);
        void Delete(User user);
        Task<bool> SaveChangesAsync();
        Task<int> GetNewUserIdAfterSaveAsync(User user); // returns user.UserId after save
    }
}
