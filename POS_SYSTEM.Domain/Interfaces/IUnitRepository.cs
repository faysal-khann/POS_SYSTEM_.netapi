using POS_SYSTEM.Domain.Entities;

namespace POS_SYSTEM.Domain.Interfaces;

public interface IUnitRepository
{
    Task<List<Unit>> GetAllAsync();
    Task<Unit?> GetByIdAsync(int id);
    Task AddAsync(Unit unit);
    void Delete(Unit unit);
    Task<bool> SaveChangesAsync();
}