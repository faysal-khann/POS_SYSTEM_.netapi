using POS_SYSTEM.Application.DTOs;

namespace POS_SYSTEM.Application.Interfaces;

public interface IUnitService
{
    Task<List<UnitOutDto>> GetAllAsync();
    Task<UnitOutDto?> GetByIdAsync(int id);
    Task<UnitOutDto> CreateAsync(UnitCreateDto dto);
    Task<UnitOutDto?> UpdateAsync(int id, UnitUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}