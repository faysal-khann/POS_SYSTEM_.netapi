using Microsoft.AspNetCore.Mvc;
using POS_SYSTEM.Application.DTOs;
using POS_SYSTEM.Application.Exceptions;
using POS_SYSTEM.Application.Interfaces;

namespace POS_SYSTEM.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service) => _service = service;

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles() => Ok(await _service.GetRolesAsync());

        [HttpGet]
        public async Task<ActionResult<List<UserListItemDto>>> GetAll(
            [FromQuery] string? search, [FromQuery] int? roleId, [FromQuery] string? status) =>
            Ok(await _service.GetUsersAsync(search, roleId, status));

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? Ok(new { message = "User deleted successfully" }) : NotFound(new { detail = "User not found" });
        }

        [HttpGet("permissions/tree")]
        public async Task<IActionResult> GetPermissionTree() => Ok(await _service.GetPermissionTreeAsync());

        [HttpGet("roles/{roleId:int}/permissions")]
        public async Task<IActionResult> GetRolePermissions(int roleId) =>
            Ok(await _service.GetRolePermissionIdsAsync(roleId));

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDto dto)
        {
            try
            {
                return Ok(await _service.CreateAsync(dto));
            }
            catch (BadRequestAppException ex) { return BadRequest(new { detail = ex.Message }); }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var detail = await _service.GetUserDetailAsync(id);
            return detail is null ? NotFound(new { detail = "User not found" }) : Ok(detail);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UserUpdateDto dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                return updated is null ? NotFound(new { detail = "User not found" }) : Ok(updated);
            }
            catch (BadRequestAppException ex) { return BadRequest(new { detail = ex.Message }); }
        }
    }
}
