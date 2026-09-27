using Microsoft.AspNetCore.Mvc;
using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Exceptions;
using POS_SYSTEM.Application.Interfaces;

namespace POS_SYSTEM.Api.Controllers;

[ApiController]
[Route("permissions")]
public class PermissionsController : ControllerBase
{
    private readonly IPermissionService _service;

    public PermissionsController(IPermissionService service) => _service = service;

    [HttpGet("modules")]
    public async Task<IActionResult> GetModules() => Ok(await _service.GetModulesAsync());

    [HttpPost("modules")]
    public async Task<IActionResult> CreateModule(ModuleCreateDto dto)
    {
        try
        {
            return Ok(await _service.CreateModuleAsync(dto));
        }
        catch (BadRequestAppException ex) { return BadRequest(new { detail = ex.Message }); }
    }

    [HttpGet]
    public async Task<ActionResult<List<PermissionListItemDto>>> GetAll(
        [FromQuery] string? search, [FromQuery] string? module) =>
        Ok(await _service.GetPermissionsAsync(search, module));

    [HttpPost]
    public async Task<IActionResult> Create(PermissionCreateDto dto)
    {
        try
        {
            return Ok(await _service.CreateAsync(dto));
        }
        catch (NotFoundAppException ex) { return NotFound(new { detail = ex.Message }); }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? Ok(new { message = "Permission deleted successfully" }) : NotFound(new { detail = "Permission not found" });
    }
}