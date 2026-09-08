using ComputerComponents.Domain.Entities;

namespace ComputerComponents.Application.Interfaces
{
    public interface IComponentRepository : IRepository<Component>
    {
        Task<IEnumerable<Component>> GetComponentsWithCategoryAsync(string? search = null, int? categoryId = null);
        Task<Component?> GetComponentWithDetailsByIdAsync(int id);
        Task<IEnumerable<Component>> GetRecentComponentsAsync(int count);
        Task<decimal> GetTotalInventoryValueAsync();
        Task<int> GetCountAsync();
    }
}
