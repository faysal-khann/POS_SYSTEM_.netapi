using Microsoft.AspNetCore.Mvc;
using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Exceptions;
using POS_SYSTEM.Application.Interfaces;

namespace POS_SYSTEM.Api.Controllers;

[ApiController]
[Route("sales")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _service;

    public SalesController(ISaleService service) => _service = service;

    [HttpGet("next-invoice-no")]
    public async Task<IActionResult> GetNextInvoiceNo() =>
        Ok(new { invoice_no = await _service.GetNextInvoiceNoAsync() });

    [HttpGet("cashiers-list")]
    public async Task<IActionResult> GetCashiersList() => Ok(await _service.GetCashiersListAsync());

    [HttpGet("cashiers")]
    public async Task<IActionResult> GetCashiers() => Ok(await _service.GetCashiersAsync());

    [HttpGet("held")]
    public async Task<IActionResult> GetHeld() => Ok(await _service.GetHeldSalesAsync());

    [HttpGet("held/{saleId:int}")]
    public async Task<IActionResult> GetHeldDetail(int saleId)
    {
        var detail = await _service.GetHeldSaleDetailAsync(saleId);
        return detail is null ? NotFound(new { detail = "Held sale not found" }) : Ok(detail);
    }

    [HttpGet("drafts")]
    public async Task<IActionResult> GetDrafts() => Ok(await _service.GetDraftSalesAsync());

    [HttpGet("drafts/{saleId:int}")]
    public async Task<IActionResult> GetDraftDetail(int saleId)
    {
        var detail = await _service.GetDraftSaleDetailAsync(saleId);
        return detail is null ? NotFound(new { detail = "Draft not found" }) : Ok(detail);
    }

    [HttpGet]
    public async Task<ActionResult<List<SaleListItemDto>>> GetAll(
        [FromQuery] DateOnly? dateFrom, [FromQuery] DateOnly? dateTo,
        [FromQuery] int? customerId, [FromQuery] int? cashierId, [FromQuery] string? paymentStatus) =>
        Ok(await _service.GetSalesAsync(dateFrom, dateTo, customerId, cashierId, paymentStatus));

    [HttpPost]
    public async Task<IActionResult> Create(SaleCreateDto dto)
    {
        try
        {
            return Ok(await _service.CreateAsync(dto));
        }
        catch (BadRequestAppException ex) { return BadRequest(new { detail = ex.Message }); }
    }

    [HttpDelete("{saleId:int}")]
    public async Task<IActionResult> Delete(int saleId)
    {
        var success = await _service.DeleteAsync(saleId);
        return success ? Ok(new { message = "Sale deleted successfully" }) : NotFound(new { detail = "Sale not found" });
    }
}