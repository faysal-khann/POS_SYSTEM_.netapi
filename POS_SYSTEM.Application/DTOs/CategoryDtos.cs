namespace POS_SYSTEM.Application.DTOs;

public record CategoryCreateDto(string CategoryName, string? Description, string? Status);

// Mirrors FastAPI: CategoryUpdate(CategoryBase) — same shape as create
public record CategoryUpdateDto(string CategoryName, string? Description, string? Status);

public record CategoryOutDto(int CategoryId, string CategoryName, string? Description, string? Status, DateTime? CreatedAt);