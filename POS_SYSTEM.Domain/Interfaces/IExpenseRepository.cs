using POS_SYSTEM.Domain.Entities;

namespace POS_SYSTEM.Domain.Interfaces;

public interface IExpenseRepository
{
    Task<Expense?> GetByIdAsync(int id);
    Task<Expense> AddAsync(Expense expense);
    Task<bool> DeleteAsync(Expense expense);
    Task<List<Expense>> GetFilteredAsync(DateOnly? dateFrom, DateOnly? dateTo, int? categoryId, string? paymentMethod);
    Task<int> CountInMonthAsync(DateOnly date);
    Task<List<(string CategoryName, decimal Total)>> GetTotalsByCategoryAsync(DateOnly? dateFrom, DateOnly? dateTo);
    Task<List<ExpenseCategory>> GetActiveCategoriesAsync();
    Task<ExpenseCategory?> GetCategoryByNameAsync(string name);
    Task<ExpenseCategory> AddCategoryAsync(ExpenseCategory category);
}