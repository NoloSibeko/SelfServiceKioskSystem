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
        private readonly ProductDTO _product;
        private readonly CategoryProductsDisplayDTO _category;

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
                    ProductID=p.ProductID,
                    Name = p.Name,
                    ImageURL = $"{baseUrl}/images/products/{p.ImageURL}",
                    CategoryName = p.Category.CategoryName,
                    Price = p.Price,
                    Quantity = p.Quantity,
                })
                .ToListAsync();

            return Ok(products);
        }


        [HttpGet("byCategory")]
        public async Task<IActionResult> GetProductsByCategory([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Category name is required.");
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Category.CategoryName.ToLower() == name.ToLower())
                .ToListAsync();

            var productDtos = products.Select(p => new ProductDTO
            {
                Name = p.Name,
                ImageURL = $"{baseUrl}/images/products/{p.ImageURL}",
                CategoryName = p.Category.CategoryName,
                Price = p.Price,
                Quantity = p.Quantity,
            }).ToList();

            if (!productDtos.Any())
            {
                return NotFound($"No products found under category '{name}'.");
            }

            return Ok(productDtos);
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
                isAvailable = true,
                ImageURL = imageFileName
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Superuser")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] UpdateProductDTO dto)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == id);
            if (product == null)
                return NotFound("Product not found.");

            // Update image only if provided
            if (dto.Image != null)
            {
                var imageFileName = await SaveImageToFileSystem(dto.Image);
                if (string.IsNullOrEmpty(imageFileName))
                    return BadRequest("Image upload failed.");

                product.ImageURL = imageFileName;
            }

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.CategoryID = dto.CategoryID;
            product.Quantity = dto.Quantity;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Product updated successfully.", updatedProduct = product });
        }


        [HttpPut("mark-available/{id}")]
        [Authorize(Roles = "Superuser")]
        public async Task<IActionResult> PutMarkAvailable(int id, [FromBody] bool isAvailable)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == id);
            if (product == null)
                return NotFound("Product not found.");

            product.isAvailable = isAvailable; // Use the passed-in value

            await _context.SaveChangesAsync();
            return Ok($"Product availability updated to {(isAvailable ? "available" : "unavailable")}.");
        }




        [HttpDelete("{id}")]
        [Authorize(Roles = "Superuser")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == id);
            if (product == null)
                return NotFound("Product not found.");

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products", product.ImageURL);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Product deleted successfully." });
        }




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

