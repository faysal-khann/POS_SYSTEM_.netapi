using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Application.DTOs
{
    public record BranchListItemDto(
    int BranchId,
    string? BranchCode,
    string BranchName,
    string? ManagerName,
    string? Phone,
    string? Address,
    string Status
);

    public record BranchDetailDto(
        int BranchId,
        string? BranchCode,
        string BranchName,
        string? ManagerName,
        string? Phone,
        string? Email,
        string? Address,
        string Status
    );

    public record BranchCreateDto(
        int CompanyId,
        string BranchName,
        string? ManagerName,
        string? Phone,
        string? Email,
        string? Address,
        string? Status
    );

    public record BranchUpdateDto(
        string BranchName,
        string? ManagerName,
        string? Phone,
        string? Email,
        string? Address,
        string? Status
    );
}
