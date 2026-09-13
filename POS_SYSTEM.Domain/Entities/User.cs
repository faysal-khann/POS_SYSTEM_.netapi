using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string PasswordHash { get; set; } = null!;

    public int RoleId { get; set; }

    public int PrimaryBranchId { get; set; }

    public string? EmployeeId { get; set; }

    public string? Designation { get; set; }

    public string? Address { get; set; }

    public string? Notes { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? LastLoginAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<ActivityLog1> ActivityLog1s { get; set; } = new List<ActivityLog1>();

    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual Branch PrimaryBranch { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<SaleReturn> SaleReturns { get; set; } = new List<SaleReturn>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
   
}
