using Microsoft.EntityFrameworkCore;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using POS_SYSTEM.Infrastructure.Persistence;

namespace POS_SYSTEM.Infrastructure.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly AppDbContext _context;

    public SaleRepository(AppDbContext context) => _context = context;

    public async Task<int> CountTodayInvoicesAsync(DateOnly today)
    {
        var start = today.ToDateTime(TimeOnly.MinValue);
        var end = start.AddDays(1);

        return await _context.Sales.CountAsync(s =>
            s.SaleDate >= start && s.SaleDate < end && s.InvoiceNo.StartsWith("INV-"));
    }

    public async Task<string?> GetLastDraftNoAsync(string prefix) =>
        await _context.Sales
            .Where(s => s.InvoiceNo.StartsWith(prefix))
            .OrderByDescending(s => s.InvoiceNo)
            .Select(s => s.InvoiceNo)
            .FirstOrDefaultAsync();

    public async Task<List<Sale>> SearchAsync(
        DateOnly? dateFrom, DateOnly? dateTo, int? customerId, int? cashierId, string? paymentStatus)
    {
        var query = _context.Sales.Include(s => s.Customer).AsQueryable();

        if (dateFrom.HasValue)
        {
            var from = dateFrom.Value.ToDateTime(TimeOnly.MinValue);
            query = query.Where(s => s.SaleDate >= from);
        }
        if (dateTo.HasValue)
        {
            var to = dateTo.Value.ToDateTime(TimeOnly.MaxValue);
            query = query.Where(s => s.SaleDate <= to);
        }
        if (customerId.HasValue) query = query.Where(s => s.CustomerId == customerId.Value);
        if (cashierId.HasValue) query = query.Where(s => s.UserId == cashierId.Value);
        if (!string.IsNullOrWhiteSpace(paymentStatus) && paymentStatus != "All")
            query = query.Where(s => s.PaymentStatus == paymentStatus);

        return await query.OrderByDescending(s => s.SaleDate).ToListAsync();
    }

    public async Task<int> GetItemCountAsync(int saleId) =>
        await _context.SaleItems.CountAsync(i => i.SaleId == saleId);

    public async Task<User?> GetUserByIdAsync(int userId) =>
        await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

    public async Task<Dictionary<int, int>> GetItemCountsAsync(List<int> saleIds) =>
        await _context.SaleItems
            .Where(i => saleIds.Contains(i.SaleId))
            .GroupBy(i => i.SaleId)
            .Select(g => new { SaleId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.SaleId, x => x.Count);

    public async Task<Dictionary<int, string>> GetUserNamesAsync(List<int> userIds) =>
        await _context.Users
            .Where(u => userIds.Contains(u.UserId))
            .ToDictionaryAsync(u => u.UserId, u => u.FullName);

    public async Task<List<User>> GetActiveUsersAsync() =>
        await _context.Users.Where(u => u.Status == "Active").AsNoTracking().ToListAsync();

    public async Task<List<int>> GetDistinctSaleUserIdsAsync() =>
        await _context.Sales.Select(s => s.UserId).Distinct().ToListAsync();

    public async Task<List<User>> GetUsersByIdsAsync(List<int> userIds) =>
        await _context.Users.Where(u => userIds.Contains(u.UserId)).AsNoTracking().ToListAsync();

    public async Task<Sale?> GetByIdAsync(int saleId) =>
        await _context.Sales.FirstOrDefaultAsync(s => s.SaleId == saleId);

    public async Task<Sale?> GetByIdWithItemsAsync(int saleId) =>
        await _context.Sales.Include(s => s.Items).FirstOrDefaultAsync(s => s.SaleId == saleId);

    public async Task<Sale?> GetByIdWithItemsAndCustomerAsync(int saleId) =>
        await _context.Sales.Include(s => s.Items).Include(s => s.Customer)
            .FirstOrDefaultAsync(s => s.SaleId == saleId);

    public async Task<Sale?> GetHeldByIdAsync(int saleId) =>
        await _context.Sales
            .Include(s => s.Items).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(s => s.SaleId == saleId && s.Status == "Held");

    public async Task<Sale?> GetDraftByIdAsync(int saleId) =>
        await _context.Sales
            .Include(s => s.Items).ThenInclude(i => i.Product)
            .Include(s => s.Customer)
            .FirstOrDefaultAsync(s => s.SaleId == saleId && s.Status == "Draft");

    public async Task<List<Sale>> GetByStatusAsync(string status) =>
        await _context.Sales
            .Include(s => s.Customer)
            .Where(s => s.Status == status)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();

    public async Task<ProductStock?> GetStockAsync(int productId, int branchId) =>
        await _context.ProductStocks.FirstOrDefaultAsync(s => s.ProductId == productId && s.BranchId == branchId);

    public async Task AddStockAsync(ProductStock stock) => await _context.ProductStocks.AddAsync(stock);
    public async Task AddStockMovementAsync(StockMovement movement) => await _context.StockMovements.AddAsync(movement);

    public async Task AddAsync(Sale sale) => await _context.Sales.AddAsync(sale);
    public async Task AddItemAsync(SaleItem item) => await _context.SaleItems.AddAsync(item);
    public void Delete(Sale sale) => _context.Sales.Remove(sale);
    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() >= 0;
}