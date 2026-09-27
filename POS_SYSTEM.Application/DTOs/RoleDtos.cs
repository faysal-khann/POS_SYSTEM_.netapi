namespace POS_SYSTEM.Application.DTOs;

public record RoleListItemDto(int RoleId, string RoleName, string? Description, int UserCount, string Status);

public record RoleCreateDto(string RoleName, string? Description, string? Status, List<int> PermissionIds);

// Mirrors FastAPI: RoleUpdate — same shape as create
public record RoleUpdateDto(string RoleName, string? Description, string? Status, List<int> PermissionIds);

public record RoleOutDto(int RoleId, string RoleName, string? Description, string Status);

public record RoleDetailDto(int RoleId, string RoleName, string? Description, string Status, List<int> PermissionIds);