using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class CashierShift
{
    public int ShiftId { get; set; }

    public int CompanyId { get; set; }

    public int BranchId { get; set; }

    public int UserId { get; set; }

    public int ClosedByUserId { get; set; }

    public string ShiftName { get; set; } = null!;

    public DateOnly ShiftDate { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public decimal OpeningBalance { get; set; }

    public decimal TotalSales { get; set; }

    public decimal TotalReturns { get; set; }

    public decimal NetSales { get; set; }

    public decimal TotalReceived { get; set; }

    public decimal CashSales { get; set; }

    public decimal CashRefunds { get; set; }

    public decimal CalculatedClosing { get; set; }

    public decimal ActualClosing { get; set; }

    public decimal Difference { get; set; }

    public string? Note { get; set; }

    public string Status { get; set; } = null!;

    public DateTime ClosedAt { get; set; }

    public virtual User ClosedByUser { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
