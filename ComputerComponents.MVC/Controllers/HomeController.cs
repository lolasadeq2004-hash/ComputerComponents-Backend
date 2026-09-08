using Microsoft.AspNetCore.Mvc;
using ComputerComponents.Application.Interfaces;

namespace ComputerComponents.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IComponentRepository _componentRepository;
        private readonly ISpecificationRepository _specificationRepository;
        private readonly IUserRepository _userRepository;

        public HomeController(
            ICategoryRepository categoryRepository,
            IComponentRepository componentRepository,
            ISpecificationRepository specificationRepository,
            IUserRepository userRepository)
        {
            _categoryRepository = categoryRepository;
            _componentRepository = componentRepository;
            _specificationRepository = specificationRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var components = await _componentRepository.GetAllAsync();
            var specs = await _specificationRepository.GetAllAsync();
            var users = await _userRepository.GetAllAsync();

            ViewBag.CategoriesCount = categories.Count();
            ViewBag.ComponentsCount = components.Count();
            ViewBag.SpecificationsCount = specs.Count();
            ViewBag.UsersCount = users.Count();
            
            ViewBag.TotalInventoryValue = await _componentRepository.GetTotalInventoryValueAsync();

            var recentComponents = await _componentRepository.GetRecentComponentsAsync(5);

            var categoriesWithComponents = await _categoryRepository.GetCategoriesWithComponentsAsync();
            var categoriesSummary = categoriesWithComponents
                .Select(c => new
                {
                    Name = c.CategoryName,
                    Count = c.Components != null ? c.Components.Count : 0
                })
                .ToList();

            ViewBag.CategoriesSummary = categoriesSummary;

            return View(recentComponents);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
