using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class ProductStock
{
    public int ProductStockId { get; set; }

    public int ProductId { get; set; }

    public int BranchId { get; set; }

    public int CurrentStock { get; set; }

    public int ReservedStock { get; set; }

    public int ReorderLevel { get; set; }

    public int MaximumLevel { get; set; }

    public DateTime? LastUpdatedAt { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}
