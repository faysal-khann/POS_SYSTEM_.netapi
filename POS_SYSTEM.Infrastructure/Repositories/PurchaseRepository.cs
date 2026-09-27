using POS_SYSTEM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using POS_SYSTEM.Domain.Interfaces;
using POS_SYSTEM.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Infrastructure.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly AppDbContext _context;

        public PurchaseRepository(AppDbContext context) => _context = context;

        public async Task<List<Purchase>> SearchAsync(DateOnly? dateFrom, DateOnly? dateTo, int? supplierId, string? status)
        {
            var query = _context.Purchases.Include(p => p.Supplier).AsQueryable();

            if (dateFrom.HasValue) query = query.Where(p => p.PurchaseDate >= dateFrom.Value);
            if (dateTo.HasValue) query = query.Where(p => p.PurchaseDate <= dateTo.Value);
            if (supplierId.HasValue) query = query.Where(p => p.SupplierId == supplierId.Value);
            if (!string.IsNullOrWhiteSpace(status) && status != "All") query = query.Where(p => p.Status == status);

            return await query.OrderByDescending(p => p.PurchaseDate).ToListAsync();
        }

        public async Task<int> GetItemCountAsync(int purchaseId) =>
            await _context.PurchaseItems.CountAsync(i => i.PurchaseId == purchaseId);

        public async Task<Purchase?> GetByIdWithItemsAsync(int purchaseId) =>
            await _context.Purchases
                .Include(p => p.PurchaseItems).ThenInclude(i => i.Product)
                .Include(p => p.PurchaseItems).ThenInclude(i => i.Size)
                .FirstOrDefaultAsync(p => p.PurchaseId == purchaseId);

        public async Task<int> GetMaxPurchaseIdAsync() =>
            await _context.Purchases.AnyAsync() ? await _context.Purchases.MaxAsync(p => p.PurchaseId) : 0;

        public async Task<ProductStock?> GetStockAsync(int productId, int branchId) =>
            await _context.ProductStocks.FirstOrDefaultAsync(s => s.ProductId == productId && s.BranchId == branchId);

        public async Task AddStockAsync(ProductStock stock) => await _context.ProductStocks.AddAsync(stock);

        public async Task AddStockMovementAsync(StockMovement movement) => await _context.StockMovements.AddAsync(movement);

        public async Task<List<Branch>> GetBranchesAsync() => await _context.Branches.AsNoTracking().ToListAsync();

        public async Task<Branch?> GetLastBranchWithCodePrefixAsync() =>
            await _context.Branches
                .Where(b => b.BranchCode != null && b.BranchCode.StartsWith("BR-"))
                .OrderByDescending(b => b.BranchId)
                .FirstOrDefaultAsync();

        public async Task AddBranchAsync(Branch branch) => await _context.Branches.AddAsync(branch);

        public async Task<List<Size>> GetActiveSizesOrderedAsync() =>
            await _context.Sizes.Where(s => s.Status == "Active").OrderBy(s => s.SortOrder).AsNoTracking().ToListAsync();

        public async Task AddAsync(Purchase purchase) => await _context.Purchases.AddAsync(purchase);
        public async Task AddItemAsync(PurchaseItem item) => await _context.PurchaseItems.AddAsync(item);
        public void Delete(Purchase purchase) => _context.Purchases.Remove(purchase);
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() >= 0;
    }
}
