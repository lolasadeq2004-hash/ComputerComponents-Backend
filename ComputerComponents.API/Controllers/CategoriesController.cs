using Microsoft.AspNetCore.Mvc;
using ComputerComponents.Application.DTOs;
using ComputerComponents.Application.Interfaces;

namespace ComputerComponents.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            var created = await _categoryService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, [FromBody] UpdateCategoryDto dto)
        {
            dto.Id = id;
            var updated = await _categoryService.UpdateAsync(id, dto);
            if (!updated)
            {
                return NotFound();
            }

            return Ok(new { message = "Category updated successfully!", id = id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var deleted = await _categoryService.DeleteAsync(id);
            if (!deleted)
            {
                return Ok(new { message = "Category does not exist or was already deleted" });
            }

            return Ok(new { message = "Category deleted successfully!", id = id });
        }
    }
}
