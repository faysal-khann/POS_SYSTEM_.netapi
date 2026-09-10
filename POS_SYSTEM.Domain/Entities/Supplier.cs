using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string SupplierCode { get; set; } = null!;

    public string SupplierName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? City { get; set; }

    public decimal? DueAmount { get; set; }

    public string Status { get; set; } = null!;

    public string? Website { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? StateDivision { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public string? ContactPerson { get; set; }

    public string? ContactPersonPhone { get; set; }

    public string? TaxVatNo { get; set; }

    public decimal? OpeningBalance { get; set; }

    public decimal? CreditLimit { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
