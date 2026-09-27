using Microsoft.EntityFrameworkCore;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using POS_SYSTEM.Infrastructure.Persistence;

namespace POS_SYSTEM.Infrastructure.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly AppDbContext _context;

    public BrandRepository(AppDbContext context) => _context = context;

    public async Task<List<Brand>> GetAllAsync() =>
        await _context.Brands.AsNoTracking().ToListAsync();

    public async Task<Brand?> GetByIdAsync(int id) =>
        await _context.Brands.FirstOrDefaultAsync(b => b.BrandId == id);

    public async Task AddAsync(Brand brand) => await _context.Brands.AddAsync(brand);
    public void Delete(Brand brand) => _context.Brands.Remove(brand);
    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() >= 0;
}