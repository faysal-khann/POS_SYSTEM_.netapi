using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;
using POS_SYSTEM.Domain.Entities;
using POS_SYSTEM.Domain.Interfaces;

namespace POS_SYSTEM.Application.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _repo;

    public ExpenseService(IExpenseRepository repo)
    {
        _repo = repo;
    }

    public async Task<string> PreviewNextExpenseNoAsync(DateOnly expenseDate)
        => await GenerateExpenseNoAsync(expenseDate);

    private async Task<string> GenerateExpenseNoAsync(DateOnly expenseDate)
    {
        var count = await _repo.CountInMonthAsync(expenseDate);
        return $"EXP-{expenseDate:yyyyMMdd}{count + 1:D4}";
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        var categories = await _repo.GetActiveCategoriesAsync();
        return categories.Select(c => new CategoryDto(c.CategoryId, c.CategoryName)).ToList();
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
    {
        var existing = await _repo.GetCategoryByNameAsync(dto.CategoryName);
        if (existing is not null)
            throw new InvalidOperationException("Category already exists.");

        var category = new ExpenseCategory { CategoryName = dto.CategoryName, Status = "Active" };
        var created = await _repo.AddCategoryAsync(category);
        return new CategoryDto(created.CategoryId, created.CategoryName);
    }

    public async Task<ExpenseSummaryDto> GetSummaryAsync(ExpenseFilterDto filter)
    {
        var expenses = await _repo.GetFilteredAsync(filter.DateFrom, filter.DateTo, filter.CategoryId, filter.PaymentMethod);

        var total = expenses.Sum(e => e.Amount);
        var count = expenses.Count;
        var avg = count > 0 ? total / count : 0;
        var highest = expenses.OrderByDescending(e => e.Amount).FirstOrDefault();

        return new ExpenseSummaryDto(total, count, avg, highest?.Amount ?? 0, highest?.Category?.CategoryName);
    }

    public async Task<List<ExpenseByCategoryDto>> GetByCategoryAsync(DateOnly? dateFrom, DateOnly? dateTo)
    {
        var totals = await _repo.GetTotalsByCategoryAsync(dateFrom, dateTo);
        var grandTotal = totals.Sum(t => t.Total);
        if (grandTotal == 0) grandTotal = 1;

        return totals.Select(t => new ExpenseByCategoryDto(
            t.CategoryName, t.Total, Math.Round((double)(t.Total / grandTotal) * 100, 1)
        )).ToList();
    }

    public async Task<List<ExpenseListItemDto>> GetExpensesAsync(ExpenseFilterDto filter)
    {
        var expenses = await _repo.GetFilteredAsync(filter.DateFrom, filter.DateTo, filter.CategoryId, filter.PaymentMethod);

        return expenses.Select(e => new ExpenseListItemDto(
            e.ExpenseId, e.ExpenseNo ?? "—", e.ExpenseDate,
            e.Category?.CategoryName ?? "—", e.Description, e.Amount,
            e.PaymentMethod, e.ReferenceNo, e.Status,
            e.CreatedByNavigation?.FullName ?? "—"
        )).ToList();
    }

    public async Task<(int ExpenseId, string ExpenseNo)> CreateAsync(CreateExpenseDto dto, int createdBy, int branchId, int companyId)
    {
        var expenseNo = await GenerateExpenseNoAsync(dto.ExpenseDate);

        var expense = new Expense
        {
            ExpenseNo = expenseNo,
            CompanyId = companyId,
            BranchId = branchId,
            ExpenseDate = dto.ExpenseDate,
            CategoryId = dto.CategoryId,
            Description = dto.Description,
            Amount = dto.Amount,
            PaymentMethod = dto.PaymentMethod,
            ReferenceNo = dto.ReferenceNo,
            SupplierId = dto.SupplierId,
            Note = dto.Note,
            IsRecurring = dto.IsRecurring,
            Status = string.IsNullOrEmpty(dto.Status) ? "Paid" : dto.Status,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repo.AddAsync(expense);
        return (created.ExpenseId, created.ExpenseNo!);
    }

    public async Task<bool> DeleteAsync(int expenseId)
    {
        var expense = await _repo.GetByIdAsync(expenseId);
        if (expense is null) return false;
        return await _repo.DeleteAsync(expense);
    }
}