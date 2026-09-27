using Microsoft.EntityFrameworkCore;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using POS_SYSTEM.Infrastructure.Persistence;

namespace POS_SYSTEM.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context) => _context = context;

    private IQueryable<Product> WithIncludes() =>
        _context.Products.Include(p => p.Category).Include(p => p.Brand).Include(p => p.Unit);

    public async Task<List<Product>> GetAllAsync() =>
        await WithIncludes().AsNoTracking().ToListAsync();

    public async Task<Product?> GetByIdAsync(int id) =>
        await WithIncludes().FirstOrDefaultAsync(p => p.ProductId == id);

    public async Task<Product?> GetByBarcodeAsync(string barcode) =>
        await WithIncludes().FirstOrDefaultAsync(p => p.Barcode == barcode);

    public async Task<List<Product>> GetByIdsAsync(List<int> ids) =>
        await _context.Products.Where(p => ids.Contains(p.ProductId)).ToListAsync();

    public async Task<int> GetMaxProductIdAsync() =>
        await _context.Products.AnyAsync() ? await _context.Products.MaxAsync(p => p.ProductId) : 0;

    public async Task<List<Category>> GetCategoriesAsync() =>
        await _context.Categories.AsNoTracking().ToListAsync();

    public async Task<List<Brand>> GetBrandsAsync() =>
        await _context.Brands.AsNoTracking().ToListAsync();

    public async Task<List<Unit>> GetUnitsAsync() =>
        await _context.Units.AsNoTracking().ToListAsync();

    public async Task AddAsync(Product product) => await _context.Products.AddAsync(product);
    public void Update(Product product) => _context.Products.Update(product);
    public void Delete(Product product) => _context.Products.Remove(product);
    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() >= 0;
}