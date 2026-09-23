using POS_SYSTEM.Application.DTOs;

namespace POS_SYSTEM.Application.Interfaces;

public interface IRoleService
{
    Task<List<RoleListItemDto>> GetRolesAsync(string? search);
    Task<RoleDetailDto?> GetDetailAsync(int id);
    Task<RoleOutDto> CreateAsync(RoleCreateDto dto);
    Task<RoleOutDto?> UpdateAsync(int id, RoleUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}