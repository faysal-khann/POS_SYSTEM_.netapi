namespace POS_SYSTEM.Application.DTOs;

public record ExpenseListItemDto(
    int ExpenseId, string ExpenseNo, DateOnly ExpenseDate, string CategoryName,
    string? Description, decimal Amount, string PaymentMethod, string? ReferenceNo,
    string Status, string CreatedByName
);

public record ExpenseSummaryDto(
    decimal TotalExpenses, int TotalTransactions, decimal AverageExpense,
    decimal HighestExpenseAmount, string? HighestExpenseCategory
);

public record ExpenseByCategoryDto(string CategoryName, decimal Amount, double Percent);

public record CreateExpenseDto(
    DateOnly ExpenseDate, int CategoryId, decimal Amount, string PaymentMethod,
    string? ReferenceNo, int? SupplierId, string? Description, string? Note,
    bool IsRecurring, string Status
);

public record ExpenseFilterDto(DateOnly? DateFrom, DateOnly? DateTo, int? CategoryId, string? PaymentMethod);

public record CategoryDto(int CategoryId, string CategoryName);
public record CreateCategoryDto(string CategoryName);