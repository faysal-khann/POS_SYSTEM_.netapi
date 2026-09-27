using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;

namespace POS_SYSTEM.Application.Services;

public class StockService : IStockService
{
    private readonly IStockRepository _repo;

    public StockService(IStockRepository repo) => _repo = repo;

    public async Task<List<StockListItemDto>> GetStockAsync(string? search, int? categoryId, int? brandId, int? branchId)
    {
        var rows = await _repo.SearchAsync(search, categoryId, brandId, branchId);

        return rows.Select(r =>
        {
            var status = r.CurrentStock <= r.ReorderLevel ? "Low Stock" : "In Stock";
            return new StockListItemDto(
                r.ProductStockId,
                r.Product!.ProductCode,
                r.Product.ProductName,
                r.Product.CategoryId,
                r.Product.Category?.CategoryName,
                r.Product.BrandId,
                r.Product.Brand?.BrandName,
                r.Product.Unit?.ShortName,
                r.BranchId,
                r.Branch!.BranchName,
                r.CurrentStock,
                r.CurrentStock * r.Product.PurchasePrice,
                status
            );
        }).ToList();
    }

    public async Task<StockDetailDto?> GetDetailAsync(int stockId)
    {
        var r = await _repo.GetByIdWithDetailsAsync(stockId);
        if (r is null) return null;

        return MapToDetailDto(r);
    }

    public async Task<StockDetailDto?> UpdateAsync(int stockId, StockUpdateDto dto)
    {
        var stock = await _repo.GetByIdWithDetailsAsync(stockId);
        if (stock is null) return null;

        stock.BranchId = dto.BranchId;
        stock.CurrentStock = dto.CurrentStock;
        stock.ReservedStock = dto.ReservedStock ?? 0;
        stock.ReorderLevel = dto.ReorderLevel ?? 0;
        stock.MaximumLevel = dto.MaximumLevel ?? 0;
        stock.LastUpdatedAt = DateTime.UtcNow;

        await _repo.SaveChangesAsync();

        var refreshed = await _repo.GetByIdWithDetailsAsync(stockId);
        return MapToDetailDto(refreshed!);
    }

    private static StockDetailDto MapToDetailDto(ProductStock r)
    {
        var status = r.CurrentStock <= r.ReorderLevel ? "Low Stock" : "In Stock";
        return new StockDetailDto(
            r.ProductStockId,
            r.ProductId,
            r.Product!.ProductCode,
            r.Product.ProductName,
            r.Product.Category?.CategoryName,
            r.Product.Brand?.BrandName,
            r.Product.Unit?.ShortName,
            r.Product.Barcode,
            r.BranchId,
            r.Branch!.BranchName,
            r.CurrentStock,
            r.ReservedStock,
            r.ReorderLevel,
            r.MaximumLevel,
            r.Product.PurchasePrice,
            r.CurrentStock * r.Product.PurchasePrice,
            status,
            r.LastUpdatedAt,
            r.Product.ImageUrl
        );
    }
}