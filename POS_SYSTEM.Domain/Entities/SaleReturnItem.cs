using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class SaleReturnItem
{
    public int SaleReturnItemId { get; set; }

    public int SaleReturnId { get; set; }

    public int ProductId { get; set; }

    public decimal ReturnQty { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal LineTotal { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual SaleReturn SaleReturn { get; set; } = null!;
}
