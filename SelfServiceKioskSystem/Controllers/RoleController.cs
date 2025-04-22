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

/*namespace SelfServiceKioskSystem.Controllers
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
                .Include(r => r.User)
                .Select(r => new RoleDTO
                {
                    RoleID = r.RoleID,
                    UserID = r.UserID,
                    UserEmail = r.User.Email,
                    UserRole = r.UserRole
                })
                .ToListAsync();

            return Ok(roles);
        }

        [HttpPost]
        [Authorize(Roles = "Superuser")] // Only Superusers can assign roles
        public async Task<IActionResult> AssignRole([FromBody] RoleDTO roleDto)
        {
            if (roleDto == null || string.IsNullOrWhiteSpace(roleDto.UserRole))
                return BadRequest("Invalid role data.");

            var allowedRoles = new[] { "User", "Superuser" };
            if (!allowedRoles.Contains(roleDto.UserRole))
                return BadRequest("Only 'User' or 'Superuser' roles are allowed.");

            var user = await _context.Users.FindAsync(roleDto.UserID);
            if (user == null)
                return NotFound("User not found.");

         /*   var existing = await _context.Roles
                .AnyAsync(r => r.UserID == roleDto.UserID && r.UserRole == roleDto.UserRole);*/

   /*         if (existing)
                return BadRequest("This user already has this role assigned.");

           /* var role = new Role
            {
                UserID = roleDto.UserID,
                UserRole = roleDto.UserRole
            };*/

         /*   _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return Ok("Role assigned successfully.");
        }


 /*       [HttpDelete("{id}")]
        [Authorize(Roles = "Superuser")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
                return NotFound();

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}*/
