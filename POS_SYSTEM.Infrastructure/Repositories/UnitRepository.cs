using Microsoft.EntityFrameworkCore;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using POS_SYSTEM.Infrastructure.Persistence;

namespace POS_SYSTEM.Infrastructure.Repositories;

public class UnitRepository : IUnitRepository
{
    private readonly AppDbContext _context;

    public UnitRepository(AppDbContext context) => _context = context;

    public async Task<List<Unit>> GetAllAsync() =>
        await _context.Units.AsNoTracking().ToListAsync();

    public async Task<Unit?> GetByIdAsync(int id) =>
        await _context.Units.FirstOrDefaultAsync(u => u.UnitId == id);

    public async Task AddAsync(Unit unit) => await _context.Units.AddAsync(unit);
    public void Delete(Unit unit) => _context.Units.Remove(unit);
    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() >= 0;
}