using Microsoft.AspNetCore.Mvc;
using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;

namespace POS_SYSTEM.Api.Controllers;

[ApiController]
[Route("sale-returns")]
public class SaleReturnsController : ControllerBase
{
    private readonly ISaleReturnService _saleReturnService;

    public SaleReturnsController(ISaleReturnService saleReturnService)
    {
        _saleReturnService = saleReturnService;
    }

    [HttpGet("lookup/{invoice_no}")]
    public async Task<ActionResult<SaleLookupResultDto>> LookupSale(string invoice_no)
    {
        try
        {
            var result = await _saleReturnService.LookupSaleAsync(invoice_no);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<SaleReturnOutDto>> CreateSaleReturn([FromBody] SaleReturnCreateDto payload)
    {
        try
        {
            var result = await _saleReturnService.CreateSaleReturnAsync(payload);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}