using Microsoft.EntityFrameworkCore;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using POS_SYSTEM.Infrastructure.Persistence;

namespace POS_SYSTEM.Infrastructure.Repositories;

public class StockRepository : IStockRepository
{
    private readonly AppDbContext _context;

    public StockRepository(AppDbContext context) => _context = context;

    private IQueryable<ProductStock> WithIncludes() =>
        _context.ProductStocks
            .Include(s => s.Product).ThenInclude(p => p!.Category)
            .Include(s => s.Product).ThenInclude(p => p!.Brand)
            .Include(s => s.Product).ThenInclude(p => p!.Unit)
            .Include(s => s.Branch);

    public async Task<List<ProductStock>> SearchAsync(string? search, int? categoryId, int? brandId, int? branchId)
    {
        var query = WithIncludes();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(s =>
                s.Product!.ProductName.Contains(search) ||
                s.Product.ProductCode.Contains(search));
        }

        if (categoryId.HasValue) query = query.Where(s => s.Product!.CategoryId == categoryId.Value);
        if (brandId.HasValue) query = query.Where(s => s.Product!.BrandId == brandId.Value);
        if (branchId.HasValue) query = query.Where(s => s.BranchId == branchId.Value);

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<ProductStock?> GetByIdWithDetailsAsync(int stockId) =>
        await WithIncludes().FirstOrDefaultAsync(s => s.ProductStockId == stockId);

    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() >= 0;
}