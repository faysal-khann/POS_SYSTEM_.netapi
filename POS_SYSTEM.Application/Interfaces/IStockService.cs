using POS_SYSTEM.Application.DTOs;

namespace POS_SYSTEM.Application.Interfaces;

public interface IStockService
{
    Task<List<StockListItemDto>> GetStockAsync(string? search, int? categoryId, int? brandId, int? branchId);
    Task<StockDetailDto?> GetDetailAsync(int stockId);
    Task<StockDetailDto?> UpdateAsync(int stockId, StockUpdateDto dto);
}