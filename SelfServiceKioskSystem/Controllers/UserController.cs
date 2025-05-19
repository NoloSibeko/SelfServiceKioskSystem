using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SelfServiceKioskSystem.Models;
using SelfServiceKioskSystem.Data;
using System.Linq;
using System.Threading.Tasks;
using SelfServiceKioskSystem.Attributes;
using Microsoft.EntityFrameworkCore;

namespace SelfServiceKioskSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Only Superusers can create, update, or delete users
        [HttpPost]
        [Authorize(Roles = "Superuser")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest("User data is invalid.");

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, user);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Superuser")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.UserID)
                return BadRequest("User ID mismatch.");

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
                return NotFound();

            existingUser.Name = user.Name;
            existingUser.Surname = user.Surname;
            existingUser.Email = user.Email;
            existingUser.ContactNumber = user.ContactNumber;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Superuser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Superuser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost("ensure-wallet-cart")]
        [Authorize(Roles = "Superuser")]
        public async Task<IActionResult> EnsureWalletsAndCartsForUsers()
        {
            var users = await _context.Users
                .Include(u => u.Wallet)
                .Include(u => u.Cart)
                .ToListAsync();

            foreach (var user in users)
            {
                if (user.Wallet == null)
                    user.Wallet = new Wallet { Balance = 0 };

                if (user.Cart == null)
                    user.Cart = new Cart();
            }

            await _context.SaveChangesAsync();
            return Ok("All users now have wallets and carts.");
        }


    }
}
