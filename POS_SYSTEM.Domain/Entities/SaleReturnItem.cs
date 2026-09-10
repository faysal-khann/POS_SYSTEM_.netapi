using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class SaleReturnItem
{
    public int ReturnItemId { get; set; }

    public int ReturnId { get; set; }

    public int ProductId { get; set; }

    public decimal Qty { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal LineTotal { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual SaleReturn Return { get; set; } = null!;
}
