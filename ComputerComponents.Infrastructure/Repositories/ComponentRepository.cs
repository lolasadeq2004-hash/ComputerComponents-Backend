using Microsoft.EntityFrameworkCore;
using ComputerComponents.Application.Interfaces;
using ComputerComponents.Domain.Entities;
using ComputerComponents.Infrastructure.Data;

namespace ComputerComponents.Infrastructure.Repositories
{
    public class ComponentRepository : Repository<Component>, IComponentRepository
    {
        public ComponentRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Component>> GetComponentsWithCategoryAsync(string? search = null, int? categoryId = null)
        {
            var query = _context.Components
                .Include(c => c.Category)
                .Include(c => c.Specifications)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.ComponentName.Contains(search) || c.Model.Contains(search));
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(c => c.CategoryId == categoryId.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Component?> GetComponentWithDetailsByIdAsync(int id)
        {
            return await _context.Components
                .Include(c => c.Category)
                .Include(c => c.Specifications)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public override async Task DeleteAsync(int id)
        {
            var component = await _context.Components
                .Include(c => c.Specifications)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (component != null)
            {
                if (component.Specifications != null && component.Specifications.Any())
                {
                    _context.Specifications.RemoveRange(component.Specifications);
                }
                _context.Components.Remove(component);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Components.CountAsync();
        }

        public async Task<IEnumerable<Component>> GetRecentComponentsAsync(int count)
        {
            return await _context.Components
                .Include(c => c.Category)
                .OrderByDescending(c => c.Id)
                .Take(count)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalInventoryValueAsync()
        {
            return await _context.Components.SumAsync(c => (decimal?)c.Price) ?? 0;
        }
    }
}
