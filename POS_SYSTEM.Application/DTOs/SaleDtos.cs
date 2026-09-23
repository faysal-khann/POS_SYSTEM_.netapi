namespace POS_SYSTEM.Application.DTOs;

public record SaleItemCreateDto(int ProductId, decimal Qty, decimal UnitPrice, decimal? DiscountPercent, decimal? TaxPercent, decimal LineTotal);

public record SaleCreateDto(
    int CompanyId,
    int BranchId,
    int? CustomerId,
    int UserId,
    string? PriceType,
    decimal SubTotal,
    decimal? DiscountAmount,
    decimal? TaxAmount,
    decimal GrandTotal,
    string? PaymentMethod,
    decimal? ReceivedAmount,
    decimal? ChangeAmount,
    string? Status,
    string? ParkName,
    List<SaleItemCreateDto> Items,
    DateTime? SaleDate,
    string? PaymentStatus
);

public record SaleOutDto(int SaleId, string InvoiceNo, decimal? GrandTotal, decimal? ChangeAmount, string? Status);

public record SaleListItemDto(
    int SaleId, string InvoiceNo, DateTime? SaleDate, string CustomerName,
    int TotalItems, decimal? GrandTotal, string? PaymentMethod, string? PaymentStatus,
    string CashierName, string? Status
);

public record HeldSaleListItemDto(int SaleId, string? ParkName, DateTime? SaleDate, string CustomerName, int TotalItems, decimal? GrandTotal, string CashierName);

public record SaleItemDetailDto(int ProductId, string ProductName, decimal Qty, decimal UnitPrice, decimal? DiscountPercent, decimal? TaxPercent);

public record HeldSaleDetailDto(int SaleId, string? ParkName, int? CustomerId, int BranchId, int CompanyId, List<SaleItemDetailDto> Items);

public record DraftSaleListItemDto(int SaleId, string DraftNo, DateTime? SaleDate, string CustomerName, int TotalItems, decimal? GrandTotal, string CashierName);

public record SaleItemDraftDetailDto(int ProductId, string ProductName, decimal Qty, decimal UnitPrice, decimal? DiscountPercent, decimal? TaxPercent, decimal LineTotal);

public record DraftSaleDetailDto(
    int SaleId, string DraftNo, DateTime? SaleDate, int? CustomerId, string CustomerName, string CashierName,
    int BranchId, int CompanyId, decimal SubTotal, decimal DiscountAmount, decimal TaxAmount, decimal GrandTotal,
    List<SaleItemDraftDetailDto> Items
);