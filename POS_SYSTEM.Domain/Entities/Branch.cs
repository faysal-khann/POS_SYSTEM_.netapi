using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class Branch
{
    public int BranchId { get; set; }

    public int CompanyId { get; set; }

    public string? BranchCode { get; set; }

    public string BranchName { get; set; } = null!;

    public string? ManagerName { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public bool? IsActive { get; set; }

    public string? Email { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
