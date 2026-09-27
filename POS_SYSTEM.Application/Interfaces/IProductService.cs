using POS_SYSTEM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Application.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductOutDto>> GetAllAsync();
        Task<ProductOutDto?> GetByIdAsync(int id);
        Task<ProductOutDto?> GetByBarcodeAsync(string barcode);
        Task<string> GetNextProductCodeAsync();

        Task<List<LookupDto>> GetCategoriesAsync();
        Task<List<LookupDto>> GetBrandsAsync();
        Task<List<LookupDto>> GetUnitsAsync();

        Task<ProductOutDto> CreateAsync(ProductCreateDto dto);
        Task<ProductOutDto?> UpdateAsync(int id, ProductUpdateDto dto);
        Task<bool> DeleteAsync(int id);

        Task<List<BulkPriceUpdateResultDto>> BulkPriceUpdateAsync(BulkPriceUpdateRequestDto dto);
        Task<ProductOutDto?> UpdatePriceAsync(int id, decimal newPrice);
    }
}
