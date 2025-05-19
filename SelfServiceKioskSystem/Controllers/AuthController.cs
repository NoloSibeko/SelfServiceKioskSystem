using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfServiceKioskSystem.Data;
using SelfServiceKioskSystem.DTOs;
using SelfServiceKioskSystem.Helpers;
using SelfServiceKioskSystem.Models;
using SelfServiceKioskSystem.Models.DTOs;
using System.Threading.Tasks;

namespace SelfServiceKioskSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtHelper _jwtHelper;

        public AuthController(ApplicationDbContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return Conflict("User already exists");

            var wallet = new Wallet { Balance = 0 };
            var cart = new Cart(); // Initialize an empty cart

            var user = new User
            {
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                ContactNumber = dto.ContactNumber,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                AccountStatus = dto.AccountStatus,
                RoleID = 1, // Default to User
                Wallet = wallet,
                Cart = cart
            };


            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            user.Role = await _context.Roles.FindAsync(user.RoleID);
            var token = _jwtHelper.GenerateToken(user);

            return Ok(new
            {
                token,
                user = new
                {
                    user.UserID,
                    user.Email,
                    user.Name,
                    user.Surname,
                    Role = user.Role.UserRole,
                    WalletBalance = user.Wallet?.Balance ?? 0
                }
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO dto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Wallet)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return Unauthorized(new { message = "Invalid credentials." });

            var token = _jwtHelper.GenerateToken(user);

            return Ok(new
            {
                token,
                user = new
                {
                    user.UserID,
                    user.Email,
                    user.Name,
                    user.Surname,
                    Role = user.Role.UserRole,
                    WalletBalance = user.Wallet?.Balance ?? 0
                }
            });
        }
    }
}
