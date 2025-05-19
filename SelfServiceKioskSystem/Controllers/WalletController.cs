using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfServiceKioskSystem.Data;
using SelfServiceKioskSystem.DTOs;
using SelfServiceKioskSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SelfServiceKioskSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WalletController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Wallet/add-funds
        [HttpPost("add-funds")]
        public async Task<IActionResult> AddFunds([FromBody] AddWalletFundsDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users
                .Include(u => u.Wallet)
                .FirstOrDefaultAsync(u => u.UserID == dto.UserId);

            if (user == null || user.Wallet == null)
                return NotFound("User or wallet not found.");

            if (dto.Amount <= 0)
                return BadRequest("Amount must be a positive number.");

            // Update wallet balance
            user.Wallet.Balance += dto.Amount;

            // Log transaction
            var transaction = new TransactionDetail
            {
                UserID = user.UserID,
                WalletID = user.Wallet.WalletID,
                CartID = 0, // No cart involved in top-up
                TransactionDate = DateTime.UtcNow,
                Amount = dto.Amount,
                Description = "Wallet Top-Up",
                ResultingBalance = user.Wallet.Balance
            };

            _context.TransactionDetails.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Funds added successfully.",
                newBalance = user.Wallet.Balance
            });
        }


        // GET: api/Wallet/balance/{userId}
        [HttpGet("balance/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetBalance(int userId)
        {
            var wallet = await _context.Users
                .Where(u => u.UserID == userId)
                .Select(u => u.Wallet)
                .FirstOrDefaultAsync();

            if (wallet == null)
                return NotFound("Wallet not found.");

            return Ok(new { balance = wallet.Balance });
        }
    }
}
