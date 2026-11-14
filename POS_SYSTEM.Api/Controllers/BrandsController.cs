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

    [HttpGet("{brand_id:int}")]
    public async Task<IActionResult> GetById(int brand_id)
    {
        var brand = await _service.GetByIdAsync(brand_id);
        return brand is null ? NotFound(new { detail = "Brand not found" }) : Ok(brand);
    }

    [HttpPost]
    public async Task<ActionResult<BrandOutDto>> Create(BrandCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { brand_id = created.BrandId }, created);
    }

    [HttpPut("{brand_id:int}")]
    public async Task<IActionResult> Update(int brand_id, BrandUpdateDto dto)
    {
        var updated = await _service.UpdateAsync(brand_id, dto);
        return updated is null ? NotFound(new { detail = "Brand not found" }) : Ok(updated);
    }

    [HttpDelete("{brand_id:int}")]
    public async Task<IActionResult> Delete(int brand_id)
    {
        var success = await _service.DeleteAsync(brand_id);
        return success ? Ok(new { message = "Brand deleted successfully" }) : NotFound(new { detail = "Brand not found" });
    }
}