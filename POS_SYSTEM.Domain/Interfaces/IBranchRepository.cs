using POS_SYSTEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Domain.Interfaces
{
    public interface IBranchRepository
    {
        Task<List<Branch>> SearchAsync(string? search);
        Task<Branch?> GetByIdAsync(int id);
        Task<Branch?> GetLastBranchWithCodePrefixAsync();
        Task AddAsync(Branch branch);
        void Delete(Branch branch);
        Task<bool> SaveChangesAsync();
    }
}
