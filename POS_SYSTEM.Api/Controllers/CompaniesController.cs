using Microsoft.AspNetCore.Mvc;
using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Interfaces;

namespace POS_SYSTEM.Api.Controllers
{
    [ApiController]
    [Route("companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _service;
        private readonly IWebHostEnvironment _env;

        public CompaniesController(ICompanyService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult<List<CompanyListItemDto>>> GetAll([FromQuery] string? search) =>
            Ok(await _service.GetCompaniesAsync(search));

        [HttpPost("upload-logo")]
        public async Task<IActionResult> UploadLogo(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName);
            var filename = $"{Guid.NewGuid():N}{ext}";
            var uploadDir = Path.Combine(_env.ContentRootPath, "uploads", "companies");
            Directory.CreateDirectory(uploadDir);

            var filepath = Path.Combine(uploadDir, filename);
            using (var stream = new FileStream(filepath, FileMode.Create))
                await file.CopyToAsync(stream);

            return Ok(new LogoUploadResultDto($"/uploads/companies/{filename}"));
        }

        [HttpGet("{company_id:int}")]
        public async Task<IActionResult> GetDetail(int company_id)
        {
            var detail = await _service.GetDetailAsync(company_id);
            return detail is null ? NotFound(new { detail = "Company not found" }) : Ok(detail);
        }

        [HttpPost]
        public async Task<ActionResult<CompanyOutDto>> Create(CompanyCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetDetail), new { id = created.CompanyId }, created);
        }

        [HttpPut("{company_id:int}")]
        public async Task<IActionResult> Update(int company_id, CompanyCreateDto dto)
        {
            var updated = await _service.UpdateAsync(company_id, dto);
            return updated is null ? NotFound(new { detail = "Company not found" }) : Ok(updated);
        }

        [HttpDelete("{company_id:int}")]
        public async Task<IActionResult> Delete(int company_id)
        {
            var success = await _service.DeleteAsync(company_id);
            return success ? Ok(new { message = "Company deleted successfully" }) : NotFound(new { detail = "Company not found" });
        }
    }
}
