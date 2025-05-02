using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfServiceKioskSystem.Data;
using SelfServiceKioskSystem.DTOs;
using SelfServiceKioskSystem.Models;
using SelfServiceKioskSystem.Models.DTOs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SelfServiceKioskSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var products = await _context.Products
                .Include(p => p.Category)
                .Select(p => new ProductDTO
                {
                    Name = p.Name,
                    ImageURL = $"{baseUrl}/images/products/{p.ImageURL}",
                    CategoryName = p.Category.CategoryName,
                    Price = p.Price,
                    Quantity = p.Quantity,
                })
                .ToListAsync();

            return Ok(products);
        }

        [HttpPost]
        [Authorize(Roles = "Superuser")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var imageFileName = await SaveImageToFileSystem(dto.Image);
            if (string.IsNullOrEmpty(imageFileName))
                return BadRequest("Image upload failed.");

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                CategoryID = dto.CategoryID,
                Quantity = dto.Quantity,
                ImageURL = imageFileName
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Superuser")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] CreateProductDTO dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            var imageFileName = await SaveImageToFileSystem(dto.Image);
            if (string.IsNullOrEmpty(imageFileName))
                return BadRequest("Image upload failed.");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.CategoryID = dto.CategoryID;
            product.ImageURL = imageFileName;

            await _context.SaveChangesAsync();
            return Ok(product);
        }

       
        [Authorize(Roles = "Superuser")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Product not found.");

            // Optionally: Delete image from file system
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products", product.ImageURL);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Product deleted successfully." });
        }

        //  Place the helper method at the bottom
        private async Task<string> SaveImageToFileSystem(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            try
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return uniqueFileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving image: " + ex.Message);
                return null;
            }
        }
    }
}

