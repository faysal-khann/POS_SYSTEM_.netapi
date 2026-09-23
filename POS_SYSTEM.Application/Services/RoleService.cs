using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Exceptions;
using POS_SYSTEM.Application.Interfaces;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;

namespace POS_SYSTEM.Application.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _repo;

    public RoleService(IRoleRepository repo) => _repo = repo;

    public async Task<List<RoleListItemDto>> GetRolesAsync(string? search)
    {
        var roles = await _repo.SearchAsync(search);
        var result = new List<RoleListItemDto>();

        foreach (var r in roles)
        {
            var userCount = await _repo.GetUserCountForRoleAsync(r.RoleId);
            result.Add(new RoleListItemDto(r.RoleId, r.RoleName, r.Description, userCount, r.Status ?? "Active"));
        }

        return result;
    }

    public async Task<RoleDetailDto?> GetDetailAsync(int id)
    {
        var role = await _repo.GetByIdAsync(id);
        if (role is null) return null;

        var permissionIds = await _repo.GetPermissionIdsForRoleAsync(id);
        return new RoleDetailDto(role.RoleId, role.RoleName, role.Description, role.Status ?? "Active", permissionIds);
    }

    public async Task<RoleOutDto> CreateAsync(RoleCreateDto dto)
    {
        var existing = await _repo.GetByNameAsync(dto.RoleName);
        if (existing is not null)
            throw new BadRequestAppException("A role with this name already exists.");

        var role = new Role
        {
            RoleName = dto.RoleName,
            Description = dto.Description,
            Status = dto.Status ?? "Active"
        };

        await _repo.AddAsync(role);
        await _repo.SaveChangesAsync(); // populates role.RoleId

        await _repo.ReplaceRolePermissionsAsync(role.RoleId, dto.PermissionIds);
        await _repo.SaveChangesAsync();

        return new RoleOutDto(role.RoleId, role.RoleName, role.Description, role.Status ?? "Active");
    }

    public async Task<RoleOutDto?> UpdateAsync(int id, RoleUpdateDto dto)
    {
        var role = await _repo.GetByIdAsync(id);
        if (role is null) return null;

        var conflict = await _repo.GetByNameAsync(dto.RoleName, excludeRoleId: id);
        if (conflict is not null)
            throw new BadRequestAppException("A role with this name already exists.");

        role.RoleName = dto.RoleName;
        role.Description = dto.Description;
        role.Status = dto.Status ?? "Active";

        await _repo.ReplaceRolePermissionsAsync(id, dto.PermissionIds);
        await _repo.SaveChangesAsync();

        return new RoleOutDto(role.RoleId, role.RoleName, role.Description, role.Status ?? "Active");
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var role = await _repo.GetByIdAsync(id);
        if (role is null) return false;

        var inUse = await _repo.GetUserCountForRoleAsync(id);
        if (inUse > 0)
            throw new BadRequestAppException($"Cannot delete — {inUse} user(s) are assigned to this role.");

        _repo.Delete(role);
        return await _repo.SaveChangesAsync();
    }
}