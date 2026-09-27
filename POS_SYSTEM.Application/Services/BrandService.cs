using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;

namespace POS_SYSTEM.Application.Services;

public class BrandService : IBrandService
{
    private readonly IBrandRepository _repo;

    public BrandService(IBrandRepository repo) => _repo = repo;

    public async Task<List<BrandOutDto>> GetAllAsync()
    {
        var brands = await _repo.GetAllAsync();
        return brands.Select(MapToDto).ToList();
    }

    public async Task<BrandOutDto?> GetByIdAsync(int id)
    {
        var b = await _repo.GetByIdAsync(id);
        return b is null ? null : MapToDto(b);
    }

    public async Task<BrandOutDto> CreateAsync(BrandCreateDto dto)
    {
        var brand = new Brand
        {
            BrandName = dto.BrandName,
            Description = dto.Description,
            Status = dto.Status ?? "Active",
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(brand);
        await _repo.SaveChangesAsync();
        return MapToDto(brand);
    }

    public async Task<BrandOutDto?> UpdateAsync(int id, BrandUpdateDto dto)
    {
        var brand = await _repo.GetByIdAsync(id);
        if (brand is null) return null;

        brand.BrandName = dto.BrandName;
        brand.Description = dto.Description;
        brand.Status = dto.Status ?? "Active";

        await _repo.SaveChangesAsync();
        return MapToDto(brand);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var brand = await _repo.GetByIdAsync(id);
        if (brand is null) return false;

        _repo.Delete(brand);
        return await _repo.SaveChangesAsync();
    }

    private static BrandOutDto MapToDto(Brand b) =>
        new(b.BrandId, b.BrandName, b.Description, b.Status, b.CreatedAt);
}