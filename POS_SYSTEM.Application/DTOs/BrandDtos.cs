namespace POS_SYSTEM.Application.DTOs;

public record BrandCreateDto(string BrandName, string? Description, string? Status);

// Mirrors FastAPI: BrandUpdate(BrandBase) — same shape as create
public record BrandUpdateDto(string BrandName, string? Description, string? Status);

public record BrandOutDto(int BrandId, string BrandName, string? Description, string? Status, DateTime? CreatedAt);