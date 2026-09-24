namespace POS_SYSTEM.Application.DTOs;

public record LoyaltySummaryDto(int CustomerId, string CustomerName, int AvailablePoints);

public record LoyaltyAdjustDto(
    int CustomerId,
    string TransactionType,   // "Earn" / "Redeem" / "Adjust"
    int Points,
    string? RefNo,
    string? Description,
    int? CreatedByUserId
);

public record LoyaltyHistoryItemDto(
    int LoyaltyTransactionId,
    string RefNo,
    string TransactionType,
    int Points,
    string? Description,
    int? SaleId,
    DateTime CreatedAt
);