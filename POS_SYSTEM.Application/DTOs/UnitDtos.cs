namespace POS_SYSTEM.Application.DTOs;

public record UnitCreateDto(string UnitName, string? ShortName, string? Description, string? Status);

// Mirrors FastAPI: UnitUpdate(UnitBase) — same shape as create
public record UnitUpdateDto(string UnitName, string? ShortName, string? Description, string? Status);

public record UnitOutDto(int UnitId, string UnitName, string? ShortName, string? Description, string? Status, DateTime? CreatedAt);