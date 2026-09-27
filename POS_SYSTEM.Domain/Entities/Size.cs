using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class Size
{
    public int SizeId { get; set; }

    public string SizeName { get; set; } = null!;

    public string? SizeCode { get; set; }

    public int SortOrder { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();
}
