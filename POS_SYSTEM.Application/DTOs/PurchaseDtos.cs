using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Application.DTOs
{
    public record PurchaseItemCreateDto(
    int ProductId,
    string? BatchNo,
    decimal Qty,
    decimal UnitPrice,
    decimal? DiscountPercent,
    decimal LineTotal,
    int? SizeId
);

    public record PurchaseItemOutDto(
        int PurchaseItemId,
        int ProductId,
        string? BatchNo,
        decimal Qty,
        decimal UnitPrice,
        decimal? DiscountPercent,
        decimal LineTotal,
        int? SizeId,
        string? ProductName,
        string? SizeName
    );

    public record PurchaseCreateDto(
        int CompanyId,
        int BranchId,
        int SupplierId,
        DateOnly PurchaseDate,
        string? PaymentTerm,
        string? ReferenceNo,
        string? Remarks,
        decimal? SubTotal,
        decimal? DiscountAmount,
        decimal? TaxPercent,
        decimal? TaxAmount,
        decimal? ShippingCharge,
        decimal? GrandTotal,
        string? Status,
        string? PaymentStatus,
        List<PurchaseItemCreateDto> Items
    );

    public record PurchaseListItemDto(
        int PurchaseId,
        string PurchaseNo,
        DateOnly PurchaseDate,
        string SupplierName,
        int TotalItems,
        decimal? TotalAmount,
        string Status,
        string PaymentStatus
    );

    public record PurchaseOutDto(
        int PurchaseId,
        int CompanyId,
        int BranchId,
        int SupplierId,
        string PurchaseNo,
        DateOnly PurchaseDate,
        string? PaymentTerm,
        string? ReferenceNo,
        string? Remarks,
        decimal? SubTotal,
        decimal? DiscountAmount,
        decimal? TaxPercent,
        decimal? TaxAmount,
        decimal? ShippingCharge,
        decimal? GrandTotal,
        string Status,
        string PaymentStatus,
        DateTime? CreatedAt,
        List<PurchaseItemOutDto> Items
    );

    public record BranchCreateDto(int CompanyId, string BranchName, string? ManagerName, string? Phone, string? Address);
    public record BranchCreatedResultDto(int Id, string Name, string? Code);
    public record BranchLookupDto(int Id, string Name);
    public record SizeLookupDto(int Id, string Name);
}
