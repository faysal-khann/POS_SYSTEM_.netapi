using POS_SYSTEM.Domain.Entities;

namespace POS_SYSTEM.Domain.Interfaces;

public interface ISaleRepository
{
    Task<int> CountTodayInvoicesAsync(DateOnly today);
    Task<string?> GetLastDraftNoAsync(string prefix);

    Task<List<Sale>> SearchAsync(DateOnly? dateFrom, DateOnly? dateTo, int? customerId, int? cashierId, string? paymentStatus);
    Task<int> GetItemCountAsync(int saleId);
    Task<User?> GetUserByIdAsync(int userId);
    Task<Dictionary<int, int>> GetItemCountsAsync(List<int> saleIds);
    Task<Dictionary<int, string>> GetUserNamesAsync(List<int> userIds);

    Task<List<User>> GetActiveUsersAsync();
    Task<List<int>> GetDistinctSaleUserIdsAsync();
    Task<List<User>> GetUsersByIdsAsync(List<int> userIds);

    Task<Sale?> GetByIdAsync(int saleId);
    Task<Sale?> GetByIdWithItemsAsync(int saleId);
    Task<Sale?> GetByIdWithItemsAndCustomerAsync(int saleId);
    Task<Sale?> GetHeldByIdAsync(int saleId);
    Task<Sale?> GetDraftByIdAsync(int saleId);
    Task<List<Sale>> GetByStatusAsync(string status);

    Task<ProductStock?> GetStockAsync(int productId, int branchId);
    Task AddStockAsync(ProductStock stock);
    Task AddStockMovementAsync(StockMovement movement);

    Task AddAsync(Sale sale);
    Task AddItemAsync(SaleItem item);
    void Delete(Sale sale);
    Task<bool> SaveChangesAsync();
}