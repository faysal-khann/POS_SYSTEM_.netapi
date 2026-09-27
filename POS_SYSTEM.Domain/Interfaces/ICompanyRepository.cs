using POS_SYSTEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Domain.Interfaces
{
    public interface ICompanyRepository
    {
        Task<List<Company>> SearchAsync(string? search);
        Task<Company?> GetByIdAsync(int id);
        Task AddAsync(Company company);
        void Delete(Company company);
        Task<bool> SaveChangesAsync();
    }
}
