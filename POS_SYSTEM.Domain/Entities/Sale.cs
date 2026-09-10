using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class Sale
{
    public int SaleId { get; set; }

    public int BranchId { get; set; }

    public int? CustomerId { get; set; }

    public string InvoiceNo { get; set; } = null!;

    public DateTime SaleDate { get; set; }

    public decimal SubTotal { get; set; }

    public decimal Discount { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal PaidAmount { get; set; }

    public decimal ChangeAmount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int CashierId { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual User Cashier { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<LoyaltyTransaction> LoyaltyTransactions { get; set; } = new List<LoyaltyTransaction>();

    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();

    public virtual ICollection<SaleReturn> SaleReturns { get; set; } = new List<SaleReturn>();
}
