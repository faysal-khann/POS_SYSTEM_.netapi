using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class StockMovement
{
    public long MovementId { get; set; }

    public int BranchId { get; set; }

    public int ProductId { get; set; }

    public string MovementType { get; set; } = null!;

    public string? ReferenceType { get; set; }

    public int? ReferenceId { get; set; }

    public decimal QtyIn { get; set; }

    public decimal QtyOut { get; set; }

    public decimal BalanceQty { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? ProductStockId { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual ProductStock? ProductStock { get; set; }
}
