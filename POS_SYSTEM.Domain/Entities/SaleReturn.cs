using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class SaleReturn
{
    public int SaleReturnId { get; set; }

    public int OriginalSaleId { get; set; }

    public string ReturnType { get; set; } = null!;

    public int CompanyId { get; set; }

    public int BranchId { get; set; }

    public int? CustomerId { get; set; }

    public int UserId { get; set; }

    public DateTime ReturnDate { get; set; }

    public string? Reason { get; set; }

    public string? Note { get; set; }

    public decimal SubTotal { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal GrandTotal { get; set; }

    public string? RefundMethod { get; set; }

    public decimal ReceivedAmount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Sale OriginalSale { get; set; } = null!;

    public virtual ICollection<SaleReturnItem> SaleReturnItems { get; set; } = new List<SaleReturnItem>();

    public virtual User User { get; set; } = null!;
}
