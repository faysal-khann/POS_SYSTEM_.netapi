using Microsoft.EntityFrameworkCore;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using POS_SYSTEM.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Infrastructure.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly AppDbContext _context;

        public SupplierRepository(AppDbContext context) => _context = context;

        public async Task<List<Supplier>> GetAllAsync() =>
            await _context.Suppliers.AsNoTracking().ToListAsync();

        public async Task<Supplier?> GetByIdAsync(int id) =>
            await _context.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == id);

        public async Task<int> GetMaxSupplierIdAsync() =>
            await _context.Suppliers.AnyAsync() ? await _context.Suppliers.MaxAsync(s => s.SupplierId) : 0;

        public async Task AddAsync(Supplier supplier) => await _context.Suppliers.AddAsync(supplier);
        public void Update(Supplier supplier) => _context.Suppliers.Update(supplier);
        public void Delete(Supplier supplier) => _context.Suppliers.Remove(supplier);
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() >= 0;
    }
}
