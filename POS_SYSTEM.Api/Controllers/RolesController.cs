using Microsoft.AspNetCore.Mvc;
using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Exceptions;
using POS_SYSTEM.Application.Interfaces;

namespace POS_SYSTEM.Api.Controllers;

[ApiController]
[Route("roles")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _service;

    public RolesController(IRoleService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<RoleListItemDto>>> GetAll([FromQuery] string? search) =>
        Ok(await _service.GetRolesAsync(search));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDetail(int id)
    {
        var detail = await _service.GetDetailAsync(id);
        return detail is null ? NotFound(new { detail = "Role not found" }) : Ok(detail);
    }

    [HttpPost]
    public async Task<IActionResult> Create(RoleCreateDto dto)
    {
        try
        {
            return Ok(await _service.CreateAsync(dto));
        }
        catch (BadRequestAppException ex) { return BadRequest(new { detail = ex.Message }); }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, RoleUpdateDto dto)
    {
        try
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated is null ? NotFound(new { detail = "Role not found" }) : Ok(updated);
        }
        catch (BadRequestAppException ex) { return BadRequest(new { detail = ex.Message }); }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var success = await _service.DeleteAsync(id);
            return success ? Ok(new { message = "Role deleted successfully" }) : NotFound(new { detail = "Role not found" });
        }
        catch (BadRequestAppException ex) { return BadRequest(new { detail = ex.Message }); }
    }
}