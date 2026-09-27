using Microsoft.AspNetCore.Mvc;
using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;

namespace POS_SYSTEM.Api.Controllers;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    private readonly IWebHostEnvironment _env;

    public ProductsController(IProductService service, IWebHostEnvironment env)
    {
        _service = service;
        _env = env;
    }

    [HttpGet("categories")]
    public async Task<ActionResult<List<LookupDto>>> GetCategories() =>
        Ok(await _service.GetCategoriesAsync());

    [HttpGet("brands")]
    public async Task<ActionResult<List<LookupDto>>> GetBrands() =>
        Ok(await _service.GetBrandsAsync());

    [HttpGet("units")]
    public async Task<ActionResult<List<LookupDto>>> GetUnits() =>
        Ok(await _service.GetUnitsAsync());

    [HttpGet("next-code")]
    public async Task<IActionResult> GetNextCode() =>
        Ok(new { ProductCode = await _service.GetNextProductCodeAsync() });

    [HttpGet]
    public async Task<ActionResult<List<ProductOutDto>>> GetAll() =>
        Ok(await _service.GetAllAsync());

    [HttpGet("by-barcode/{barcode}")]
    public async Task<ActionResult<ProductOutDto>> GetByBarcode(string barcode)
    {
        var product = await _service.GetByBarcodeAsync(barcode);
        return product is null ? NotFound(new { detail = "Product not found for this barcode" }) : Ok(product);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductOutDto>> GetById(int id)
    {
        var product = await _service.GetByIdAsync(id);
        return product is null ? NotFound(new { detail = "Product not found" }) : Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductOutDto>> Create(ProductCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.ProductId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductOutDto>> Update(int id, ProductUpdateDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated is null ? NotFound(new { detail = "Product not found" }) : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? Ok(new { message = "Product deleted successfully" }) : NotFound(new { detail = "Product not found" });
    }

    [HttpPost("bulk-price-update")]
    public async Task<IActionResult> BulkPriceUpdate(BulkPriceUpdateRequestDto dto)
    {
        try
        {
            return Ok(await _service.BulkPriceUpdateAsync(dto));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { detail = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { detail = ex.Message });
        }
    }

    [HttpPut("{id:int}/price")]
    public async Task<IActionResult> UpdatePrice(int id, UpdatePriceDto dto)
    {
        try
        {
            var updated = await _service.UpdatePriceAsync(id, dto.SalePrice);
            if (updated is null) return NotFound(new { detail = "Product not found" });

            return Ok(new { message = "Price updated successfully", ProductId = updated.ProductId, updated.SalePrice });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { detail = ex.Message });
        }
    }

    [HttpPost("upload-image")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest(new { detail = "Only JPG, PNG, or WEBP images are allowed" });

        var ext = Path.GetExtension(file.FileName);
        var filename = $"{Guid.NewGuid():N}{ext}";
        var uploadDir = Path.Combine(_env.ContentRootPath, "uploads", "products");
        Directory.CreateDirectory(uploadDir);

        var filepath = Path.Combine(uploadDir, filename);
        using (var stream = new FileStream(filepath, FileMode.Create))
            await file.CopyToAsync(stream);

        return Ok(new { url = $"/uploads/products/{filename}" });
    }
}