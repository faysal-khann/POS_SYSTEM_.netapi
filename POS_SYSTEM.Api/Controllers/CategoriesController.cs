using Microsoft.AspNetCore.Mvc;
using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;

namespace POS_SYSTEM.Api.Controllers;

[ApiController]
[Route("categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoriesController(ICategoryService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<CategoryOutDto>>> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{category_id:int}")]
    public async Task<IActionResult> GetById(int category_id)
    {
        var category = await _service.GetByIdAsync(category_id);
        return category is null ? NotFound(new { detail = "Category not found" }) : Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryOutDto>> Create(CategoryCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.CategoryId }, created);
    }

    [HttpPut("{category_id:int}")]
    public async Task<IActionResult> Update(int category_id, CategoryUpdateDto dto)
    {
        var updated = await _service.UpdateAsync(category_id, dto);
        return updated is null ? NotFound(new { detail = "Category not found" }) : Ok(updated);
    }

    [HttpDelete("{category_id:int}")]
    public async Task<IActionResult> Delete(int category_id)
    {
        var success = await _service.DeleteAsync(category_id);
        return success ? Ok(new { message = "Category deleted successfully" }) : NotFound(new { detail = "Category not found" });
    }
}