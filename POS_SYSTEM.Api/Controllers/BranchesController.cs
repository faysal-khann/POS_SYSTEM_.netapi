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

    [HttpGet("{branch_id:int}")]
    public async Task<IActionResult> GetDetail(int branch_id)
    {
        var detail = await _service.GetDetailAsync(branch_id);
        return detail is null ? NotFound(new { detail = "Branch not found" }) : Ok(detail);
    }

    [HttpPost]
    public async Task<ActionResult<BranchDetailDto>> Create(BranchCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetDetail), new { branch_id = created.BranchId }, created);
    }

    [HttpPut("{branch_id:int}")]
    public async Task<IActionResult> Update(int branch_id, BranchUpdateDto dto)
    {
        var updated = await _service.UpdateAsync(branch_id, dto);
        return updated is null ? NotFound(new { detail = "Branch not found" }) : Ok(updated);
    }

    [HttpDelete("{branch_id:int}")]
    public async Task<IActionResult> Delete(int branch_id)
    {
        var success = await _service.DeleteAsync(branch_id);
        return success ? Ok(new { message = "Branch deleted successfully" }) : NotFound(new { detail = "Branch not found" });
    }
}