using Microsoft.AspNetCore.Mvc;
using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Exceptions;
using POS_SYSTEM.Application.Interfaces;

namespace POS_SYSTEM.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service) => _service = service;

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            try
            {
                return Ok(await _service.LoginAsync(dto));
            }
            catch (UnauthorizedAppException ex) { return Unauthorized(new { detail = ex.Message }); }
            catch (ForbiddenAppException ex) { return StatusCode(403, new { detail = ex.Message }); }
            catch (NotFoundAppException ex) { return NotFound(new { detail = ex.Message }); }
        }

        [HttpGet("lookup-company")]
        public async Task<IActionResult> LookupCompany([FromQuery] string identifier)
        {
            try
            {
                return Ok(await _service.LookupCompanyAsync(identifier));
            }
            catch (NotFoundAppException ex) { return NotFound(new { detail = ex.Message }); }
        }

        [HttpPost("verify-credentials")]
        public async Task<IActionResult> VerifyCredentials(VerifyCredentialsRequestDto dto)
        {
            try
            {
                return Ok(await _service.VerifyCredentialsAsync(dto));
            }
            catch (UnauthorizedAppException ex) { return Unauthorized(new { detail = ex.Message }); }
            catch (ForbiddenAppException ex) { return StatusCode(403, new { detail = ex.Message }); }
            catch (NotFoundAppException ex) { return NotFound(new { detail = ex.Message }); }
        }
    }
}
