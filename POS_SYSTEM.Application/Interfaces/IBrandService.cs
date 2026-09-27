using POS_SYSTEM.Application.DTOs;

namespace POS_SYSTEM.Application.Interfaces;

public interface IBrandService
{
    Task<List<BrandOutDto>> GetAllAsync();
    Task<BrandOutDto?> GetByIdAsync(int id);
    Task<BrandOutDto> CreateAsync(BrandCreateDto dto);
    Task<BrandOutDto?> UpdateAsync(int id, BrandUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}