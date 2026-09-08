using ComputerComponents.Application.DTOs;
using ComputerComponents.Application.Interfaces;

namespace ComputerComponents.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IComponentRepository _componentRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISpecificationRepository _specificationRepository;

        public DashboardService(
            IComponentRepository componentRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            ISpecificationRepository specificationRepository)
        {
            _componentRepository = componentRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _specificationRepository = specificationRepository;
        }

        public async Task<DashboardStatsDto> GetStatsAsync()
        {
            var totalComponents = await _componentRepository.GetCountAsync();
            var totalCategories = await _categoryRepository.GetCountAsync();
            var totalUsers = await _userRepository.GetCountAsync();
            var totalSpecs = await _specificationRepository.GetCountAsync();
            var totalInventoryValue = await _componentRepository.GetTotalInventoryValueAsync();

            var recentComponents = await _componentRepository.GetRecentComponentsAsync(5);
            var categories = await _categoryRepository.GetCategoriesWithComponentsAsync();

            return new DashboardStatsDto
            {
                TotalComponents = totalComponents,
                TotalCategories = totalCategories,
                TotalUsers = totalUsers,
                TotalSpecifications = totalSpecs,
                TotalInventoryValue = totalInventoryValue,
                RecentComponents = recentComponents.Select(c => new ComponentDto
                {
                    Id = c.Id,
                    ComponentName = c.ComponentName,
                    Model = c.Model,
                    Price = c.Price,
                    ImageUrl = c.ImageUrl,
                    CategoryId = c.CategoryId,
                    CategoryName = c.Category?.CategoryName
                }).ToList(),
                CategoriesSummary = categories.Select(c => new CategorySummaryDto
                {
                    Name = c.CategoryName,
                    Count = c.Components?.Count ?? 0
                }).ToList()
            };
        }
    }
}
