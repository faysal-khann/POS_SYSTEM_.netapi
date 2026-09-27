using POS_SYSTEM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Application.Interfaces
{
    public interface IPurchaseService
    {
        Task<List<PurchaseListItemDto>> GetPurchasesAsync(DateOnly? dateFrom, DateOnly? dateTo, int? supplierId, string? status);
        Task<PurchaseOutDto?> GetByIdAsync(int id);
        Task<PurchaseOutDto> CreateAsync(PurchaseCreateDto dto);
        Task<bool> DeleteAsync(int id);

        Task<string> PreviewNextNumberAsync(DateOnly purchaseDate);
        Task<List<SizeLookupDto>> GetSizesAsync();
        Task<List<BranchLookupDto>> GetBranchesAsync();
        Task<BranchDetailDto> CreateBranchAsync(BranchCreateDto dto);
    }
}
