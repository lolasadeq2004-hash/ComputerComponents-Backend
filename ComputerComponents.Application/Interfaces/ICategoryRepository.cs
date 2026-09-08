using ComputerComponents.Domain.Entities;

namespace ComputerComponents.Application.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategoriesWithComponentsAsync();
        Task<Category?> GetCategoryWithComponentsByIdAsync(int id);
        Task<int> GetCountAsync();
    }
}
