using POS_SYSTEM.Application.DTOs;

namespace POS_SYSTEM.Application.Interfaces;

public interface ISaleReturnService
{
    Task<SaleLookupResultDto> LookupSaleAsync(string invoiceNo);
    Task<SaleReturnOutDto> CreateSaleReturnAsync(SaleReturnCreateDto dto);
}