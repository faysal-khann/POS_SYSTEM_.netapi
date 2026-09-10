using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string CustomerCode { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public string CustomerGroup { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    public string? NationalIdTaxId { get; set; }

    public string AddressLine1 { get; set; } = null!;

    public string? AddressLine2 { get; set; }

    public string City { get; set; } = null!;

    public string? StateDivision { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public decimal? OpeningBalance { get; set; }

    public decimal? CreditLimit { get; set; }

    public decimal? DueAmount { get; set; }

    public string? Notes { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<LoyaltyTransaction> LoyaltyTransactions { get; set; } = new List<LoyaltyTransaction>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
