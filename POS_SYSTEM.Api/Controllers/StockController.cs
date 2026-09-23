using Microsoft.AspNetCore.Mvc;
using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;

namespace POS_SYSTEM.Api.Controllers;

[ApiController]
[Route("stock")]
public class StockController : ControllerBase
{
    private readonly IStockService _service;

    public StockController(IStockService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<StockListItemDto>>> GetAll(
        [FromQuery] string? search, [FromQuery] int? categoryId,
        [FromQuery] int? brandId, [FromQuery] int? branchId) =>
        Ok(await _service.GetStockAsync(search, categoryId, brandId, branchId));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDetail(int id)
    {
        var detail = await _service.GetDetailAsync(id);
        return detail is null ? NotFound(new { detail = "Stock record not found" }) : Ok(detail);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, StockUpdateDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated is null ? NotFound(new { detail = "Stock record not found" }) : Ok(updated);
    }
}