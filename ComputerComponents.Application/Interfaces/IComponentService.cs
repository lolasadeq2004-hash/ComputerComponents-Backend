using ComputerComponents.Application.DTOs;

namespace ComputerComponents.Application.Interfaces
{
    public interface IComponentService
    {
        Task<IEnumerable<ComponentDto>> GetAllAsync(string? search = null, int? categoryId = null);
        Task<ComponentDto?> GetByIdAsync(int id);
        Task<ComponentDto> CreateAsync(CreateComponentDto dto);
        Task<bool> UpdateAsync(int id, UpdateComponentDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
