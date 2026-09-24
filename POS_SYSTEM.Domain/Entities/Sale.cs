using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class Sale
{
    public int SaleId { get; set; }

    public int CompanyId { get; set; }

    public int BranchId { get; set; }

    public int? CustomerId { get; set; }

    public int UserId { get; set; }

    public string InvoiceNo { get; set; } = null!;

    public DateTime SaleDate { get; set; }

    public string? PriceType { get; set; }

    public decimal? SubTotal { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal? TaxAmount { get; set; }

    public decimal? GrandTotal { get; set; }

    public string? PaymentMethod { get; set; }

    public decimal? ReceivedAmount { get; set; }

    public decimal? ChangeAmount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public string? ParkName { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<LoyaltyTransaction> LoyaltyTransactions { get; set; } = new List<LoyaltyTransaction>();

    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();

    public virtual ICollection<SaleReturn> SaleReturns { get; set; } = new List<SaleReturn>();

    public virtual User User { get; set; } = null!;
}
