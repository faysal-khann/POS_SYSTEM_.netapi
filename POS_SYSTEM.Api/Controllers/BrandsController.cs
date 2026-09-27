using Microsoft.AspNetCore.Mvc;
using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;

namespace POS_SYSTEM.Api.Controllers;

[ApiController]
[Route("brands")]
public class BrandsController : ControllerBase
{
    private readonly IBrandService _service;

    public BrandsController(IBrandService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<BrandOutDto>>> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var brand = await _service.GetByIdAsync(id);
        return brand is null ? NotFound(new { detail = "Brand not found" }) : Ok(brand);
    }

    [HttpPost]
    public async Task<ActionResult<BrandOutDto>> Create(BrandCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.BrandId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, BrandUpdateDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated is null ? NotFound(new { detail = "Brand not found" }) : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? Ok(new { message = "Brand deleted successfully" }) : NotFound(new { detail = "Brand not found" });
    }
}