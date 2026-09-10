using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class Expense
{
    public int ExpenseId { get; set; }

    public int BranchId { get; set; }

    public int ExpenseCategoryId { get; set; }

    public DateOnly ExpenseDate { get; set; }

    public string? Description { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string? ReferenceNo { get; set; }

    public int CreatedBy { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ExpenseCategory ExpenseCategory { get; set; } = null!;
}
