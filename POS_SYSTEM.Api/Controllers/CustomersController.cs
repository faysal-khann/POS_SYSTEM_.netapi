using Microsoft.AspNetCore.Mvc;
using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;

namespace POS_SYSTEM.Api.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomersController(ICustomerService service) => _service = service;

        [HttpGet("next-code")]
        public async Task<IActionResult> GetNextCode() =>
            Ok(new { CustomerCode = await _service.GetNextCustomerCodeAsync() });

        [HttpGet]
        public async Task<ActionResult<List<CustomerOutDto>>> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CustomerOutDto>> GetById(int id)
        {
            var customer = await _service.GetByIdAsync(id);
            return customer is null ? NotFound(new { detail = "Customer not found" }) : Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerOutDto>> Create(CustomerCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.CustomerId }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CustomerOutDto>> Update(int id, CustomerUpdateDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated is null ? NotFound(new { detail = "Customer not found" }) : Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? Ok(new { message = "Customer deleted successfully" }) : NotFound(new { detail = "Customer not found" });
        }
    }
}
