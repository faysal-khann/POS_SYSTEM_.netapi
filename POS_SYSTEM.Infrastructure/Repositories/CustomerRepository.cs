using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using POS_SYSTEM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context) => _context = context;

        public async Task<List<Customer>> GetAllAsync() =>
            await _context.Customers.AsNoTracking().ToListAsync();

        public async Task<Customer?> GetByIdAsync(int id) =>
            await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);

        public async Task<int> GetMaxCustomerIdAsync() =>
            await _context.Customers.AnyAsync() ? await _context.Customers.MaxAsync(c => c.CustomerId) : 0;

        public async Task AddAsync(Customer customer) => await _context.Customers.AddAsync(customer);
        public void Update(Customer customer) => _context.Customers.Update(customer);
        public void Delete(Customer customer) => _context.Customers.Remove(customer);
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() >= 0;
    }
}
