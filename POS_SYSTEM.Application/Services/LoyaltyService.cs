using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Exceptions;
using POS_SYSTEM.Application.Interfaces;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;

namespace POS_SYSTEM.Application.Services;

public class LoyaltyService : ILoyaltyService
{
    private static readonly HashSet<string> ValidTransactionTypes = new() { "Earn", "Redeem", "Adjust" };

    private readonly ILoyaltyRepository _repo;

    public LoyaltyService(ILoyaltyRepository repo) => _repo = repo;

    public async Task<List<LoyaltySummaryDto>> GetSummaryAsync()
    {
        var customers = await _repo.GetAllCustomersAsync();
        var ids = customers.Select(c => c.CustomerId).ToList();
        var pointsMap = await _repo.GetAvailablePointsForCustomersAsync(ids);

        return customers.Select(c => new LoyaltySummaryDto(
            c.CustomerId, c.CustomerName, pointsMap.GetValueOrDefault(c.CustomerId, 0)
        )).ToList();
    }

    public async Task<int> GetAvailablePointsAsync(int customerId) =>
        await _repo.GetAvailablePointsAsync(customerId);

    public async Task<List<LoyaltyHistoryItemDto>> GetHistoryAsync(int customerId)
    {
        var transactions = await _repo.GetHistoryAsync(customerId);
        return transactions
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new LoyaltyHistoryItemDto(
                t.LoyaltyTransactionId, t.RefNo, t.TransactionType, t.Points, t.Description, t.SaleId, t.CreatedAt
            )).ToList();
    }

    public async Task AdjustAsync(LoyaltyAdjustDto dto)
    {
        if (!ValidTransactionTypes.Contains(dto.TransactionType))
            throw new BadRequestAppException($"Invalid transaction type. Must be one of: {string.Join(", ", ValidTransactionTypes)}");

        if (dto.Points <= 0)
            throw new BadRequestAppException("Points must be greater than zero.");

        if (dto.TransactionType == "Redeem")
        {
            var available = await _repo.GetAvailablePointsAsync(dto.CustomerId);
            if (dto.Points > available)
                throw new BadRequestAppException("Not enough loyalty points available.");
        }

        await _repo.AddTransactionAsync(new LoyaltyTransaction
        {
            CustomerId = dto.CustomerId,
            RefNo = dto.RefNo ?? $"ADJ-{DateTime.UtcNow:yyyyMMddHHmmss}",
            TransactionType = dto.TransactionType,
            Points = dto.Points,
            Description = dto.Description,
            CreatedByUserId = dto.CreatedByUserId,
            CreatedAt = DateTime.UtcNow
        });

        await _repo.SaveChangesAsync();
    }

    public async Task ReverseSaleTransactionsAsync(int saleId)
    {
        var transactions = await _repo.GetBySaleIdAsync(saleId);
        foreach (var t in transactions)
            _repo.RemoveTransaction(t);

        if (transactions.Count > 0)
            await _repo.SaveChangesAsync();
    }
}