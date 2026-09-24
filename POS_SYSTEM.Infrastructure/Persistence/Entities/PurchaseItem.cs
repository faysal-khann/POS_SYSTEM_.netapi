using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Infrastructure.Persistence.Entities;

public partial class PurchaseItem
{
    public int PurchaseItemId { get; set; }

    public int PurchaseId { get; set; }

    public int ProductId { get; set; }

    public string? BatchNo { get; set; }

    public decimal Qty { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal? DiscountPercent { get; set; }

    public decimal LineTotal { get; set; }

    public int? SizeId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Purchase Purchase { get; set; } = null!;

    public virtual Size? Size { get; set; }
}
