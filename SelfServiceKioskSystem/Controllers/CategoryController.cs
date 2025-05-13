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
                CategoryName = dto.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var response = new CreateCategoryDTO
            {
                //CategoryID = category.CategoryID,
                Name = category.CategoryName
            };

            return Ok(response);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryProductsDisplayDTO>> GetCategoryWithProducts(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryID == id);

            if (category == null) return NotFound();

            var dto = new CategoryProductsDisplayDTO
            {
                CategoryID = category.CategoryID,
                CategoryName = category.CategoryName,
                Products = category.Products.Select(p => new ProductDTO
                {
                    ProductID = p.ProductID,
                    Name = p.Name,
                    ImageURL = p.ImageURL,
                    Price = p.Price,
                    IsAvailable = true,
                    Description = p.Description,
                    Quantity = p.Quantity
                }).ToList()
            };

            return Ok(dto);
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

            var response = new UpdateCategoryDTO
            {
                CategoryID = category.CategoryID,
                CategoryName = category.CategoryName
            };

            return Ok(response);
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
