using POS_SYSTEM.Application.DTOs;

namespace POS_SYSTEM.Application.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryOutDto>> GetAllAsync();
    Task<CategoryOutDto?> GetByIdAsync(int id);
    Task<CategoryOutDto> CreateAsync(CategoryCreateDto dto);
    Task<CategoryOutDto?> UpdateAsync(int id, CategoryUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}