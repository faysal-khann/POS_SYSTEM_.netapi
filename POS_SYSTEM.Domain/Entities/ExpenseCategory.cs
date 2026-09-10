using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class ExpenseCategory
{
    public int ExpenseCategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
