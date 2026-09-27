using Microsoft.EntityFrameworkCore;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using POS_SYSTEM.Infrastructure.Persistence;

namespace POS_SYSTEM.Infrastructure.Repositories;

public class CashierShiftRepository : ICashierShiftRepository
{
    private readonly AppDbContext _context;
    public CashierShiftRepository(AppDbContext context) => _context = context;

    public async Task<(decimal TotalSales, decimal TotalReceived, decimal CashSales)> GetSalesTotalsAsync(
        int userId, int branchId, DateTime startUtc, DateTime endUtc)
    {
        var query = _context.Sales.Where(s =>
            s.Status == "Completed" &&
            s.UserId == userId &&
            s.BranchId == branchId &&
            s.SaleDate >= startUtc &&
            s.SaleDate < endUtc);

        var totalSales = await query.SumAsync(s => (decimal?)s.GrandTotal) ?? 0;
        var totalReceived = await query.SumAsync(s => (decimal?)((s.ReceivedAmount ?? 0) - (s.ChangeAmount ?? 0))) ?? 0;
        var cashSales = await query.Where(s => s.PaymentMethod == "Cash")
            .SumAsync(s => (decimal?)((s.ReceivedAmount ?? 0) - (s.ChangeAmount ?? 0))) ?? 0;

        return (totalSales, totalReceived, cashSales);
    }

    public async Task<(decimal TotalReturns, decimal CashRefunds)> GetReturnTotalsAsync(
        int userId, int branchId, DateTime startLocal, DateTime endLocal)
    {
        var query = _context.SaleReturns.Where(r =>
            r.UserId == userId &&
            r.BranchId == branchId &&
            r.ReturnDate >= startLocal &&
            r.ReturnDate < endLocal);

        var totalReturns = await query.SumAsync(r => (decimal?)r.GrandTotal) ?? 0;
        var cashRefunds = await query.Where(r => r.RefundMethod == "Cash")
            .SumAsync(r => (decimal?)r.GrandTotal) ?? 0;

        return (totalReturns, cashRefunds);
    }

    public async Task<bool> ShiftAlreadyClosedAsync(int userId, int branchId, DateOnly shiftDate, string shiftName)
    {
        return await _context.CashierShifts.AnyAsync(s =>
            s.UserId == userId &&
            s.BranchId == branchId &&
            s.ShiftDate == shiftDate &&
            s.ShiftName == shiftName);
    }

    public async Task<CashierShift> AddAsync(CashierShift shift)
    {
        _context.CashierShifts.Add(shift);
        await _context.SaveChangesAsync();
        return shift;
    }
}