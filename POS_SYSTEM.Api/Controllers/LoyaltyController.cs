using Microsoft.AspNetCore.Mvc;
using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Exceptions;
using POS_SYSTEM.Application.Interfaces;

namespace POS_SYSTEM.Api.Controllers;

[ApiController]
[Route("loyalty")]
public class LoyaltyController : ControllerBase
{
    private readonly ILoyaltyService _service;

    public LoyaltyController(ILoyaltyService service) => _service = service;

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary() => Ok(await _service.GetSummaryAsync());

    [HttpGet("summary/{customerId:int}")]
    public async Task<IActionResult> GetCustomerPoints(int customerId) =>
        Ok(new { availablePoints = await _service.GetAvailablePointsAsync(customerId) });

    [HttpGet("history/{customerId:int}")]
    public async Task<IActionResult> GetHistory(int customerId) =>
        Ok(await _service.GetHistoryAsync(customerId));

    [HttpPost("adjust")]
    public async Task<IActionResult> Adjust(LoyaltyAdjustDto dto)
    {
        try
        {
            await _service.AdjustAsync(dto);
            return Ok(new { message = "Loyalty points adjusted successfully" });
        }
        catch (BadRequestAppException ex) { return BadRequest(new { detail = ex.Message }); }
    }
}