using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfServiceKioskSystem.DTOs;
using SelfServiceKioskSystem.Models;
using SelfServiceKioskSystem.Data;

namespace SelfServiceKioskSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = new Category
            {
                CategoryName = dto.Name,
                Description = dto.Description
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }

       /* [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryID == id);

            if (category == null)
                return NotFound("Category not found.");

            var response = new CategoryProductsDisplayDTO
            {
                CategoryID = category.CategoryID,
                CategoryName = category.CategoryName,
                Name = category.Products
                    .Where(p => p.isAvailable) // Filter only active products
                    .Select(p => new CategoryProductsDisplayDTO
                    {
                        ProductID = p.ProductID,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        ImageURL = p.ImageURL,
                        Quantity = p.Quantity
                    })
                    .ToList()
            };

            return Ok(response);
        }*/


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CreateCategoryDTO dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            category.CategoryName = dto.Name;
            category.Description = dto.Description;

            await _context.SaveChangesAsync();

            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
