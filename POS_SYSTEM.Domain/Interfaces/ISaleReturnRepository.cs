using POS_SYSTEM.Domain.Entities;

namespace POS_SYSTEM.Domain.Interfaces;

public interface ISaleReturnRepository
{
    Task<Sale?> GetSaleByInvoiceNoWithDetailsAsync(string invoiceNo);
    Task<decimal> GetTotalReturnedQtyAsync(int originalSaleId, int productId);
    Task<SaleReturn> CreateSaleReturnAsync(SaleReturn saleReturn);
    Task RestockAndRecordMovementAsync(int productId, int branchId, int saleReturnId, decimal returnQty);
}