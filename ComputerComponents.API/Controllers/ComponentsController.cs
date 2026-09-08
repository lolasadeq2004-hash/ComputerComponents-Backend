using Microsoft.AspNetCore.Mvc;
using ComputerComponents.Application.DTOs;
using ComputerComponents.Application.Interfaces;

namespace ComputerComponents.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentsController : ControllerBase
    {
        private readonly IComponentService _componentService;

        public ComponentsController(IComponentService componentService)
        {
            _componentService = componentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search = null, [FromQuery] int? categoryId = null)
        {
            var components = await _componentService.GetAllAsync(search, categoryId);
            return Ok(components);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var component = await _componentService.GetByIdAsync(id);
            if (component == null) return NotFound();
            return Ok(component);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateComponentDto dto)
        {
            var created = await _componentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutComponent(int id, [FromBody] UpdateComponentDto dto)
        {
            dto.Id = id;
            var updated = await _componentService.UpdateAsync(id, dto);
            if (!updated)
            {
                return NotFound();
            }

            return Ok(new { message = "Component updated successfully!", id = id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComponent(int id)
        {
            var deleted = await _componentService.DeleteAsync(id);
            if (!deleted)
            {
                return Ok(new { message = "Item does not exist or was already deleted" });
            }

            return Ok(new { message = "Deleted successfully!", id = id });
        }
    }
}
