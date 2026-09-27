using System.Text.Json.Serialization;

namespace POS_SYSTEM.Application.DTOs;

public record PermissionListItemDto(
    int PermissionId,
    string PermissionName,
    string Module,
    string? Description,
    string Status
);

public record PermissionCreateDto(
    string PermissionName,
    int ParentPermissionId,
    string? Description,
    string? Status
);

public record ModuleCreateDto(string ModuleName);

// Exception: FastAPI's ModuleOut and the raw /modules list both use lowercase id/name.
public record ModuleOutDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name
);