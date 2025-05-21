using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfServiceKioskSystem.Models;
using SelfServiceKioskSystem.DTOs;
using System.Linq;
using System.Threading.Tasks;
using SelfServiceKioskSystem.Data;
using System.Collections.Generic;

namespace SelfServiceKioskSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/cart/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<CartDTO>> GetCart(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserID == userId);

            if (cart == null)
            {
                return NotFound("Cart not found");
            }

            var cartDto = new CartDTO
            {
                CartID = cart.CartID,
                UserID = cart.UserID,
                TotalAmount = cart.TotalAmount,
                Items = cart.CartItems.Select(ci => new CartItemDTO
                {
                    CartItemID = ci.CartItemID,
                    ProductID = ci.ProductID,
                    ProductName = ci.Product.Name,
                    Price = ci.Product.Price,
                    Quantity = ci.Quantity,
                    Subtotal = ci.Product.Price * ci.Quantity
                }).ToList()
            };

            return Ok(cartDto);
        }

        // POST: api/cart/add
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDTO addToCartDto)
        {
            // Validate product exists and has sufficient stock
            var product = await _context.Products.FindAsync(addToCartDto.ProductID);
            if (product == null)
            {
                return NotFound("Product not found");
            }

            if (product.Quantity < addToCartDto.Quantity)
            {
                return BadRequest("Insufficient stock");
            }

            // Get or create cart
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserID == addToCartDto.UserID);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserID = addToCartDto.UserID,
                    CartItems = new List<CartItem>(),
                    TotalAmount = 0
                };
                _context.Carts.Add(cart);
            }

            // Check if product already in cart
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductID == addToCartDto.ProductID);
            if (existingItem != null)
            {
                // Update existing item
                existingItem.Quantity += addToCartDto.Quantity;
            }
            else
            {
                // Add new item
                cart.CartItems.Add(new CartItem
                {
                    ProductID = addToCartDto.ProductID,
                    Quantity = addToCartDto.Quantity
                });
            }

            // Update product inventory
            product.Quantity -= addToCartDto.Quantity;

            // Recalculate cart total
            cart.TotalAmount = cart.CartItems.Sum(ci =>
                _context.Products.Find(ci.ProductID).Price * ci.Quantity);

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Item added to cart", CartId = cart.CartID });
        }

        // PUT: api/cart/update/{itemId}
        [HttpPut("update/{itemId}")]
        public async Task<IActionResult> UpdateCartItem(int itemId, [FromBody] UpdateCartItemDTO updateDto)
        {
            var cartItem = await _context.CartItems
                .Include(ci => ci.Product)
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.CartItemID == itemId);

            if (cartItem == null)
            {
                return NotFound("Item not found in cart");
            }

            var product = cartItem.Product;

            // Calculate quantity difference
            int quantityDifference = updateDto.Quantity - cartItem.Quantity;

            // Check if enough stock available
            if (product.Quantity < quantityDifference)
            {
                return BadRequest("Insufficient stock");
            }

            // Update quantities
            product.Quantity -= quantityDifference;
            cartItem.Quantity = updateDto.Quantity;

            // Update cart total
            cartItem.Cart.TotalAmount = cartItem.Cart.CartItems.Sum(ci =>
                _context.Products.Find(ci.ProductID).Price * ci.Quantity);

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Cart item updated" });
        }

        // DELETE: api/cart/remove/{itemId}
        [HttpDelete("remove/{itemId}")]
        public async Task<IActionResult> RemoveFromCart(int itemId)
        {
            var cartItem = await _context.CartItems
                .Include(ci => ci.Product)
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.CartItemID == itemId);

            if (cartItem == null)
            {
                return NotFound("Item not found in cart");
            }

            // Return product to inventory
            cartItem.Product.Quantity += cartItem.Quantity;

            // Update cart total
            cartItem.Cart.TotalAmount -= cartItem.Product.Price * cartItem.Quantity;

            // Remove item
            _context.CartItems.Remove(cartItem);

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Item removed from cart" });
        }

        // POST: api/cart/checkout/{userId}
        [HttpPost("checkout/{userId}")]
        public async Task<IActionResult> Checkout(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .Include(c => c.User)
                .ThenInclude(u => u.Wallet)
                .FirstOrDefaultAsync(c => c.UserID == userId);

            if (cart == null)
            {
                return NotFound("Cart not found");
            }

            // Check wallet balance
            if (cart.User.Wallet.Balance < cart.TotalAmount)
            {
                return BadRequest("Insufficient funds");
            }

            // Process payment
            cart.User.Wallet.Balance -= cart.TotalAmount;

            // Create transaction record
            var transaction = new TransactionDetail
            {
                UserID = userId,
                WalletID = cart.User.Wallet.WalletID,
                CartID = cart.CartID,
                TransactionDate = DateTime.UtcNow,
                Amount = cart.TotalAmount,
                Description = "Purchase checkout",
                ResultingBalance = cart.User.Wallet.Balance,
                
            };

            _context.TransactionDetails.Add(transaction);

            // Clear cart
            _context.CartItems.RemoveRange(cart.CartItems);
            cart.TotalAmount = 0;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Checkout successful",
                NewBalance = cart.User.Wallet.Balance,
                TransactionId = transaction.TransactionID
            });
        }
    }
}