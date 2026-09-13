using Microsoft.EntityFrameworkCore;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using POS_SYSTEM.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _context;

        public CompanyRepository(AppDbContext context) => _context = context;

        public async Task<List<Company>> SearchAsync(string? search)
        {
            var query = _context.Companies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(c => c.CompanyName.Contains(search));

            return await query.OrderBy(c => c.CompanyId).ToListAsync();
        }

        public async Task<Company?> GetByIdAsync(int id) =>
            await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == id);

        public async Task AddAsync(Company company) => await _context.Companies.AddAsync(company);
        public void Delete(Company company) => _context.Companies.Remove(company);
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() >= 0;
    }
}
