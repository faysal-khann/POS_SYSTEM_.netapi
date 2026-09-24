using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Infrastructure.Persistence.Entities;

public partial class Purchase
{
    public int PurchaseId { get; set; }

    public int CompanyId { get; set; }

    public int BranchId { get; set; }

    public int SupplierId { get; set; }

    public string PurchaseNo { get; set; } = null!;

    public DateOnly PurchaseDate { get; set; }

    public string? PaymentTerm { get; set; }

    public string? ReferenceNo { get; set; }

    public string? Remarks { get; set; }

    public decimal? SubTotal { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal? TaxPercent { get; set; }

    public decimal? TaxAmount { get; set; }

    public decimal? ShippingCharge { get; set; }

    public decimal? GrandTotal { get; set; }

    public string? Status { get; set; }

    public string? PaymentStatus { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

    public virtual Supplier Supplier { get; set; } = null!;
}
