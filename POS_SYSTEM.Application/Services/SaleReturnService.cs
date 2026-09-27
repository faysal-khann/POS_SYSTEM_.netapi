using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Exceptions;
using POS_SYSTEM.Application.Interfaces;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;

namespace POS_SYSTEM.Application.Services;

public class SaleReturnService : ISaleReturnService
{
    private readonly ISaleReturnRepository _saleReturnRepository;

    public SaleReturnService(ISaleReturnRepository saleReturnRepository)
    {
        _saleReturnRepository = saleReturnRepository;
    }

    public async Task<SaleLookupResultDto> LookupSaleAsync(string invoiceNo)
    {
        var sale = await _saleReturnRepository.GetSaleByInvoiceNoWithDetailsAsync(invoiceNo);
        if (sale == null)
            throw new KeyNotFoundException("Invoice not found");

        var returnableItems = new List<ReturnableItemDto>();

        // Use sale.SaleItems instead of sale.Items
        foreach (var item in sale.SaleItems)
        {
            var alreadyReturned = await _saleReturnRepository.GetTotalReturnedQtyAsync(sale.SaleId, item.ProductId);
            var available = item.Qty - alreadyReturned;

            if (available > 0)
            {
                returnableItems.Add(new ReturnableItemDto(
                    item.ProductId,
                    item.Product?.ProductName ?? "—",
                    available,
                    item.UnitPrice,
                    item.TaxPercent ?? 0m // Handle nullable decimal?
                ));
            }
        }

        return new SaleLookupResultDto(
            sale.SaleId,
            sale.InvoiceNo,
            sale.CustomerId,
            sale.Customer?.CustomerName ?? "Walk-in Customer",
            sale.BranchId,
            sale.CompanyId,
            returnableItems
        );
    }

    public async Task<SaleReturnOutDto> CreateSaleReturnAsync(SaleReturnCreateDto dto)
    {
        if (dto.Items == null || dto.Items.Count == 0)
            throw new ArgumentException("Select at least one item to return.");

        var saleReturn = new SaleReturn
        {
            OriginalSaleId = dto.OriginalSaleID,
            ReturnType = string.IsNullOrWhiteSpace(dto.ReturnType) ? "Sales Return" : dto.ReturnType,
            CompanyId = dto.CompanyID,
            BranchId = dto.BranchID,
            CustomerId = dto.CustomerID,
            UserId = dto.UserID,
            Reason = dto.Reason,
            Note = dto.Note,
            SubTotal = dto.SubTotal,
            TaxAmount = dto.TaxAmount,
            GrandTotal = dto.GrandTotal,
            RefundMethod = dto.RefundMethod,
            ReceivedAmount = dto.ReceivedAmount,

            // Map items into SaleReturnItems collection
            SaleReturnItems = dto.Items.Select(i => new SaleReturnItem
            {
                ProductId = i.ProductID,
                ReturnQty = i.ReturnQty,
                UnitPrice = i.UnitPrice,
                LineTotal = i.LineTotal
            }).ToList()
        };

        var createdReturn = await _saleReturnRepository.CreateSaleReturnAsync(saleReturn);

        if (dto.ReturnType == "Sales Return")
        {
            foreach (var item in dto.Items)
            {
                await _saleReturnRepository.RestockAndRecordMovementAsync(
                    item.ProductID,
                    dto.BranchID,
                    createdReturn.SaleReturnId,
                    item.ReturnQty
                );
            }
        }

        return new SaleReturnOutDto(createdReturn.SaleReturnId, createdReturn.GrandTotal );
    }
}