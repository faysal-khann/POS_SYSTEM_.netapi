using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Application.DTOs
{
    public record CompanyListItemDto(
    int CompanyId,
    string CompanyName,
    string? Phone,
    string? Email,
    string? Address,
    string? Currency,
    string Status
);

    public record CompanyCreateDto(
        string CompanyName,
        string? Phone,
        string? Email,
        string? Address,
        string? Country,
        string? Currency,
        string? TaxNo,
        bool? IsActive
    );

    public record CompanyDetailDto(
        // Mirrors FastAPI's response_model=CompanyCreate on GET /{id} — no CompanyID field, by design of the original (quirk noted).
        string CompanyName,
        string? Phone,
        string? Email,
        string? Address,
        string? Country,
        string? Currency,
        string? TaxNo,
        bool? IsActive
    );

    public record CompanyOutDto(int CompanyId, string CompanyName);

    public record LogoUploadResultDto(string LogoPath);
}
