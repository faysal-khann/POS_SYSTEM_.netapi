using POS_SYSTEM.Application.DTOs;

namespace POS_SYSTEM.Application.Interfaces;

public interface ILoyaltyService
{
    Task<List<LoyaltySummaryDto>> GetSummaryAsync();
    Task<int> GetAvailablePointsAsync(int customerId);
    Task<List<LoyaltyHistoryItemDto>> GetHistoryAsync(int customerId);
    Task AdjustAsync(LoyaltyAdjustDto dto);
    Task ReverseSaleTransactionsAsync(int saleId);
}