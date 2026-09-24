using POS_SYSTEM.Application.DTOs;

namespace POS_SYSTEM.Application.Interfaces;

public interface IExpenseService
{
    Task<string> PreviewNextExpenseNoAsync(DateOnly expenseDate);
    Task<List<CategoryDto>> GetCategoriesAsync();
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
    Task<ExpenseSummaryDto> GetSummaryAsync(ExpenseFilterDto filter);
    Task<List<ExpenseByCategoryDto>> GetByCategoryAsync(DateOnly? dateFrom, DateOnly? dateTo);
    Task<List<ExpenseListItemDto>> GetExpensesAsync(ExpenseFilterDto filter);
    Task<(int ExpenseId, string ExpenseNo)> CreateAsync(CreateExpenseDto dto, int createdBy, int branchId, int companyId);
    Task<bool> DeleteAsync(int expenseId);
}