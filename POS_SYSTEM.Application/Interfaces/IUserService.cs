using POS_SYSTEM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserListItemDto>> GetUsersAsync(string? search, int? roleId, string? status);
        Task<UserDetailDto?> GetUserDetailAsync(int id);
        Task<UserOutDto> CreateAsync(UserCreateDto dto);
        Task<UserOutDto?> UpdateAsync(int id, UserUpdateDto dto);
        Task<bool> DeleteAsync(int id);

        Task<List<RoleLookupDto>> GetRolesAsync();
        Task<List<PermissionNodeDto>> GetPermissionTreeAsync();
        Task<List<int>> GetRolePermissionIdsAsync(int roleId);
    }
}
