using loginAPP.Business;
using loginAPP.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace loginAPP.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userService.GetUserAsync());
        }

        [HttpGet("Roles")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _userService.GetRoleAsync());
        }

        [HttpPost("roles/add")]
        public async Task<IActionResult> AddRole([FromQuery] string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return BadRequest("Role name is required");

            var result = await _userService.AddRoleAsync(roleName);
            return result ? Ok() : BadRequest("Role already exists");
        }

        [HttpDelete("roles/delete/{id}")]
        public async Task<IActionResult> DeleteRole( string roleId)
        {
            var result = await  _userService.DeleteRoleAsync(roleId);
            return result ? Ok() : BadRequest("role not found");
        }

        [HttpDelete("Users/delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            return result ? Ok() : BadRequest("user not found");
        }


        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto model )
        {
            var result = await _userService.AssignRoleToUserAsync(model.UserId, model.RoleName);
            return result ? Ok() : BadRequest("Failed to assign role");
        }

        [HttpPost("remove-role")]
        public async Task<IActionResult> RemoveRole([FromBody] AssignRoleDto model)
        {
            var result = await _userService.RemoveRoleFromUserAsync(model.UserId, model.RoleName);
            return result ? Ok() : BadRequest("Failed to remove role");
        }


        

    }
}
