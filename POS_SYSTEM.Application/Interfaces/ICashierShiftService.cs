using POS_SYSTEM.Application.DTOs;

namespace POS_SYSTEM.Application.Interfaces;

public interface ICashierShiftService
{
    Task<ShiftSummaryDto> GetSummaryAsync(int userId, int branchId, DateTime start, DateTime end);
    Task<CashierShiftResultDto> CloseShiftAsync(CreateCashierShiftDto dto);
}