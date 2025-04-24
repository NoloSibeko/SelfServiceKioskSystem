using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SelfServiceKioskSystem.Data;
using SelfServiceKioskSystem.DTOs;
using SelfServiceKioskSystem.Models;
using SelfServiceKioskSystem.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SelfServiceKioskSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly JwtHelper _jwtHelper;

        public AuthController(ApplicationDbContext context, IConfiguration configuration, JwtHelper jwtHelper)
        {
            _context = context;
            _configuration = configuration;
            _jwtHelper = jwtHelper;
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO dto)
        {
            // Validation
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if user exists
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return Conflict("User already exists");

            // Create wallet
            var wallet = new Wallet { Balance = 0 };

            // Create user
            var user = new User
            {
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                ContactNumber = dto.ContactNumber,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                AccountStatus = dto.AccountStatus,
                RoleID = 1, 
                Wallet = wallet
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Registration successful" });
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO dto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return Unauthorized(new { message = "Invalid credentials." });
            }

            var token = _jwtHelper.GenerateToken(user);

            return Ok(new
            {
                token,
                user = new
                {
                    user.UserID,
                    user.Email,
                    Role = user.Role.UserRole,                 
                    user.Name,
                    user.Surname,
                    WalletBalance = user.Wallet?.Balance ?? 0

                }
            });
           
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.UserRole)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
