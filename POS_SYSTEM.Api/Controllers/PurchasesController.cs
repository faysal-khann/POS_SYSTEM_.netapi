using Microsoft.AspNetCore.Mvc;
using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;

namespace POS_SYSTEM.Api.Controllers
{
    [ApiController]
    [Route("purchases")]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchaseService _service;

        public PurchasesController(IPurchaseService service) => _service = service;

        // --- Static/lookup routes first ---

        [HttpGet("sizes")]
        public async Task<IActionResult> GetSizes() => Ok(await _service.GetSizesAsync());

        [HttpGet("branches")]
        public async Task<IActionResult> GetBranches() => Ok(await _service.GetBranchesAsync());

        [HttpPost("branches")]
        public async Task<IActionResult> CreateBranch(BranchCreateDto dto) =>
            Ok(await _service.CreateBranchAsync(dto));

        [HttpGet("next-number")]
        public async Task<IActionResult> PreviewNextNumber([FromQuery] DateOnly purchaseDate) =>
            Ok(new { purchase_no = await _service.PreviewNextNumberAsync(purchaseDate) });

        [HttpGet]
        public async Task<ActionResult<List<PurchaseListItemDto>>> GetAll(
            [FromQuery] DateOnly? dateFrom, [FromQuery] DateOnly? dateTo,
            [FromQuery] int? supplierId, [FromQuery] string? status) =>
            Ok(await _service.GetPurchasesAsync(dateFrom, dateTo, supplierId, status));

        [HttpPost]
        public async Task<ActionResult<PurchaseOutDto>> Create(PurchaseCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.PurchaseId }, created);
        }

        // --- Dynamic routes last ---

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PurchaseOutDto>> GetById(int id)
        {
            var purchase = await _service.GetByIdAsync(id);
            return purchase is null ? NotFound(new { detail = "Purchase not found" }) : Ok(purchase);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? Ok(new { message = "Purchase deleted successfully" }) : NotFound(new { detail = "Purchase not found" });
        }
    }
}
