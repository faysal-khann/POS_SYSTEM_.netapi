using Microsoft.EntityFrameworkCore;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;
using POS_SYSTEM.Infrastructure.Persistence;

namespace POS_SYSTEM.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly AppDbContext _context;

    public ExpenseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Expense?> GetByIdAsync(int id)
        => await _context.Expenses.FindAsync(id);

    public async Task<Expense> AddAsync(Expense expense)
    {
        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();
        return expense;
    }

    public async Task<bool> DeleteAsync(Expense expense)
    {
        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Expense>> GetFilteredAsync(DateOnly? dateFrom, DateOnly? dateTo, int? categoryId, string? paymentMethod)
    {
        var query = _context.Expenses
            .Include(e => e.Category)
            .Include(e => e.CreatedByNavigation)
            .AsQueryable();

        if (dateFrom.HasValue) query = query.Where(e => e.ExpenseDate >= dateFrom.Value);
        if (dateTo.HasValue) query = query.Where(e => e.ExpenseDate <= dateTo.Value);
        if (categoryId.HasValue) query = query.Where(e => e.CategoryId == categoryId.Value);
        if (!string.IsNullOrEmpty(paymentMethod) && paymentMethod != "All")
            query = query.Where(e => e.PaymentMethod == paymentMethod);

        return await query.OrderByDescending(e => e.ExpenseDate).ToListAsync();
    }

    public async Task<int> CountInMonthAsync(DateOnly date)
    {
        var monthStart = new DateOnly(date.Year, date.Month, 1);
        var monthEnd = monthStart.AddMonths(1);

        return await _context.Expenses
            .Where(e => e.ExpenseDate >= monthStart && e.ExpenseDate < monthEnd)
            .CountAsync();
    }

    public async Task<List<(string CategoryName, decimal Total)>> GetTotalsByCategoryAsync(DateOnly? dateFrom, DateOnly? dateTo)
    {
        var query = _context.Expenses
            .Include(e => e.Category)
            .AsQueryable();

        if (dateFrom.HasValue) query = query.Where(e => e.ExpenseDate >= dateFrom.Value);
        if (dateTo.HasValue) query = query.Where(e => e.ExpenseDate <= dateTo.Value);

        var grouped = await query
            .GroupBy(e => e.Category.CategoryName)
            .Select(g => new { CategoryName = g.Key, Total = g.Sum(e => e.Amount) })
            .ToListAsync();

        return grouped.Select(g => (g.CategoryName, g.Total)).ToList();
    }

    public async Task<List<ExpenseCategory>> GetActiveCategoriesAsync()
        => await _context.ExpenseCategories.Where(c => c.Status == "Active").ToListAsync();

    public async Task<ExpenseCategory?> GetCategoryByNameAsync(string name)
        => await _context.ExpenseCategories.FirstOrDefaultAsync(c => c.CategoryName == name);

    public async Task<ExpenseCategory> AddCategoryAsync(ExpenseCategory category)
    {
        _context.ExpenseCategories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }
}