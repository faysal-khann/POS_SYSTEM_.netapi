namespace POS_SYSTEM.Application.DTOs;

public record StockListItemDto(
    int ProductStockId,
    string ProductCode,
    string ProductName,
    int? CategoryId,
    string? CategoryName,
    int? BrandId,
    string? BrandName,
    string? UnitShortName,
    int BranchId,
    string BranchName,
    int CurrentStock,
    decimal StockValue,
    string Status
);

public record StockDetailDto(
    int ProductStockId,
    int ProductId,
    string ProductCode,
    string ProductName,
    string? CategoryName,
    string? BrandName,
    string? UnitShortName,
    string? Barcode,
    int BranchId,
    string BranchName,
    int CurrentStock,
    int ReservedStock,
    int ReorderLevel,
    int MaximumLevel,
    decimal PurchasePrice,
    decimal StockValue,
    string Status,
    DateTime? LastUpdatedAt,
    string? ImageUrl
);

public record StockUpdateDto(int BranchId, int CurrentStock, int? ReservedStock, int? ReorderLevel, int? MaximumLevel);