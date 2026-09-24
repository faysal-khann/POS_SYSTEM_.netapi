namespace POS_SYSTEM.Application.DTOs;

public record ShiftSummaryDto(
    decimal TotalSales, decimal TotalReturns, decimal NetSales,
    decimal TotalReceived, decimal CashSales, decimal CashRefunds
);

public record CreateCashierShiftDto(
    int CompanyId, int BranchId, int UserId, int ClosedByUserId,
    string ShiftName, DateOnly ShiftDate,
    DateTime WindowStart, DateTime WindowEnd,
    decimal OpeningBalance, decimal ActualClosing, string? Note
);

public record CashierShiftResultDto(int ShiftId, decimal CalculatedClosing, decimal ActualClosing, decimal Difference);