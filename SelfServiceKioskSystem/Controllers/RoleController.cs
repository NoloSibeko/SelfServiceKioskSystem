using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfServiceKioskSystem.Data;
using SelfServiceKioskSystem.DTOs;
using SelfServiceKioskSystem.Models;
using SelfServiceKioskSystem.Models.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SelfServiceKioskSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoleController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Superuser")]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetRoles()
        {
            var roles = await _context.Roles
                .Select(r => new RoleDTO
                {
                    RoleID = r.RoleID,
                    UserRole = r.UserRole
                })
                .ToListAsync();

            return Ok(roles);
        }

        [HttpPost("assign")]
        [Authorize(Roles = "Superuser")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] RoleDTO roleDto)
        {
            if (roleDto == null || roleDto.RoleID == 0)
                return BadRequest("Invalid role data.");

            var user = await _context.Users.FindAsync(roleDto.UserID);
            if (user == null)
                return NotFound("User not found.");

            var role = await _context.Roles.FindAsync(roleDto.RoleID);
            if (role == null)
                return NotFound("Role not found.");

            if (user.RoleID == role.RoleID)
                return BadRequest("User already has this role.");

            user.RoleID = role.RoleID;
            await _context.SaveChangesAsync();

            return Ok("Role assigned successfully.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Superuser")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
                return NotFound();

            // Prevent deletion if any users are using this role
            bool hasUsers = await _context.Users.AnyAsync(u => u.RoleID == id);
            if (hasUsers)
                return BadRequest("Cannot delete a role assigned to users.");

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
