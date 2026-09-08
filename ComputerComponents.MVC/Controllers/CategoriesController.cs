using Microsoft.AspNetCore.Mvc;
using ComputerComponents.Application.Interfaces;
using ComputerComponents.Domain.Entities;

namespace ComputerComponents.MVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetCategoriesWithComponentsAsync();
            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            ModelState.Remove("Components");
            if (ModelState.IsValid)
            {
                await _categoryRepository.AddAsync(category);
                TempData["Success"] = "تمت إضافة الفئة بنجاح!";
            }
            else
            {
                TempData["Error"] = "يرجى التحقق من البيانات المدخلة!";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            ModelState.Remove("Components");
            if (ModelState.IsValid)
            {
                await _categoryRepository.UpdateAsync(category);
                TempData["Success"] = "تم تعديل الفئة بنجاح!";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryRepository.DeleteAsync(id);
            TempData["Success"] = "تم حذف الفئة بنجاح!";
            return RedirectToAction(nameof(Index));
        }
    }
}
