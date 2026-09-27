using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Application.DTOs
{
    public record SupplierCreateDto(
    string SupplierName,
    string? Phone,
    string? Email,
    string? Website,
    string? AddressLine1,
    string? AddressLine2,
    string? City,
    string? StateDivision,
    string? PostalCode,
    string? Country,
    string? ContactPerson,
    string? ContactPersonPhone,
    string? TaxVatNo,
    decimal? OpeningBalance,
    decimal? CreditLimit,
    decimal? DueAmount,
    string? Notes,
    string? Status
);

    // Mirrors FastAPI: SupplierUpdate(SupplierBase) — same shape as create
    public record SupplierUpdateDto(
        string SupplierName,
        string? Phone,
        string? Email,
        string? Website,
        string? AddressLine1,
        string? AddressLine2,
        string? City,
        string? StateDivision,
        string? PostalCode,
        string? Country,
        string? ContactPerson,
        string? ContactPersonPhone,
        string? TaxVatNo,
        decimal? OpeningBalance,
        decimal? CreditLimit,
        decimal? DueAmount,
        string? Notes,
        string? Status
    );

    public record SupplierOutDto(
        int SupplierId,
        string SupplierCode,
        string SupplierName,
        string? Phone,
        string? Email,
        string? Website,
        string? AddressLine1,
        string? AddressLine2,
        string? City,
        string? StateDivision,
        string? PostalCode,
        string? Country,
        string? ContactPerson,
        string? ContactPersonPhone,
        string? TaxVatNo,
        decimal? OpeningBalance,
        decimal? CreditLimit,
        decimal? DueAmount,
        string? Notes,
        string? Status
    );
}
