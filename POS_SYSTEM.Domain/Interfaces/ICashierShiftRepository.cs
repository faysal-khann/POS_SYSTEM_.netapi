using POS_SYSTEM.Domain.Entities;

namespace POS_SYSTEM.Domain.Interfaces;

public interface ICashierShiftRepository
{
    Task<(decimal TotalSales, decimal TotalReceived, decimal CashSales)> GetSalesTotalsAsync(
        int userId, int branchId, DateTime startUtc, DateTime endUtc);

    Task<(decimal TotalReturns, decimal CashRefunds)> GetReturnTotalsAsync(
        int userId, int branchId, DateTime startLocal, DateTime endLocal);

    Task<bool> ShiftAlreadyClosedAsync(int userId, int branchId, DateOnly shiftDate, string shiftName);

    Task<CashierShift> AddAsync(CashierShift shift);
}