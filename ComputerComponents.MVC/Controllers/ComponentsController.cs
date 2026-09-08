using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ComputerComponents.Application.Interfaces;
using ComputerComponents.Domain.Entities;

namespace ComputerComponents.MVC.Controllers
{
    public class ComponentsController : Controller
    {
        private readonly IComponentRepository _componentRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ComponentsController(IComponentRepository componentRepository, ICategoryRepository categoryRepository)
        {
            _componentRepository = componentRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index(string? search, int? categoryId)
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.SearchQuery = search;

            var components = await _componentRepository.GetComponentsWithCategoryAsync(search, categoryId);
            return View(components);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Component component)
        {
            ModelState.Remove("Category");
            ModelState.Remove("Specifications");
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(component.ImageUrl))
                {
                    component.ImageUrl = "https://images.unsplash.com/photo-1587202372775-e229f172b9d7?w=400";
                }
                await _componentRepository.AddAsync(component);
                TempData["Success"] = "تمت إضافة المكوّن بنجاح!";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Component component)
        {
            ModelState.Remove("Category");
            ModelState.Remove("Specifications");
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(component.ImageUrl))
                {
                    component.ImageUrl = "https://images.unsplash.com/photo-1587202372775-e229f172b9d7?w=400";
                }
                await _componentRepository.UpdateAsync(component);
                TempData["Success"] = "تم تعديل المكوّن بنجاح!";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _componentRepository.DeleteAsync(id);
            TempData["Success"] = "تم حذف المكوّن بنجاح!";
            return RedirectToAction(nameof(Index));
        }
    }
}
