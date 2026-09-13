using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Exceptions;
using POS_SYSTEM.Application.Interfaces;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo) => _repo = repo;

        public async Task<List<UserListItemDto>> GetUsersAsync(string? search, int? roleId, string? status)
        {
            var users = await _repo.SearchAsync(search, roleId, status);
            return users.Select(u => new UserListItemDto(
                u.UserId, u.FullName, u.Username, u.Email,
                u.Role?.RoleName ?? "—", u.Status ?? "Active", u.LastLoginAt
            )).ToList();
        }

        public async Task<UserDetailDto?> GetUserDetailAsync(int id)
        {
            var user = await _repo.GetByIdWithRoleAndBranchAsync(id);
            if (user is null) return null;

            var permissionIds = await _repo.GetPermissionIdsForUserAsync(id);

            return new UserDetailDto(
                user.UserId, user.FullName, user.Username, user.Email, user.Phone,
                user.RoleId, user.Role?.RoleName ?? "—",
                user.PrimaryBranchId, user.PrimaryBranch?.BranchName ?? "—",
                user.EmployeeId, user.Designation, user.Address, user.Notes,
                user.Status ?? "Active", user.LastLoginAt, permissionIds
            );
        }

        public async Task<UserOutDto> CreateAsync(UserCreateDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                throw new BadRequestAppException("Passwords do not match");

            var existing = await _repo.FindByUsernameOrEmailAsync(dto.Username, dto.Email);
            if (existing is not null)
                throw new BadRequestAppException("Username or email already exists");

            var user = new User
            {
                FullName = dto.FullName,
                Username = dto.Username,
                Email = dto.Email,
                Phone = dto.Phone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId = dto.RoleId,
                PrimaryBranchId = dto.PrimaryBranchId,
                EmployeeId = dto.EmployeeId,
                Designation = dto.Designation,
                Address = dto.Address,
                Notes = dto.Notes,
                Status = dto.Status ?? "Active"
            };

            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync(); // EF populates user.UserId here after insert

            await _repo.ReplaceUserPermissionsAsync(user.UserId, dto.PermissionIds);
            await _repo.SaveChangesAsync();

            return new UserOutDto(user.UserId, user.FullName, user.Username, user.Email);
        }

        public async Task<UserOutDto?> UpdateAsync(int id, UserUpdateDto dto)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user is null) return null;

            var conflict = await _repo.FindByUsernameOrEmailAsync(dto.Username, dto.Email, excludeUserId: id);
            if (conflict is not null)
                throw new BadRequestAppException("Username or email already in use");

            if (!string.IsNullOrEmpty(dto.Password))
            {
                if (dto.Password != dto.ConfirmPassword)
                    throw new BadRequestAppException("Passwords do not match");
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }

            user.FullName = dto.FullName;
            user.Username = dto.Username;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.RoleId = dto.RoleId;
            user.PrimaryBranchId = dto.PrimaryBranchId;
            user.EmployeeId = dto.EmployeeId;
            user.Designation = dto.Designation;
            user.Address = dto.Address;
            user.Notes = dto.Notes;
            user.Status = dto.Status ?? "Active";

            await _repo.ReplaceUserPermissionsAsync(id, dto.PermissionIds);
            await _repo.SaveChangesAsync();

            return new UserOutDto(user.UserId, user.FullName, user.Username, user.Email);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user is null) return false;

            _repo.Delete(user);
            return await _repo.SaveChangesAsync();
        }

        public async Task<List<RoleLookupDto>> GetRolesAsync() =>
            (await _repo.GetRolesAsync()).Select(r => new RoleLookupDto(r.RoleId, r.RoleName)).ToList();

        public async Task<List<PermissionNodeDto>> GetPermissionTreeAsync()
        {
            var perms = await _repo.GetActivePermissionsOrderedAsync();
            var byParent = perms
                .GroupBy(p => p.ParentPermissionId)
                .ToDictionary(g => g.Key, g => g.ToList());

            List<PermissionNodeDto> Build(int? parentId)
            {
                if (!byParent.TryGetValue(parentId, out var children)) return new List<PermissionNodeDto>();
                return children
                    .Select(p => new PermissionNodeDto(p.PermissionId, p.PermissionName, Build(p.PermissionId)))
                    .ToList();
            }

            return Build(null);
        }

        public async Task<List<int>> GetRolePermissionIdsAsync(int roleId) =>
            await _repo.GetPermissionIdsForRoleAsync(roleId);
    }
}
