using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Exceptions;
using POS_SYSTEM.Application.Interfaces;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;

namespace POS_SYSTEM.Application.Services;

public class PermissionService : IPermissionService
{
    private readonly IPermissionRepository _repo;

    public PermissionService(IPermissionRepository repo) => _repo = repo;

    public async Task<List<ModuleOutDto>> GetModulesAsync()
    {
        var modules = await _repo.GetModulesAsync();
        return modules.Select(m => new ModuleOutDto(m.PermissionId, m.PermissionName)).ToList();
    }

    public async Task<List<PermissionListItemDto>> GetPermissionsAsync(string? search, string? module)
    {
        var permissions = await _repo.SearchChildPermissionsAsync(search, module);
        return permissions.Select(p => new PermissionListItemDto(
            p.PermissionId, p.PermissionName, p.Module, p.Description, p.Status ?? "Active"
        )).ToList();
    }

    public async Task<PermissionListItemDto> CreateAsync(PermissionCreateDto dto)
    {
        var parent = await _repo.GetByIdAsync(dto.ParentPermissionId)
            ?? throw new NotFoundAppException("Parent module not found");

        var keyBase = dto.PermissionName.ToLower().Replace(" ", "_").Replace("/", "_");

        var permission = new Permission
        {
            ParentPermissionId = dto.ParentPermissionId,
            PermissionKey = $"{parent.PermissionKey}.{keyBase}",
            PermissionName = dto.PermissionName,
            Description = dto.Description,
            Module = parent.Module,
            Status = dto.Status ?? "Active"
        };

        await _repo.AddAsync(permission);
        await _repo.SaveChangesAsync();

        return new PermissionListItemDto(
            permission.PermissionId, permission.PermissionName, permission.Module,
            permission.Description, permission.Status ?? "Active"
        );
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var permission = await _repo.GetByIdAsync(id);
        if (permission is null) return false;

        _repo.Delete(permission);
        return await _repo.SaveChangesAsync();
    }

    public async Task<ModuleOutDto> CreateModuleAsync(ModuleCreateDto dto)
    {
        var existing = await _repo.GetModuleByNameAsync(dto.ModuleName);
        if (existing is not null)
            throw new BadRequestAppException("A module with this name already exists.");

        var key = dto.ModuleName.ToLower().Replace(" ", "_");

        var module = new Permission
        {
            ParentPermissionId = null,
            PermissionKey = key,
            PermissionName = dto.ModuleName,
            Module = dto.ModuleName,
            Status = "Active"
        };

        await _repo.AddAsync(module);
        await _repo.SaveChangesAsync();

        return new ModuleOutDto(module.PermissionId, module.PermissionName);
    }
}