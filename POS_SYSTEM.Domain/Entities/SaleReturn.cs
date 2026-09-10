using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class SaleReturn
{
    public int ReturnId { get; set; }

    public int SaleId { get; set; }

    public string ReturnNo { get; set; } = null!;

    public DateTime ReturnDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string RefundMethod { get; set; } = null!;

    public int CreatedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Sale Sale { get; set; } = null!;

    public virtual ICollection<SaleReturnItem> SaleReturnItems { get; set; } = new List<SaleReturnItem>();
}
