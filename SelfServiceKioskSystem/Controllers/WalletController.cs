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
        // POST: api/Wallet/{userId}/AddFunds
        [HttpPost("{userId}/AddFunds")]
        public async Task<IActionResult> AddFunds([FromBody] AddWalletFundsDTO dto)
        {
            var user = await _context.Users.Include(u => u.Wallet)
                                           .FirstOrDefaultAsync(u => u.UserID == dto.UserId);
            if (user == null)
                return BadRequest("User not found.");

            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserID == user.UserID);
            if (cart == null)
                return BadRequest("Cart does not exist for this user.");

            // ADD the amount to the current balance
            user.Wallet.Balance += dto.Amount;

            var transaction = new TransactionDetail
            {
                UserID = user.UserID,
                WalletID = user.Wallet.WalletID,
                CartID = cart.CartID,
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
                userId = user.UserID,
                walletId = user.Wallet.WalletID,
                newBalance = user.Wallet.Balance,
                transactionId = transaction.TransactionID,
                timestamp = transaction.TransactionDate
            });
        }

        // GET: api/Wallet/balance/5
        [HttpGet("{userId}/Balance")]
        public async Task<IActionResult> GetBalance(int userId)
        {
            var wallet = await _context.Wallets
                .Where(w => w.UserID == userId)
                .FirstOrDefaultAsync();

            if (wallet == null)
                return NotFound("Wallet not found.");

            return Ok(new
            {
                userId = wallet.UserID,
                walletId = wallet.WalletID,
                balance = wallet.Balance
            });
        }
    }
}
