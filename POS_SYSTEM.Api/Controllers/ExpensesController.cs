using Microsoft.AspNetCore.Mvc;
using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;

namespace POS_SYSTEM.Api.Controllers;

[ApiController]
[Route("api/expenses")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _service;
    public ExpensesController(IExpenseService service) => _service = service;

    [HttpGet("next-number")]
    public async Task<IActionResult> PreviewNextNumber([FromQuery] DateOnly expenseDate)
        => Ok(new { expense_no = await _service.PreviewNextExpenseNoAsync(expenseDate) });

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories() => Ok(await _service.GetCategoriesAsync());

    [HttpPost("categories")]
    public async Task<IActionResult> CreateCategory(CreateCategoryDto dto)
    {
        try { return Ok(await _service.CreateCategoryAsync(dto)); }
        catch (InvalidOperationException ex) { return BadRequest(new { detail = ex.Message }); }
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary([FromQuery] ExpenseFilterDto filter)
        => Ok(await _service.GetSummaryAsync(filter));

    [HttpGet("by-category")]
    public async Task<IActionResult> GetByCategory([FromQuery] DateOnly? dateFrom, [FromQuery] DateOnly? dateTo)
        => Ok(await _service.GetByCategoryAsync(dateFrom, dateTo));

    [HttpGet]
    public async Task<IActionResult> GetExpenses([FromQuery] ExpenseFilterDto filter)
        => Ok(await _service.GetExpensesAsync(filter));

    [HttpPost]
    public async Task<IActionResult> Create(CreateExpenseDto dto, [FromQuery] int createdBy, [FromQuery] int branchId, [FromQuery] int companyId)
    {
        var (id, no) = await _service.CreateAsync(dto, createdBy, branchId, companyId);
        return Ok(new { ExpenseID = id, ExpenseNo = no });
    }

    [HttpDelete("{expenseId}")]
    public async Task<IActionResult> Delete(int expenseId)
        => await _service.DeleteAsync(expenseId)
            ? Ok(new { message = "Expense deleted successfully" })
            : NotFound(new { detail = "Expense not found" });
}