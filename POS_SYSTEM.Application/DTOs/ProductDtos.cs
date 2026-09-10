using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Application.DTOs;

public record LookupDto(int Id, string Name);

public record ProductCreateDto(
    string ProductName,
    string? Barcode,
    int CategoryId,
    int? BrandId,
    int UnitId,
    decimal PurchasePrice,
    decimal SalePrice,
    decimal TaxPercent,
    int OpeningStock,
    int ReorderLevel,
    string? ImageUrl,
    string Status,
    string? Description
);

// Same shape as create, mirroring FastAPI's ProductUpdate(ProductBase)
public record ProductUpdateDto(
    string ProductName,
    string? Barcode,
    int CategoryId,
    int? BrandId,
    int UnitId,
    decimal PurchasePrice,
    decimal SalePrice,
    decimal TaxPercent,
    int OpeningStock,
    int ReorderLevel,
    string? ImageUrl,
    string Status,
    string? Description
);

public record ProductOutDto(
    int ProductId,
    string ProductCode,
    string ProductName,
    string? Barcode,
    int CategoryId,
    int? BrandId,
    int UnitId,
    decimal PurchasePrice,
    decimal SalePrice,
    decimal TaxPercent,
    int OpeningStock,
    int ReorderLevel,
    int CurrentStock,
    string? ImageUrl,
    string Status,
    string? Description,
    DateTime? CreatedAt,
    string? CategoryName,
    string? BrandName
);

public record BulkPriceUpdateRequestDto(
    List<int> ProductIds,
    string UpdateType,   // "percentage" or "fixed"
    decimal Value,
    string PriceField = "SalePrice"
);

public record BulkPriceUpdateResultDto(
    int ProductId,
    string ProductCode,
    string ProductName,
    decimal OldPrice,
    decimal NewPrice
);

public record UpdatePriceDto(decimal SalePrice);