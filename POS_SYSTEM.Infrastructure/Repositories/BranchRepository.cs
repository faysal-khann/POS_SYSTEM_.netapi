using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using POS_SYSTEM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Infrastructure.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly AppDbContext _context;

        public BranchRepository(AppDbContext context) => _context = context;

        public async Task<List<Branch>> SearchAsync(string? search)
        {
            var query = _context.Branches.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(b => b.BranchName.Contains(search));

            return await query.OrderBy(b => b.BranchId).ToListAsync();
        }

        public async Task<Branch?> GetByIdAsync(int id) =>
            await _context.Branches.FirstOrDefaultAsync(b => b.BranchId == id);

        public async Task<Branch?> GetLastBranchWithCodePrefixAsync() =>
            await _context.Branches
                .Where(b => b.BranchCode != null && b.BranchCode.StartsWith("BR-"))
                .OrderByDescending(b => b.BranchId)
                .FirstOrDefaultAsync();

        public async Task AddAsync(Branch branch) => await _context.Branches.AddAsync(branch);
        public void Delete(Branch branch) => _context.Branches.Remove(branch);
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() >= 0;
    }
}
