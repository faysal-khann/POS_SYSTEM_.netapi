using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class Expense
{
    public int ExpenseId { get; set; }

    public int CompanyId { get; set; }

    public int BranchId { get; set; }

    public DateOnly ExpenseDate { get; set; }

    public int CategoryId { get; set; }

    public string? Description { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string? ReferenceNo { get; set; }

    public int? SupplierId { get; set; }

    public string? Note { get; set; }

    public bool IsRecurring { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string ExpenseNo { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual Branch Branch { get; set; } = null!;

    public virtual ExpenseCategory Category { get; set; } = null!;

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Supplier? Supplier { get; set; }
}
