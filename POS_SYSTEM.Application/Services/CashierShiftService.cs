using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;

namespace POS_SYSTEM.Application.Services;

public class CashierShiftService : ICashierShiftService
{
    private readonly ICashierShiftRepository _repo;
    public CashierShiftService(ICashierShiftRepository repo) => _repo = repo;

    // Sales.SaleDate is naive UTC — ensure comparisons use UTC-normalized values
    private static DateTime ToNaiveUtc(DateTime dt) =>
        dt.Kind == DateTimeKind.Utc ? dt : dt.ToUniversalTime();

    // SaleReturns.ReturnDate defaults to SQL Server's local clock (GETDATE),
    // so its window has to be shifted to the server's local time, same as FastAPI did.
    private static DateTime UtcToServerLocal(DateTime utcDt) =>
        TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utcDt, DateTimeKind.Utc), TimeZoneInfo.Local);

    private async Task<ShiftSummaryDto> ComputeSummaryAsync(int userId, int branchId, DateTime startUtc, DateTime endUtc)
    {
        var (totalSales, totalReceived, cashSales) = await _repo.GetSalesTotalsAsync(userId, branchId, startUtc, endUtc);

        var rStart = UtcToServerLocal(startUtc);
        var rEnd = UtcToServerLocal(endUtc);
        var (totalReturns, cashRefunds) = await _repo.GetReturnTotalsAsync(userId, branchId, rStart, rEnd);

        return new ShiftSummaryDto(
            Math.Round(totalSales, 2),
            Math.Round(totalReturns, 2),
            Math.Round(totalSales - totalReturns, 2),
            Math.Round(totalReceived, 2),
            Math.Round(cashSales, 2),
            Math.Round(cashRefunds, 2)
        );
    }

    public async Task<ShiftSummaryDto> GetSummaryAsync(int userId, int branchId, DateTime start, DateTime end)
    {
        var startUtc = ToNaiveUtc(start);
        var endUtc = ToNaiveUtc(end);

        if (endUtc <= startUtc)
            throw new InvalidOperationException("Shift end must be after its start.");

        return await ComputeSummaryAsync(userId, branchId, startUtc, endUtc);
    }

    public async Task<CashierShiftResultDto> CloseShiftAsync(CreateCashierShiftDto dto)
    {
        var startUtc = ToNaiveUtc(dto.WindowStart);
        var endUtc = ToNaiveUtc(dto.WindowEnd);

        if (endUtc <= startUtc)
            throw new InvalidOperationException("Shift end must be after its start.");

        var already = await _repo.ShiftAlreadyClosedAsync(dto.UserId, dto.BranchId, dto.ShiftDate, dto.ShiftName);
        if (already)
            throw new InvalidOperationException("This shift has already been closed.");

        var summary = await ComputeSummaryAsync(dto.UserId, dto.BranchId, startUtc, endUtc);
        var calculated = Math.Round(dto.OpeningBalance + summary.CashSales - summary.CashRefunds, 2);
        var difference = Math.Round(dto.ActualClosing - calculated, 2);

        var shift = new CashierShift
        {
            CompanyId = dto.CompanyId,
            BranchId = dto.BranchId,
            UserId = dto.UserId,
            ClosedByUserId = dto.ClosedByUserId, // ← confirm this property name matches your entity
            ShiftName = dto.ShiftName,
            ShiftDate = dto.ShiftDate,
            StartTime = startUtc,
            EndTime = endUtc,
            OpeningBalance = dto.OpeningBalance,
            TotalSales = summary.TotalSales,
            TotalReturns = summary.TotalReturns,
            NetSales = summary.NetSales,
            TotalReceived = summary.TotalReceived,
            CashSales = summary.CashSales,
            CashRefunds = summary.CashRefunds,
            CalculatedClosing = calculated,
            ActualClosing = dto.ActualClosing,
            Difference = difference,
            Note = dto.Note,
            Status = "Closed",
            ClosedAt = DateTime.UtcNow
        };

        var created = await _repo.AddAsync(shift);
        return new CashierShiftResultDto(created.ShiftId, calculated, dto.ActualClosing, difference);
    }
}