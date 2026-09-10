using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class LoyaltyTransaction
{
    public long LoyaltyId { get; set; }

    public int CustomerId { get; set; }

    public int? SaleId { get; set; }

    public int Points { get; set; }

    public string TransactionType { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Sale? Sale { get; set; }
}
