using Microsoft.AspNetCore.Mvc;
using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;

namespace POS_SYSTEM.Api.Controllers
{
    [ApiController]
    [Route("suppliers")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _service;

        public SuppliersController(ISupplierService service) => _service = service;

        [HttpGet("next-code")]
        public async Task<IActionResult> GetNextCode() =>
            Ok(new { SupplierCode = await _service.GetNextSupplierCodeAsync() });

        [HttpGet]
        public async Task<ActionResult<List<SupplierOutDto>>> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SupplierOutDto>> GetById(int id)
        {
            var supplier = await _service.GetByIdAsync(id);
            return supplier is null ? NotFound(new { detail = "Supplier not found" }) : Ok(supplier);
        }

        [HttpPost]
        public async Task<ActionResult<SupplierOutDto>> Create(SupplierCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.SupplierId }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<SupplierOutDto>> Update(int id, SupplierUpdateDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated is null ? NotFound(new { detail = "Supplier not found" }) : Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? Ok(new { message = "Supplier deleted successfully" }) : NotFound(new { detail = "Supplier not found" });
        }
    }
}
