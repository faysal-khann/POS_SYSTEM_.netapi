using Microsoft.EntityFrameworkCore;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using POS_SYSTEM.Infrastructure.Persistence;

namespace POS_SYSTEM.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context) => _context = context;

    public async Task<List<Category>> GetAllAsync() =>
        await _context.Categories.AsNoTracking().ToListAsync();

    public async Task<Category?> GetByIdAsync(int id) =>
        await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);

    public async Task AddAsync(Category category) => await _context.Categories.AddAsync(category);
    public void Delete(Category category) => _context.Categories.Remove(category);
    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() >= 0;
}