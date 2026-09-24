using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Infrastructure.Persistence.Entities;

public partial class LoyaltyTransaction
{
    public int LoyaltyTransactionId { get; set; }

    public int CustomerId { get; set; }

    public string RefNo { get; set; } = null!;

    public string TransactionType { get; set; } = null!;

    public int Points { get; set; }

    public string? Description { get; set; }

    public int? SaleId { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User? CreatedByUser { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Sale? Sale { get; set; }
}
