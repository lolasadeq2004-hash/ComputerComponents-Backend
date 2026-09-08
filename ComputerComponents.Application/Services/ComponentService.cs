using ComputerComponents.Application.DTOs;
using ComputerComponents.Application.Interfaces;
using ComputerComponents.Domain.Entities;

namespace ComputerComponents.Application.Services
{
    public class ComponentService : IComponentService
    {
        private readonly IComponentRepository _componentRepository;
        private readonly ISpecificationRepository _specificationRepository;

        public ComponentService(
            IComponentRepository componentRepository,
            ISpecificationRepository specificationRepository)
        {
            _componentRepository = componentRepository;
            _specificationRepository = specificationRepository;
        }

        public async Task<IEnumerable<ComponentDto>> GetAllAsync(string? search = null, int? categoryId = null)
        {
            var components = await _componentRepository.GetComponentsWithCategoryAsync(search, categoryId);
            return components.Select(MapToDto);
        }

        public async Task<ComponentDto?> GetByIdAsync(int id)
        {
            var component = await _componentRepository.GetComponentWithDetailsByIdAsync(id);
            if (component == null) return null;

            return MapToDto(component);
        }

        public async Task<ComponentDto> CreateAsync(CreateComponentDto dto)
        {
            var component = new Component
            {
                ComponentName = dto.ComponentName,
                Model = dto.Model,
                Price = dto.Price,
                ImageUrl = string.IsNullOrWhiteSpace(dto.ImageUrl)
                    ? "https://images.unsplash.com/photo-1587202372775-e229f172b9d7?w=400"
                    : dto.ImageUrl,
                CategoryId = dto.CategoryId
            };

            if (dto.Specifications != null && dto.Specifications.Any())
            {
                component.Specifications = dto.Specifications.Select(s => new Specification
                {
                    PropertyName = s.PropertyName,
                    PropertyValue = s.PropertyValue
                }).ToList();
            }

            await _componentRepository.AddAsync(component);

            var created = await _componentRepository.GetComponentWithDetailsByIdAsync(component.Id);
            return MapToDto(created ?? component);
        }

        public async Task<bool> UpdateAsync(int id, UpdateComponentDto dto)
        {
            var component = await _componentRepository.GetComponentWithDetailsByIdAsync(id);
            if (component == null) return false;

            component.ComponentName = dto.ComponentName;
            component.Model = dto.Model;
            component.Price = dto.Price;
            if (!string.IsNullOrWhiteSpace(dto.ImageUrl))
            {
                component.ImageUrl = dto.ImageUrl;
            }
            component.CategoryId = dto.CategoryId;

            await _componentRepository.UpdateAsync(component);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var component = await _componentRepository.GetByIdAsync(id);
            if (component == null) return false;

            await _componentRepository.DeleteAsync(id);
            return true;
        }

        private static ComponentDto MapToDto(Component c)
        {
            return new ComponentDto
            {
                Id = c.Id,
                ComponentName = c.ComponentName,
                Model = c.Model,
                Price = c.Price,
                ImageUrl = c.ImageUrl,
                CategoryId = c.CategoryId,
                CategoryName = c.Category?.CategoryName,
                Specifications = c.Specifications?.Select(s => new SpecificationDto
                {
                    Id = s.Id,
                    PropertyName = s.PropertyName,
                    PropertyValue = s.PropertyValue,
                    ComponentId = s.ComponentId,
                    ComponentName = c.ComponentName
                }).ToList()
            };
        }
    }
}
