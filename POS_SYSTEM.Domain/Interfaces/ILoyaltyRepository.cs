using POS_SYSTEM.Domain.Entities;

namespace POS_SYSTEM.Domain.Interfaces;

public interface ILoyaltyRepository
{
    Task<int> GetAvailablePointsAsync(int customerId);
    Task<List<Customer>> GetAllCustomersAsync();
    Task<Dictionary<int, int>> GetAvailablePointsForCustomersAsync(List<int> customerIds);
    Task<List<LoyaltyTransaction>> GetHistoryAsync(int customerId);
    Task<List<LoyaltyTransaction>> GetBySaleIdAsync(int saleId);

    Task AddTransactionAsync(LoyaltyTransaction transaction);
    void RemoveTransaction(LoyaltyTransaction transaction);
    Task<bool> SaveChangesAsync();
}