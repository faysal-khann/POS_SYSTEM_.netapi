using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Application.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _repo;

        public PurchaseService(IPurchaseRepository repo) => _repo = repo;

        public async Task<List<PurchaseListItemDto>> GetPurchasesAsync(
            DateOnly? dateFrom, DateOnly? dateTo, int? supplierId, string? status)
        {
            var purchases = await _repo.SearchAsync(dateFrom, dateTo, supplierId, status);
            var result = new List<PurchaseListItemDto>();

            foreach (var p in purchases)
            {
                var itemCount = await _repo.GetItemCountAsync(p.PurchaseId);
                result.Add(new PurchaseListItemDto(
                    p.PurchaseId, p.PurchaseNo, p.PurchaseDate,
                    p.Supplier?.SupplierName ?? "—",
                    itemCount, p.GrandTotal, p.Status, p.PaymentStatus
                ));
            }

            return result;
        }

        public async Task<PurchaseOutDto?> GetByIdAsync(int id)
        {
            var p = await _repo.GetByIdWithItemsAsync(id);
            return p is null ? null : MapToDto(p);
        }

        // Mirrors the SECOND (effectively active) generate_purchase_no in the FastAPI source:
        // ignores per-day counting, uses global max PurchaseID + 1.
        private async Task<string> GeneratePurchaseNoAsync(DateOnly purchaseDate)
        {
            var maxId = await _repo.GetMaxPurchaseIdAsync();
            var yy = (purchaseDate.Year % 100).ToString("D2");
            var mm = purchaseDate.Month.ToString("D2");
            return $"PUR-{yy}{mm}{(maxId + 1):D4}";
        }

        public async Task<string> PreviewNextNumberAsync(DateOnly purchaseDate) =>
            await GeneratePurchaseNoAsync(purchaseDate);

        public async Task<PurchaseOutDto> CreateAsync(PurchaseCreateDto dto)
        {
            var purchaseNo = await GeneratePurchaseNoAsync(dto.PurchaseDate);

            var purchase = new Purchase
            {
                PurchaseNo = purchaseNo,
                CompanyId = dto.CompanyId,
                BranchId = dto.BranchId,
                SupplierId = dto.SupplierId,
                PurchaseDate = dto.PurchaseDate,
                PaymentTerm = dto.PaymentTerm,
                ReferenceNo = dto.ReferenceNo,
                Remarks = dto.Remarks,
                SubTotal = dto.SubTotal ?? 0,
                DiscountAmount = dto.DiscountAmount ?? 0,
                TaxPercent = dto.TaxPercent ?? 0,
                TaxAmount = dto.TaxAmount ?? 0,
                ShippingCharge = dto.ShippingCharge ?? 0,
                GrandTotal = dto.GrandTotal ?? 0,
                Status = dto.Status ?? "Completed",
                PaymentStatus = dto.PaymentStatus ?? "Paid"
            };

            await _repo.AddAsync(purchase);
            await _repo.SaveChangesAsync(); // populates purchase.PurchaseId

            foreach (var item in dto.Items)
            {
                await _repo.AddItemAsync(new PurchaseItem
                {
                    PurchaseId = purchase.PurchaseId,
                    ProductId = item.ProductId,
                    SizeId = item.SizeId,
                    BatchNo = item.BatchNo,
                    Qty = item.Qty,
                    UnitPrice = item.UnitPrice,
                    DiscountPercent = item.DiscountPercent ?? 0,
                    LineTotal = item.LineTotal
                });

                // find or create stock row for this product + branch
                var stock = await _repo.GetStockAsync(item.ProductId, dto.BranchId);
                var previousBalance = stock?.CurrentStock ?? 0;
                var newBalance = previousBalance + (int)item.Qty;

                if (stock is not null)
                {
                    stock.CurrentStock = newBalance;
                }
                else
                {
                    stock = new ProductStock
                    {
                        ProductId = item.ProductId,
                        BranchId = dto.BranchId,
                        CurrentStock = newBalance
                    };
                    await _repo.AddStockAsync(stock);
                    await _repo.SaveChangesAsync(); // populate stock.ProductStockId before referencing it
                }

                await _repo.AddStockMovementAsync(new StockMovement
                {
                    ProductStockId = stock.ProductStockId,
                    BranchId = dto.BranchId,
                    ProductId = item.ProductId,
                    MovementType = "Purchase",
                    ReferenceType = "Purchase",
                    ReferenceId = purchase.PurchaseId,
                    QtyIn = item.Qty,
                    QtyOut = 0,
                    BalanceQty = newBalance
                });
            }

            await _repo.SaveChangesAsync();

            var saved = await _repo.GetByIdWithItemsAsync(purchase.PurchaseId);
            return MapToDto(saved!);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var purchase = await _repo.GetByIdWithItemsAsync(id);
            if (purchase is null) return false;

            _repo.Delete(purchase);
            return await _repo.SaveChangesAsync();
        }

        public async Task<List<SizeLookupDto>> GetSizesAsync() =>
            (await _repo.GetActiveSizesOrderedAsync()).Select(s => new SizeLookupDto(s.SizeId, s.SizeName)).ToList();

        public async Task<List<BranchLookupDto>> GetBranchesAsync() =>
            (await _repo.GetBranchesAsync()).Select(b => new BranchLookupDto(b.BranchId, b.BranchName)).ToList();

        public async Task<BranchDetailDto> CreateBranchAsync(BranchCreateDto dto)
        {
            var nextCode = await GenerateNextBranchCodeAsync();

            var branch = new Branch
            {
                CompanyId = dto.CompanyId,
                BranchCode = nextCode,
                BranchName = dto.BranchName,
                ManagerName = dto.ManagerName,
                Phone = dto.Phone,
                Address = dto.Address,
                Email = dto.Email,
                IsActive = (dto.Status ?? "Active") == "Active"
            };

            await _repo.AddBranchAsync(branch);
            await _repo.SaveChangesAsync();

            return new BranchDetailDto(
                branch.BranchId,
                branch.BranchCode,
                branch.BranchName,
                branch.ManagerName,
                branch.Phone,
                branch.Email,
                branch.Address,
                (branch.IsActive ?? false) ? "Active" : "Inactive"
            );
        }

        private async Task<string> GenerateNextBranchCodeAsync()
        {
            var last = await _repo.GetLastBranchWithCodePrefixAsync();
            if (last?.BranchCode is null) return "BR-001";

            var parts = last.BranchCode.Split('-');
            var lastNumber = parts.Length > 1 && int.TryParse(parts[1], out var n) ? n : 0;
            return $"BR-{(lastNumber + 1):D3}";
        }

        private static PurchaseOutDto MapToDto(Purchase p) => new(
            p.PurchaseId, p.CompanyId, p.BranchId, p.SupplierId, p.PurchaseNo, p.PurchaseDate,
            p.PaymentTerm, p.ReferenceNo, p.Remarks,
            p.SubTotal, p.DiscountAmount, p.TaxPercent, p.TaxAmount, p.ShippingCharge, p.GrandTotal,
            p.Status, p.PaymentStatus, p.CreatedAt,
            p.PurchaseItems.Select(i => new PurchaseItemOutDto(
                i.PurchaseItemId, i.ProductId, i.BatchNo, i.Qty, i.UnitPrice, i.DiscountPercent, i.LineTotal,
                i.SizeId, i.Product?.ProductName, i.Size?.SizeName
            )).ToList()
        );
    }
}
