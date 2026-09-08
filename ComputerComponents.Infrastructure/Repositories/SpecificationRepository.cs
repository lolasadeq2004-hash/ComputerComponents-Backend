using Microsoft.EntityFrameworkCore;
using ComputerComponents.Application.Interfaces;
using ComputerComponents.Domain.Entities;
using ComputerComponents.Infrastructure.Data;

namespace ComputerComponents.Infrastructure.Repositories
{
    public class SpecificationRepository : Repository<Specification>, ISpecificationRepository
    {
        public SpecificationRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Specification>> GetSpecificationsWithComponentAsync()
        {
            return await _context.Specifications
                .Include(s => s.Component)
                .ToListAsync();
        }

        public async Task<Specification?> GetSpecificationWithComponentByIdAsync(int id)
        {
            return await _context.Specifications
                .Include(s => s.Component)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Specification>> GetByComponentIdAsync(int componentId)
        {
            return await _context.Specifications
                .Include(s => s.Component)
                .Where(s => s.ComponentId == componentId)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Specifications.CountAsync();
        }
    }
}
