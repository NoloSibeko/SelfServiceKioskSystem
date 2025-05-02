using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfServiceKioskSystem.DTOs;
using SelfServiceKioskSystem.Models;
using SelfServiceKioskSystem.Data;
using SelfServiceKioskSystem.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "Superuser")]
        public async Task<IActionResult> CreateCategory(CreateCategoryDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = new Category
            {
                CategoryName = dto.Name,
               
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }

        [HttpGet("{id}")]
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
                Products = category.Products
                    .Where(p => p.isAvailable)
                    .Select(p => new ProductDTO
                    {
                        ProductID = p.ProductID,
                        Name = p.Name,
                        ImageURL = p.ImageURL,
                        CategoryName = category.CategoryName,
                        Price = p.Price,
                        IsAvailable = p.isAvailable,
                        Description = p.Description,
                        Quantity = p.Quantity
                    })
                    .ToList()
            };


            return Ok(response);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Superuser")]
        public async Task<IActionResult> UpdateCategory(int id, CreateCategoryDTO dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            category.CategoryName = dto.Name;
           

            await _context.SaveChangesAsync();

            return Ok(category);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Superuser")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _context.Categories
                .Select(c => new
                {
                    c.CategoryID,
                    c.CategoryName
                })
                .ToListAsync();

            return Ok(categories);
        }

    }
}
