using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;

namespace POS_SYSTEM.Application.Services;

public class UnitService : IUnitService
{
    private readonly IUnitRepository _repo;

    public UnitService(IUnitRepository repo) => _repo = repo;

    public async Task<List<UnitOutDto>> GetAllAsync()
    {
        var units = await _repo.GetAllAsync();
        return units.Select(MapToDto).ToList();
    }

    public async Task<UnitOutDto?> GetByIdAsync(int id)
    {
        var u = await _repo.GetByIdAsync(id);
        return u is null ? null : MapToDto(u);
    }

    public async Task<UnitOutDto> CreateAsync(UnitCreateDto dto)
    {
        var unit = new Unit
        {
            UnitName = dto.UnitName,
            ShortName = dto.ShortName,
            Description = dto.Description,
            Status = dto.Status ?? "Active",
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(unit);
        await _repo.SaveChangesAsync();
        return MapToDto(unit);
    }

    public async Task<UnitOutDto?> UpdateAsync(int id, UnitUpdateDto dto)
    {
        var unit = await _repo.GetByIdAsync(id);
        if (unit is null) return null;

        unit.UnitName = dto.UnitName;
        unit.ShortName = dto.ShortName;
        unit.Description = dto.Description;
        unit.Status = dto.Status ?? "Active";

        await _repo.SaveChangesAsync();
        return MapToDto(unit);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var unit = await _repo.GetByIdAsync(id);
        if (unit is null) return false;

        _repo.Delete(unit);
        return await _repo.SaveChangesAsync();
    }

    private static UnitOutDto MapToDto(Unit u) =>
        new(u.UnitId, u.UnitName, u.ShortName, u.Description, u.Status, u.CreatedAt);
}