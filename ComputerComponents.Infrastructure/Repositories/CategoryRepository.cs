using Microsoft.EntityFrameworkCore;
using ComputerComponents.Application.Interfaces;
using ComputerComponents.Domain.Entities;
using ComputerComponents.Infrastructure.Data;

namespace ComputerComponents.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Category>> GetCategoriesWithComponentsAsync()
        {
            return await _context.Categories
                .Include(c => c.Components)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryWithComponentsByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Components)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Categories.CountAsync();
        }
    }
}
