using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class Company
{
    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Country { get; set; }

    public string? Currency { get; set; }

    public string? TaxNo { get; set; }

    public string? LogoPath { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Website { get; set; }

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
