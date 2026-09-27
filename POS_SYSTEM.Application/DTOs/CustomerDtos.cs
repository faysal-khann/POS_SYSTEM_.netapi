using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Application.DTOs
{
    public record CustomerCreateDto(
     string CustomerName,
     string Phone,
     string? Email,
     string CustomerGroup,
     DateOnly? DateOfBirth,
     string? NationalIdTaxId,
     string AddressLine1,
     string? AddressLine2,
     string City,
     string? StateDivision,
     string? PostalCode,
     string? Country,
     decimal OpeningBalance,
     decimal CreditLimit,
     decimal DueAmount,
     string? Notes,
     string Status
 );

    // Mirrors FastAPI: CustomerUpdate(CustomerBase) — same shape as create
    public record CustomerUpdateDto(
        string CustomerName,
        string Phone,
        string? Email,
        string CustomerGroup,
        DateOnly? DateOfBirth,
        string? NationalIdTaxId,
        string AddressLine1,
        string? AddressLine2,
        string City,
        string? StateDivision,
        string? PostalCode,
        string? Country,
        decimal OpeningBalance,
        decimal CreditLimit,
        decimal DueAmount,
        string? Notes,
        string Status
    );

    public record CustomerOutDto(
        int CustomerId,
        string CustomerCode,
        string CustomerName,
        string Phone,
        string? Email,
        string CustomerGroup,
        DateOnly? DateOfBirth,
        string? NationalIdTaxId,
        string AddressLine1,
        string? AddressLine2,
        string City,
        string? StateDivision,
        string? PostalCode,
        string? Country,
        decimal? OpeningBalance,
        decimal? CreditLimit,
        decimal? DueAmount,
        string? Notes,
        string Status,
        DateTime? CreatedAt
    );
}
