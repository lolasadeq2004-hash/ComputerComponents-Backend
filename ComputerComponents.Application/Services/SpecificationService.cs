using ComputerComponents.Application.DTOs;
using ComputerComponents.Application.Interfaces;
using ComputerComponents.Domain.Entities;

namespace ComputerComponents.Application.Services
{
    public class SpecificationService : ISpecificationService
    {
        private readonly ISpecificationRepository _specificationRepository;

        public SpecificationService(ISpecificationRepository specificationRepository)
        {
            _specificationRepository = specificationRepository;
        }

        public async Task<IEnumerable<SpecificationDto>> GetAllAsync(int? componentId = null)
        {
            var specs = componentId.HasValue
                ? await _specificationRepository.GetByComponentIdAsync(componentId.Value)
                : await _specificationRepository.GetSpecificationsWithComponentAsync();

            return specs.Select(s => new SpecificationDto
            {
                Id = s.Id,
                PropertyName = s.PropertyName,
                PropertyValue = s.PropertyValue,
                ComponentId = s.ComponentId,
                ComponentName = s.Component?.ComponentName
            });
        }

        public async Task<SpecificationDto?> GetByIdAsync(int id)
        {
            var spec = await _specificationRepository.GetSpecificationWithComponentByIdAsync(id);
            if (spec == null) return null;

            return new SpecificationDto
            {
                Id = spec.Id,
                PropertyName = spec.PropertyName,
                PropertyValue = spec.PropertyValue,
                ComponentId = spec.ComponentId,
                ComponentName = spec.Component?.ComponentName
            };
        }

        public async Task<SpecificationDto> CreateAsync(CreateSpecificationDto dto)
        {
            var spec = new Specification
            {
                PropertyName = dto.PropertyName,
                PropertyValue = dto.PropertyValue,
                ComponentId = dto.ComponentId
            };

            await _specificationRepository.AddAsync(spec);

            var created = await _specificationRepository.GetSpecificationWithComponentByIdAsync(spec.Id);
            return new SpecificationDto
            {
                Id = spec.Id,
                PropertyName = spec.PropertyName,
                PropertyValue = spec.PropertyValue,
                ComponentId = spec.ComponentId,
                ComponentName = created?.Component?.ComponentName
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateSpecificationDto dto)
        {
            var spec = await _specificationRepository.GetByIdAsync(id);
            if (spec == null) return false;

            spec.PropertyName = dto.PropertyName;
            spec.PropertyValue = dto.PropertyValue;
            spec.ComponentId = dto.ComponentId;

            await _specificationRepository.UpdateAsync(spec);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var spec = await _specificationRepository.GetByIdAsync(id);
            if (spec == null) return false;

            await _specificationRepository.DeleteAsync(id);
            return true;
        }
    }
}
