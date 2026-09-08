using ComputerComponents.Domain.Entities;

namespace ComputerComponents.Application.Interfaces
{
    public interface ISpecificationRepository : IRepository<Specification>
    {
        Task<IEnumerable<Specification>> GetSpecificationsWithComponentAsync();
        Task<Specification?> GetSpecificationWithComponentByIdAsync(int id);
        Task<IEnumerable<Specification>> GetByComponentIdAsync(int componentId);
        Task<int> GetCountAsync();
    }
}
