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
    DateOnly ExpenseDate, int CategoryID, decimal Amount, string PaymentMethod,
    string? ReferenceNo, int? SupplierID, string? Description, string? Note,
    bool IsRecurring, string Status
);

public record ExpenseFilterDto(DateOnly? date_from, DateOnly? date_to, int? category_id, string? payment_method);


//public record ExpenseFilterDto(
//    [FromQuery(Name = "date_from")] DateOnly? DateFrom,
//    [FromQuery(Name = "date_to")] DateOnly? DateTo,
//    [FromQuery(Name = "category_id")] int? CategoryId,
//    [FromQuery(Name = "payment_method")] string? PaymentMethod
//);
public record CategoryDto(int id, string name);
public record CreateCategoryDto(string CategoryName);