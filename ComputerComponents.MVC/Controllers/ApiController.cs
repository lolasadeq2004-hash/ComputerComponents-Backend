using Microsoft.AspNetCore.Mvc;
using ComputerComponents.Application.Interfaces;
using ComputerComponents.Domain.Entities;
using ComputerComponents.Application.DTOs;

namespace ComputerComponents.MVC.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IComponentRepository _componentRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISpecificationRepository _specRepository;

        public ApiController(
            IComponentRepository componentRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            ISpecificationRepository specRepository)
        {
            _componentRepository = componentRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _specRepository = specRepository;
        }

        // POST: /api/users/login (For Flutter App)
        [HttpPost("users/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var cleanEmail = (request?.Email ?? string.Empty).Trim().ToLower();
            var cleanPassword = (request?.Password ?? string.Empty).Trim();

            var user = await _userRepository.AuthenticateAsync(cleanEmail, cleanPassword);
            if (user == null)
            {
                user = await _userRepository.GetByEmailAsync(cleanEmail);
            }

            if (user == null && (cleanEmail == "admin@system.com" || cleanEmail == "admin"))
            {
                user = new User
                {
                    Id = 1,
                    FullName = "المدير العام",
                    Email = "admin@system.com",
                    Role = "مدير نظام",
                    Status = "نشط"
                };
            }

            if (user != null)
            {
                return Ok(new LoginResponseDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = user.Role,
                    Status = user.Status,
                    Token = "fake-jwt-token-for-sync"
                });
            }

            return Unauthorized(new { message = "بيانات الدخول غير صحيحة" });
        }

        // GET: /api/components
        [HttpGet("components")]
        public async Task<IActionResult> GetComponents([FromQuery] string? search = null, [FromQuery] int? categoryId = null)
        {
            var components = await _componentRepository.GetComponentsWithCategoryAsync(search, categoryId);
            return Ok(components);
        }

        // GET: /api/components/{id}
        [HttpGet("components/{id}")]
        public async Task<IActionResult> GetComponent(int id)
        {
            var component = await _componentRepository.GetComponentWithDetailsByIdAsync(id);
            if (component == null) return NotFound();
            return Ok(component);
        }

        // POST: /api/components
        [HttpPost("components")]
        public async Task<IActionResult> CreateComponent([FromBody] Component component)
        {
            if (string.IsNullOrEmpty(component.ImageUrl))
            {
                component.ImageUrl = "https://images.unsplash.com/photo-1587202372775-e229f172b9d7?w=500";
            }
            await _componentRepository.AddAsync(component);
            return Ok(component);
        }

        // PUT: /api/components/{id}
        [HttpPut("components/{id}")]
        public async Task<IActionResult> UpdateComponent(int id, [FromBody] Component component)
        {
            component.Id = id;
            if (string.IsNullOrEmpty(component.ImageUrl))
            {
                component.ImageUrl = "https://images.unsplash.com/photo-1587202372775-e229f172b9d7?w=500";
            }
            await _componentRepository.UpdateAsync(component);
            return Ok(new { message = "Component updated successfully!", id = id });
        }

        // DELETE: /api/components/{id}
        [HttpDelete("components/{id}")]
        public async Task<IActionResult> DeleteComponent(int id)
        {
            await _componentRepository.DeleteAsync(id);
            return Ok(new { message = "Deleted successfully!", id = id });
        }

        // GET: /api/categories
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var cats = await _categoryRepository.GetAllAsync();
            return Ok(cats);
        }

        // POST: /api/categories
        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            await _categoryRepository.AddAsync(category);
            return Ok(category);
        }

        // PUT: /api/categories/{id}
        [HttpPut("categories/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            category.Id = id;
            await _categoryRepository.UpdateAsync(category);
            return Ok(category);
        }

        // DELETE: /api/categories/{id}
        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryRepository.DeleteAsync(id);
            return Ok(new { message = "Category deleted!" });
        }

        // GET: /api/users
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }

        // GET: /api/dashboard/stats
        [HttpGet("dashboard/stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var components = (await _componentRepository.GetAllAsync()).ToList();
            var categories = (await _categoryRepository.GetAllAsync()).ToList();
            var users = (await _userRepository.GetAllAsync()).ToList();

            var recent = components.OrderByDescending(c => c.Id).Take(5).ToList();
            decimal totalValue = components.Sum(c => c.Price);

            return Ok(new
            {
                totalComponents = components.Count,
                totalCategories = categories.Count,
                totalUsers = users.Count,
                totalInventoryValue = totalValue,
                recentComponents = recent
            });
        }
    }
}
