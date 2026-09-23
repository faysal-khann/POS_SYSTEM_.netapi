using POS_SYSTEM.Application.DTOs;

namespace POS_SYSTEM.Application.Interfaces;

public interface ISaleService
{
    Task<string> GetNextInvoiceNoAsync();
    Task<List<LookupDto>> GetCashiersListAsync();
    Task<List<LookupDto>> GetCashiersAsync();

    Task<SaleOutDto> CreateAsync(SaleCreateDto dto);
    Task<List<SaleListItemDto>> GetSalesAsync(DateOnly? dateFrom, DateOnly? dateTo, int? customerId, int? cashierId, string? paymentStatus);
    Task<bool> DeleteAsync(int saleId);

    Task<List<HeldSaleListItemDto>> GetHeldSalesAsync();
    Task<HeldSaleDetailDto?> GetHeldSaleDetailAsync(int saleId);

    Task<List<DraftSaleListItemDto>> GetDraftSalesAsync();
    Task<DraftSaleDetailDto?> GetDraftSaleDetailAsync(int saleId);
}