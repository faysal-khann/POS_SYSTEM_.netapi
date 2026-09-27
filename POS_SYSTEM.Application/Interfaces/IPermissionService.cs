using POS_SYSTEM.Application.DTOs;

namespace POS_SYSTEM.Application.Interfaces;

public interface IPermissionService
{
    Task<List<ModuleOutDto>> GetModulesAsync();
    Task<List<PermissionListItemDto>> GetPermissionsAsync(string? search, string? module);
    Task<PermissionListItemDto> CreateAsync(PermissionCreateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<ModuleOutDto> CreateModuleAsync(ModuleCreateDto dto);
}