using Microsoft.AspNetCore.Mvc;
using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;

namespace POS_SYSTEM.Api.Controllers;

[ApiController]
[Route("units")]
public class UnitsController : ControllerBase
{
    private readonly IUnitService _service;

    public UnitsController(IUnitService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<UnitOutDto>>> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var unit = await _service.GetByIdAsync(id);
        return unit is null ? NotFound(new { detail = "Unit not found" }) : Ok(unit);
    }

    [HttpPost]
    public async Task<ActionResult<UnitOutDto>> Create(UnitCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.UnitId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UnitUpdateDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated is null ? NotFound(new { detail = "Unit not found" }) : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? Ok(new { message = "Unit deleted successfully" }) : NotFound(new { detail = "Unit not found" });
    }
}