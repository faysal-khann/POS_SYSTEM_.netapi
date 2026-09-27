using POS_SYSTEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Domain.Interfaces
{
    public interface IPurchaseRepository
    {
        Task<List<Purchase>> SearchAsync(DateOnly? dateFrom, DateOnly? dateTo, int? supplierId, string? status);
        Task<int> GetItemCountAsync(int purchaseId);
        Task<Purchase?> GetByIdWithItemsAsync(int purchaseId);
        Task<int> GetMaxPurchaseIdAsync();

        Task<ProductStock?> GetStockAsync(int productId, int branchId);
        Task AddStockAsync(ProductStock stock);
        Task AddStockMovementAsync(StockMovement movement);

        Task<List<Branch>> GetBranchesAsync();
        Task<Branch?> GetLastBranchWithCodePrefixAsync();
        Task AddBranchAsync(Branch branch);

        Task<List<Size>> GetActiveSizesOrderedAsync();

        Task AddAsync(Purchase purchase);
        Task AddItemAsync(PurchaseItem item);
        void Delete(Purchase purchase);
        Task<bool> SaveChangesAsync();
    }
}
