using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;

namespace POS_SYSTEM.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repo;

    public CategoryService(ICategoryRepository repo) => _repo = repo;

    public async Task<List<CategoryOutDto>> GetAllAsync()
    {
        var categories = await _repo.GetAllAsync();
        return categories.Select(MapToDto).ToList();
    }

    public async Task<CategoryOutDto?> GetByIdAsync(int id)
    {
        var c = await _repo.GetByIdAsync(id);
        return c is null ? null : MapToDto(c);
    }

    public async Task<CategoryOutDto> CreateAsync(CategoryCreateDto dto)
    {
        var category = new Category
        {
            CategoryName = dto.CategoryName,
            Description = dto.Description,
            Status = dto.Status ?? "Active",
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(category);
        await _repo.SaveChangesAsync();
        return MapToDto(category);
    }

    public async Task<CategoryOutDto?> UpdateAsync(int id, CategoryUpdateDto dto)
    {
        var category = await _repo.GetByIdAsync(id);
        if (category is null) return null;

        category.CategoryName = dto.CategoryName;
        category.Description = dto.Description;
        category.Status = dto.Status ?? "Active";

        await _repo.SaveChangesAsync();
        return MapToDto(category);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _repo.GetByIdAsync(id);
        if (category is null) return false;

        _repo.Delete(category);
        return await _repo.SaveChangesAsync();
    }

    private static CategoryOutDto MapToDto(Category c) =>
        new(c.CategoryId, c.CategoryName, c.Description, c.Status, c.CreatedAt);
}