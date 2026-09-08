using Microsoft.AspNetCore.Mvc;
using ComputerComponents.Application.DTOs;
using ComputerComponents.Application.Interfaces;

namespace ComputerComponents.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecificationsController : ControllerBase
    {
        private readonly ISpecificationService _specificationService;

        public SpecificationsController(ISpecificationService specificationService)
        {
            _specificationService = specificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? componentId = null)
        {
            var specs = await _specificationService.GetAllAsync(componentId);
            return Ok(specs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var specification = await _specificationService.GetByIdAsync(id);
            if (specification == null) return NotFound();
            return Ok(specification);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSpecificationDto dto)
        {
            var created = await _specificationService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpecification(int id, [FromBody] UpdateSpecificationDto dto)
        {
            dto.Id = id;
            var updated = await _specificationService.UpdateAsync(id, dto);
            if (!updated)
            {
                return NotFound();
            }

            return Ok(new { message = "Specification updated successfully!", id = id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpecification(int id)
        {
            var deleted = await _specificationService.DeleteAsync(id);
            if (!deleted)
            {
                return Ok(new { message = "Specification does not exist or was already deleted" });
            }

            return Ok(new { message = "Specification deleted successfully!", id = id });
        }
    }
}
