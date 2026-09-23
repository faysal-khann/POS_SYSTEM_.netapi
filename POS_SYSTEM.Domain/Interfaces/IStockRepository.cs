using POS_SYSTEM.Domain.Entities;

namespace POS_SYSTEM.Domain.Interfaces;

public interface IStockRepository
{
    Task<List<ProductStock>> SearchAsync(string? search, int? categoryId, int? brandId, int? branchId);
    Task<ProductStock?> GetByIdWithDetailsAsync(int stockId);
    Task<bool> SaveChangesAsync();
}