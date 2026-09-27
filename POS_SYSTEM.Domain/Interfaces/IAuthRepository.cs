using System;
using System.Collections.Generic;
using System.Text;
using POS_SYSTEM.Domain.Entities;
namespace POS_SYSTEM.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail);
        Task<Branch?> GetBranchByIdAsync(int branchId);
        Task<Company?> GetCompanyByIdAsync(int companyId);
        Task<List<string>> GetPermissionKeysForUserAsync(int userId);
        Task UpdateLastLoginAsync(User user);
    }
}
