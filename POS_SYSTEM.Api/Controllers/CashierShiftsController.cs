using Microsoft.AspNetCore.Mvc;
using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;

namespace POS_SYSTEM.Api.Controllers;

[ApiController]
[Route("cashier-shifts")]
public class CashierShiftsController : ControllerBase
{
    private readonly ICashierShiftService _service;
    public CashierShiftsController(ICashierShiftService service) => _service = service;

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(
        [FromQuery(Name = "user_id")] int userId,
        [FromQuery(Name = "branch_id")] int branchId,
        [FromQuery] DateTime start,
        [FromQuery] DateTime end)
    {
        try
        {
            return Ok(await _service.GetSummaryAsync(userId, branchId, start, end));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { detail = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CloseShift(CreateCashierShiftDto dto)
    {
        try
        {
            return Ok(await _service.CloseShiftAsync(dto));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { detail = ex.Message });
        }
    }
}