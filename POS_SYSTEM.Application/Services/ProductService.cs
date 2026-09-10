using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;

namespace POS_SYSTEM.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo) => _repo = repo;

    public async Task<List<ProductOutDto>> GetAllAsync()
    {
        var products = await _repo.GetAllAsync();
        return products.Select(MapToDto).ToList();
    }

    public async Task<ProductOutDto?> GetByIdAsync(int id)
    {
        var p = await _repo.GetByIdAsync(id);
        return p is null ? null : MapToDto(p);
    }

    public async Task<ProductOutDto?> GetByBarcodeAsync(string barcode)
    {
        var p = await _repo.GetByBarcodeAsync(barcode);
        return p is null ? null : MapToDto(p);
    }

    public async Task<string> GetNextProductCodeAsync()
    {
        var maxId = await _repo.GetMaxProductIdAsync();
        return $"P{maxId + 1:D3}";
    }

    public async Task<List<LookupDto>> GetCategoriesAsync() =>
        (await _repo.GetCategoriesAsync()).Select(c => new LookupDto(c.CategoryId, c.CategoryName)).ToList();

    public async Task<List<LookupDto>> GetBrandsAsync() =>
        (await _repo.GetBrandsAsync()).Select(b => new LookupDto(b.BrandId, b.BrandName)).ToList();

    public async Task<List<LookupDto>> GetUnitsAsync() =>
        (await _repo.GetUnitsAsync()).Select(u => new LookupDto(u.UnitId, u.UnitName)).ToList();

    public async Task<ProductOutDto> CreateAsync(ProductCreateDto dto)
    {
        var code = await GetNextProductCodeAsync();

        var product = new Product
        {
            ProductCode = code,
            ProductName = dto.ProductName,
            Barcode = dto.Barcode,
            CategoryId = dto.CategoryId,
            BrandId = dto.BrandId,
            UnitId = dto.UnitId,
            PurchasePrice = dto.PurchasePrice,
            SalePrice = dto.SalePrice,
            TaxPercent = dto.TaxPercent,
            OpeningStock = dto.OpeningStock,
            ReorderLevel = dto.ReorderLevel,
            CurrentStock = dto.OpeningStock, // mirrors FastAPI: CurrentStock = OpeningStock on create
            ImageUrl = dto.ImageUrl,
            Status = dto.Status,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(product);
        await _repo.SaveChangesAsync();

        var saved = await _repo.GetByIdAsync(product.ProductId);
        return MapToDto(saved!);
    }

    public async Task<ProductOutDto?> UpdateAsync(int id, ProductUpdateDto dto)
    {
        var product = await _repo.GetByIdAsync(id);
        if (product is null) return null;

        product.ProductName = dto.ProductName;
        product.Barcode = dto.Barcode;
        product.CategoryId = dto.CategoryId;
        product.BrandId = dto.BrandId;
        product.UnitId = dto.UnitId;
        product.PurchasePrice = dto.PurchasePrice;
        product.SalePrice = dto.SalePrice;
        product.TaxPercent = dto.TaxPercent;
        product.OpeningStock = dto.OpeningStock;
        product.ReorderLevel = dto.ReorderLevel;
        product.ImageUrl = dto.ImageUrl;
        product.Status = dto.Status;
        product.Description = dto.Description;
        product.UpdatedAt = DateTime.UtcNow;

        _repo.Update(product);
        await _repo.SaveChangesAsync();

        var saved = await _repo.GetByIdAsync(id);
        return MapToDto(saved!);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _repo.GetByIdAsync(id);
        if (product is null) return false;

        _repo.Delete(product);
        return await _repo.SaveChangesAsync();
    }

    public async Task<List<BulkPriceUpdateResultDto>> BulkPriceUpdateAsync(BulkPriceUpdateRequestDto dto)
    {
        if (dto.PriceField is not ("SalePrice" or "PurchasePrice"))
            throw new ArgumentException("Invalid price field");

        var products = await _repo.GetByIdsAsync(dto.ProductIds);
        if (products.Count == 0)
            throw new KeyNotFoundException("No matching products found");

        var results = new List<BulkPriceUpdateResultDto>();

        foreach (var p in products)
        {
            var oldPrice = dto.PriceField == "SalePrice" ? p.SalePrice : p.PurchasePrice;

            decimal newPrice = dto.UpdateType switch
            {
                "percentage" => Math.Round(oldPrice * (1 + dto.Value / 100), 2),
                "fixed" => Math.Round(oldPrice + dto.Value, 2),
                _ => throw new ArgumentException("Invalid update type")
            };

            if (dto.PriceField == "SalePrice") p.SalePrice = newPrice;
            else p.PurchasePrice = newPrice;

            p.UpdatedAt = DateTime.UtcNow;

            results.Add(new BulkPriceUpdateResultDto(p.ProductId, p.ProductCode, p.ProductName, oldPrice, newPrice));
        }

        await _repo.SaveChangesAsync();
        return results;
    }

    public async Task<ProductOutDto?> UpdatePriceAsync(int id, decimal newPrice)
    {
        if (newPrice < 0) throw new ArgumentException("Invalid price");

        var product = await _repo.GetByIdAsync(id);
        if (product is null) return null;

        product.SalePrice = newPrice;
        product.UpdatedAt = DateTime.UtcNow;

        _repo.Update(product);
        await _repo.SaveChangesAsync();

        var saved = await _repo.GetByIdAsync(id);
        return MapToDto(saved!);
    }

    private static ProductOutDto MapToDto(Product p) => new(
        p.ProductId, p.ProductCode, p.ProductName, p.Barcode,
        p.CategoryId, p.BrandId, p.UnitId,
        p.PurchasePrice, p.SalePrice, p.TaxPercent,
        p.OpeningStock, p.ReorderLevel, p.CurrentStock,
        p.ImageUrl, p.Status, p.Description, p.CreatedAt,
        p.Category?.CategoryName, p.Brand?.BrandName
    );
}