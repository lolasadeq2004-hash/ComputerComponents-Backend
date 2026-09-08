using ComputerComponents.Application.DTOs;

namespace ComputerComponents.Application.Interfaces
{
    public interface ISpecificationService
    {
        Task<IEnumerable<SpecificationDto>> GetAllAsync(int? componentId = null);
        Task<SpecificationDto?> GetByIdAsync(int id);
        Task<SpecificationDto> CreateAsync(CreateSpecificationDto dto);
        Task<bool> UpdateAsync(int id, UpdateSpecificationDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
