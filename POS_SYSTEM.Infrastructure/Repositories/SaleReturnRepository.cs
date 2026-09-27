using Microsoft.EntityFrameworkCore;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using POS_SYSTEM.Infrastructure.Persistence;

namespace POS_SYSTEM.Infrastructure.Repositories;

public class SaleReturnRepository : ISaleReturnRepository
{
    private readonly AppDbContext _context;

    public SaleReturnRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Sale?> GetSaleByInvoiceNoWithDetailsAsync(string invoiceNo)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.SaleItems) // Updated from Items
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(s => s.InvoiceNo == invoiceNo);
    }

    public async Task<decimal> GetTotalReturnedQtyAsync(int originalSaleId, int productId)
    {
        return await _context.SaleReturnItems
            .Where(sri => sri.SaleReturn.OriginalSaleId == originalSaleId && sri.ProductId == productId) // Updated OriginalSaleId & ProductId
            .SumAsync(sri => (decimal?)sri.ReturnQty) ?? 0m;
    }

    public async Task<SaleReturn> CreateSaleReturnAsync(SaleReturn saleReturn)
    {
        await _context.SaleReturns.AddAsync(saleReturn);
        await _context.SaveChangesAsync();
        return saleReturn;
    }

    public async Task RestockAndRecordMovementAsync(int productId, int branchId, int saleReturnId, decimal returnQty)
    {
        var stock = await _context.ProductStocks
            .FirstOrDefaultAsync(s => s.ProductId == productId && s.BranchId == branchId); // Updated ProductId & BranchId

        decimal previous = stock?.CurrentStock ?? 0m;
        decimal newBalance = previous + returnQty;

        if (stock != null)
        {
            stock.CurrentStock = (int)newBalance;
        }
        else
        {
            stock = new ProductStock
            {
                ProductId = productId, // Updated ProductId
                BranchId = branchId,   // Updated BranchId
                CurrentStock = (int)newBalance
            };
            await _context.ProductStocks.AddAsync(stock);
            await _context.SaveChangesAsync();
        }

        var movement = new StockMovement
        {
            ProductStockId = stock.ProductStockId, // Updated ProductStockId
            BranchId = branchId,                   // Updated BranchId
            ProductId = productId,                 // Updated ProductId
            MovementType = "Return",
            ReferenceType = "SaleReturn",
            ReferenceId = saleReturnId,            // Updated ReferenceId
            QtyIn = returnQty,
            QtyOut = 0,
            BalanceQty = newBalance
        };

        await _context.StockMovements.AddAsync(movement);
        await _context.SaveChangesAsync();
    }
}