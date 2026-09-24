using Microsoft.EntityFrameworkCore;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using POS_SYSTEM.Infrastructure.Persistence;

namespace POS_SYSTEM.Infrastructure.Repositories;

public class LoyaltyRepository : ILoyaltyRepository
{
    private readonly AppDbContext _context;

    public LoyaltyRepository(AppDbContext context) => _context = context;

    public async Task<int> GetAvailablePointsAsync(int customerId)
    {
        var earned = await _context.LoyaltyTransactions
            .Where(t => t.CustomerId == customerId && (t.TransactionType == "Earn" || t.TransactionType == "Adjust"))
            .SumAsync(t => t.Points);

        var redeemed = await _context.LoyaltyTransactions
            .Where(t => t.CustomerId == customerId && t.TransactionType == "Redeem")
            .SumAsync(t => t.Points);

        return earned - redeemed;
    }

    public async Task<List<Customer>> GetAllCustomersAsync() =>
        await _context.Customers.AsNoTracking().ToListAsync();

    public async Task<Dictionary<int, int>> GetAvailablePointsForCustomersAsync(List<int> customerIds)
    {
        var result = new Dictionary<int, int>();
        foreach (var id in customerIds)
            result[id] = await GetAvailablePointsAsync(id);
        return result;
    }

    public async Task<List<LoyaltyTransaction>> GetHistoryAsync(int customerId) =>
        await _context.LoyaltyTransactions
            .Where(t => t.CustomerId == customerId)
            .AsNoTracking()
            .ToListAsync();

    public async Task<List<LoyaltyTransaction>> GetBySaleIdAsync(int saleId) =>
        await _context.LoyaltyTransactions
            .Where(t => t.SaleId == saleId)
            .ToListAsync();

    public async Task AddTransactionAsync(LoyaltyTransaction transaction) =>
        await _context.LoyaltyTransactions.AddAsync(transaction);

    public void RemoveTransaction(LoyaltyTransaction transaction) =>
        _context.LoyaltyTransactions.Remove(transaction);

    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() >= 0;
}