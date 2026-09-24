using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Exceptions;
using POS_SYSTEM.Application.Interfaces;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;

namespace POS_SYSTEM.Application.Services;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _repo;
    private readonly ILoyaltyRepository _loyaltyRepo;

    public SaleService(ISaleRepository repo, ILoyaltyRepository loyaltyRepo)
    {
        _repo = repo;
        _loyaltyRepo = loyaltyRepo;
    }

    private async Task<string> GenerateInvoiceNoAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var countToday = await _repo.CountTodayInvoicesAsync(today);
        return $"INV-{today:yyyyMMdd}{(countToday + 1):D4}";
    }

    private async Task<string> GenerateDraftNoAsync()
    {
        var prefix = $"DRAFT-{DateTime.UtcNow:yyMMdd}-";
        var last = await _repo.GetLastDraftNoAsync(prefix);

        int seq = 1;
        if (last is not null)
        {
            var lastSegment = last.Split('-').Last();
            if (int.TryParse(lastSegment, out var n)) seq = n + 1;
        }

        return $"{prefix}{seq:D4}";
    }

    public async Task<string> GetNextInvoiceNoAsync() => await GenerateInvoiceNoAsync();

    public async Task<List<LookupDto>> GetCashiersListAsync()
    {
        var users = await _repo.GetActiveUsersAsync();
        return users.Select(u => new LookupDto(u.UserId, u.FullName)).ToList();
    }

    public async Task<List<LookupDto>> GetCashiersAsync()
    {
        var userIds = await _repo.GetDistinctSaleUserIdsAsync();
        if (userIds.Count == 0) return new List<LookupDto>();

        var users = await _repo.GetUsersByIdsAsync(userIds);
        return users.Select(u => new LookupDto(u.UserId, u.FullName)).ToList();
    }

    public async Task<SaleOutDto> CreateAsync(SaleCreateDto dto)
    {
        if (dto.Items.Count == 0)
            throw new BadRequestAppException("Add at least one item to the sale.");

        var status = dto.Status ?? "Completed";
        var invoiceNo = status == "Draft" ? await GenerateDraftNoAsync() : await GenerateInvoiceNoAsync();

        var sale = new Sale
        {
            InvoiceNo = invoiceNo,
            CompanyId = dto.CompanyId,
            BranchId = dto.BranchId,
            CustomerId = dto.CustomerId,
            UserId = dto.UserId,
            SaleDate = dto.SaleDate ?? DateTime.UtcNow,
            PriceType = dto.PriceType,
            SubTotal = dto.SubTotal,
            DiscountAmount = dto.DiscountAmount,
            TaxAmount = dto.TaxAmount,
            GrandTotal = dto.GrandTotal,
            PaymentMethod = dto.PaymentMethod,
            ReceivedAmount = dto.ReceivedAmount,
            ChangeAmount = dto.ChangeAmount,
            PaymentStatus = dto.PaymentStatus ?? "Paid",
            ParkName = dto.ParkName,
            Status = status
        };

        await _repo.AddAsync(sale);
        await _repo.SaveChangesAsync(); // populates sale.SaleId

        foreach (var item in dto.Items)
        {
            await _repo.AddItemAsync(new SaleItem
            {
                SaleId = sale.SaleId,
                ProductId = item.ProductId,
                Qty = item.Qty,
                UnitPrice = item.UnitPrice,
                DiscountPercent = item.DiscountPercent,
                TaxPercent = item.TaxPercent,
                LineTotal = item.LineTotal
            });

            // only completed sales move stock; held/draft don't
            if (status == "Completed")
            {
                var stock = await _repo.GetStockAsync(item.ProductId, dto.BranchId);
                var previous = stock?.CurrentStock ?? 0;
                var newBalance = previous - (int)item.Qty;

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
                    await _repo.SaveChangesAsync(); // populate stock.ProductStockId
                }

                await _repo.AddStockMovementAsync(new StockMovement
                {
                    ProductStockId = stock.ProductStockId,
                    BranchId = dto.BranchId,
                    ProductId = item.ProductId,
                    MovementType = "Sale",
                    ReferenceType = "Sale",
                    ReferenceId = sale.SaleId,
                    QtyIn = 0,
                    QtyOut = item.Qty,
                    BalanceQty = newBalance
                });
            }
        }

        await _repo.SaveChangesAsync();
        // =========================================================
        // LOYALTY POINT REDEMPTION
        // =========================================================

        if (status == "Completed"
            && dto.CustomerId.HasValue
            && (dto.PointsToRedeem ?? 0) > 0)
        {
            var customerId = dto.CustomerId.Value;
            var requestedPoints = dto.PointsToRedeem!.Value;

            // Get the customer's current available points.
            var available = await _loyaltyRepo.GetAvailablePointsAsync(customerId);

            // Never allow redemption to make the balance negative.
            if (requestedPoints > available)
            {
                throw new BadRequestAppException(
                    $"Customer does not have enough loyalty points. " +
                    $"Available: {available}, Requested: {requestedPoints}.");
            }

            if (requestedPoints > 0)
            {
                sale.PointsRedeemed = requestedPoints;
                sale.RedeemedAmount = requestedPoints;

                await _loyaltyRepo.AddTransactionAsync(new LoyaltyTransaction
                {
                    CustomerId = customerId,
                    SaleId = sale.SaleId,
                    RefNo = sale.InvoiceNo,
                    TransactionType = "Redeem",
                    Points = requestedPoints,
                    Description = $"Redeemed on {sale.InvoiceNo}",
                    CreatedByUserId = dto.UserId,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        // =========================================================
        // LOYALTY POINT EARNING
        // =========================================================

        if (status == "Completed" && dto.CustomerId.HasValue)
        {
            var netPaid =
                (sale.GrandTotal ?? 0) -
                (sale.RedeemedAmount ?? 0);

            var earned = (int)(Math.Floor(netPaid / 1000) * 10);

            if (earned > 0)
            {
                sale.PointsEarned = earned;

                await _loyaltyRepo.AddTransactionAsync(new LoyaltyTransaction
                {
                    CustomerId = dto.CustomerId.Value,
                    SaleId = sale.SaleId,
                    RefNo = sale.InvoiceNo,
                    TransactionType = "Earn",
                    Points = earned,
                    Description = $"Earned on {sale.InvoiceNo}",
                    CreatedByUserId = dto.UserId,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        // Save loyalty transactions + PointsEarned/PointsRedeemed.
        await _repo.SaveChangesAsync();

        return new SaleOutDto(
            sale.SaleId,
            sale.InvoiceNo,
            sale.GrandTotal,
            sale.ChangeAmount,
            sale.Status
        );

        return new SaleOutDto(sale.SaleId, sale.InvoiceNo, sale.GrandTotal, sale.ChangeAmount, sale.Status);
    }

    public async Task<List<SaleListItemDto>> GetSalesAsync(
        DateOnly? dateFrom, DateOnly? dateTo, int? customerId, int? cashierId, string? paymentStatus)
    {
        var sales = await _repo.SearchAsync(dateFrom, dateTo, customerId, cashierId, paymentStatus);
        var result = new List<SaleListItemDto>();

        foreach (var s in sales)
        {
            var itemCount = await _repo.GetItemCountAsync(s.SaleId);
            var cashier = await _repo.GetUserByIdAsync(s.UserId);

            result.Add(new SaleListItemDto(
                s.SaleId, s.InvoiceNo, s.SaleDate,
                s.Customer?.CustomerName ?? "Walk-in Customer",
                itemCount, s.GrandTotal, s.PaymentMethod, s.PaymentStatus,
                cashier?.FullName ?? "—", s.Status
            ));
        }

        return result;
    }

    public async Task<bool> DeleteAsync(int saleId)
    {
        var sale = await _repo.GetByIdAsync(saleId);
        if (sale is null)
            return false;

        // Find loyalty transactions belonging to this sale.
        var transactions = await _loyaltyRepo.GetBySaleIdAsync(saleId);

        // Remove loyalty transactions first.
        foreach (var transaction in transactions)
        {
            _loyaltyRepo.RemoveTransaction(transaction);
        }

        // Delete the sale.
        _repo.Delete(sale);

        // Save everything.
        return await _repo.SaveChangesAsync();
    }

    public async Task<List<HeldSaleListItemDto>> GetHeldSalesAsync()
    {
        var sales = await _repo.GetByStatusAsync("Held");
        var saleIds = sales.Select(s => s.SaleId).ToList();
        var itemCounts = await _repo.GetItemCountsAsync(saleIds);
        var userIds = sales.Select(s => s.UserId).Distinct().ToList();
        var userNames = await _repo.GetUserNamesAsync(userIds);

        return sales.Select(s => new HeldSaleListItemDto(
            s.SaleId, s.ParkName, s.SaleDate,
            s.Customer?.CustomerName ?? "Walk-in Customer",
            itemCounts.GetValueOrDefault(s.SaleId, 0),
            s.GrandTotal,
            userNames.GetValueOrDefault(s.UserId, "—")
        )).ToList();
    }

    public async Task<HeldSaleDetailDto?> GetHeldSaleDetailAsync(int saleId)
    {
        var sale = await _repo.GetHeldByIdAsync(saleId);
        if (sale is null) return null;

        var items = sale.SaleItems.Select(i => new SaleItemDetailDto(
            i.ProductId, i.Product?.ProductName ?? "—", i.Qty, i.UnitPrice, i.DiscountPercent, i.TaxPercent
        )).ToList();

        return new HeldSaleDetailDto(sale.SaleId, sale.ParkName, sale.CustomerId, sale.BranchId, sale.CompanyId, items);
    }

    public async Task<List<DraftSaleListItemDto>> GetDraftSalesAsync()
    {
        var sales = await _repo.GetByStatusAsync("Draft");
        if (sales.Count == 0) return new List<DraftSaleListItemDto>();

        var saleIds = sales.Select(s => s.SaleId).ToList();
        var itemCounts = await _repo.GetItemCountsAsync(saleIds);
        var userIds = sales.Select(s => s.UserId).Distinct().ToList();
        var userNames = await _repo.GetUserNamesAsync(userIds);

        return sales.Select(s => new DraftSaleListItemDto(
            s.SaleId, s.InvoiceNo, s.SaleDate,
            s.Customer?.CustomerName ?? "Walk-in Customer",
            itemCounts.GetValueOrDefault(s.SaleId, 0),
            s.GrandTotal,
            userNames.GetValueOrDefault(s.UserId, "—")
        )).ToList();
    }

    public async Task<DraftSaleDetailDto?> GetDraftSaleDetailAsync(int saleId)
    {
        var sale = await _repo.GetDraftByIdAsync(saleId);
        if (sale is null) return null;

        var cashier = await _repo.GetUserByIdAsync(sale.UserId);

        var items = sale.SaleItems.Select(i => new SaleItemDraftDetailDto(
            i.ProductId, i.Product?.ProductName ?? "—", i.Qty, i.UnitPrice,
            i.DiscountPercent ?? 0, i.TaxPercent ?? 0, i.LineTotal
        )).ToList();

        return new DraftSaleDetailDto(
            sale.SaleId, sale.InvoiceNo, sale.SaleDate, sale.CustomerId,
            sale.Customer?.CustomerName ?? "Walk-in Customer",
            cashier?.FullName ?? "—",
            sale.BranchId, sale.CompanyId,
            sale.SubTotal ?? 0, sale.DiscountAmount ?? 0, sale.TaxAmount ?? 0, sale.GrandTotal ?? 0,
            items
        );
    }
}