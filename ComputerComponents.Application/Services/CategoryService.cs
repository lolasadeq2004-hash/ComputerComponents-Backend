using ComputerComponents.Application.DTOs;
using ComputerComponents.Application.Interfaces;
using ComputerComponents.Domain.Entities;

namespace ComputerComponents.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetCategoriesWithComponentsAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                Description = c.Description,
                Components = c.Components?.Select(comp => new ComponentDto
                {
                    Id = comp.Id,
                    ComponentName = comp.ComponentName,
                    Model = comp.Model,
                    Price = comp.Price,
                    ImageUrl = comp.ImageUrl,
                    CategoryId = comp.CategoryId
                }).ToList()
            });
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryWithComponentsByIdAsync(id);
            if (category == null) return null;

            return new CategoryDto
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                Description = category.Description,
                Components = category.Components?.Select(comp => new ComponentDto
                {
                    Id = comp.Id,
                    ComponentName = comp.ComponentName,
                    Model = comp.Model,
                    Price = comp.Price,
                    ImageUrl = comp.ImageUrl,
                    CategoryId = comp.CategoryId
                }).ToList()
            };
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            var category = new Category
            {
                CategoryName = dto.CategoryName,
                Description = dto.Description
            };

            await _categoryRepository.AddAsync(category);

            return new CategoryDto
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                Description = category.Description
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateCategoryDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return false;

            category.CategoryName = dto.CategoryName;
            category.Description = dto.Description;

            await _categoryRepository.UpdateAsync(category);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return false;

            await _categoryRepository.DeleteAsync(id);
            return true;
        }
    }
}
