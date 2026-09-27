using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Application.DTOs
{
    public record UserListItemDto(
    int UserId,
    string FullName,
    string Username,
    string Email,
    string RoleName,
    string Status,
    DateTime? LastLoginAt
);

    public record PermissionNodeDto(int Id, string Name, List<PermissionNodeDto> Children);

    public record UserCreateDto(
        string FullName,
        string Username,
        string Email,
        string? Phone,
        string Password,
        string ConfirmPassword,
        int RoleId,
        int PrimaryBranchId,
        string? EmployeeId,
        string? Designation,
        string? Address,
        string? Notes,
        string? Status,
        List<int> PermissionIds
    );

    public record UserUpdateDto(
        string FullName,
        string Username,
        string Email,
        string? Phone,
        string? Password,
        string? ConfirmPassword,
        int RoleId,
        int PrimaryBranchId,
        string? EmployeeId,
        string? Designation,
        string? Address,
        string? Notes,
        string? Status,
        List<int> PermissionIds
    );

    public record UserOutDto(int UserId, string FullName, string Username, string Email);

    public record UserDetailDto(
        int UserId,
        string FullName,
        string Username,
        string Email,
        string? Phone,
        int RoleId,
        string RoleName,
        int PrimaryBranchId,
        string BranchName,
        string? EmployeeId,
        string? Designation,
        string? Address,
        string? Notes,
        string Status,
        DateTime? LastLoginAt,
        List<int> PermissionIds
    );

    public record RoleLookupDto(int Id, string Name);
}
