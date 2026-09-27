namespace POS_SYSTEM.Application.DTOs;

public record ReturnableItemDto(
    int ProductID,
    string ProductName,
    decimal QtyAvailable,
    decimal UnitPrice,
    decimal TaxPercent
);

public record SaleLookupResultDto(
    int SaleID,
    string InvoiceNo,
    int? CustomerID,
    string CustomerName,
    int BranchID,
    int CompanyID,
    List<ReturnableItemDto> Items
);

public record SaleReturnItemInputDto(
    int ProductID,
    decimal ReturnQty,
    decimal UnitPrice,
    decimal LineTotal
);

public record SaleReturnCreateDto(
    int OriginalSaleID,
    string ReturnType,
    int CompanyID,
    int BranchID,
    int? CustomerID,
    int UserID,
    string? Reason,
    string? Note,
    decimal SubTotal,
    decimal TaxAmount,
    decimal GrandTotal,
    string RefundMethod,
    decimal ReceivedAmount,
    List<SaleReturnItemInputDto> Items
);

public record SaleReturnOutDto(
    int SaleReturnID,
    decimal GrandTotal,
    string Status = "Completed"
);