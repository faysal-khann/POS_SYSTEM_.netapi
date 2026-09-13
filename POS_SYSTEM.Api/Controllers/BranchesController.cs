using Microsoft.AspNetCore.Mvc;
using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;

namespace POS_SYSTEM.Api.Controllers;

[ApiController]
[Route("branches")]
public class BranchesController : ControllerBase
{
    private readonly IBranchService _service;

    public BranchesController(IBranchService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<BranchListItemDto>>> GetAll([FromQuery] string? search) =>
        Ok(await _service.GetBranchesAsync(search));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDetail(int id)
    {
        var detail = await _service.GetDetailAsync(id);
        return detail is null ? NotFound(new { detail = "Branch not found" }) : Ok(detail);
    }

    [HttpPost]
    public async Task<ActionResult<BranchDetailDto>> Create(BranchCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetDetail), new { id = created.BranchId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, BranchUpdateDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated is null ? NotFound(new { detail = "Branch not found" }) : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? Ok(new { message = "Branch deleted successfully" }) : NotFound(new { detail = "Branch not found" });
    }
}