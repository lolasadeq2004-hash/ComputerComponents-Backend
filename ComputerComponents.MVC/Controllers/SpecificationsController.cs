using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ComputerComponents.Application.Interfaces;
using ComputerComponents.Domain.Entities;

namespace ComputerComponents.MVC.Controllers
{
    public class SpecificationsController : Controller
    {
        private readonly ISpecificationRepository _specificationRepository;
        private readonly IComponentRepository _componentRepository;

        public SpecificationsController(
            ISpecificationRepository specificationRepository,
            IComponentRepository componentRepository)
        {
            _specificationRepository = specificationRepository;
            _componentRepository = componentRepository;
        }

        public async Task<IActionResult> Index()
        {
            var components = await _componentRepository.GetAllAsync();
            ViewBag.Components = new SelectList(components, "Id", "ComponentName");

            var specs = await _specificationRepository.GetSpecificationsWithComponentAsync();
            return View(specs);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Specification spec)
        {
            ModelState.Remove("Component");
            if (ModelState.IsValid)
            {
                await _specificationRepository.AddAsync(spec);
                TempData["Success"] = "تمت إضافة الخاصية بنجاح!";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Specification spec)
        {
            ModelState.Remove("Component");
            if (ModelState.IsValid)
            {
                await _specificationRepository.UpdateAsync(spec);
                TempData["Success"] = "تم تعديل الخاصية بنجاح!";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _specificationRepository.DeleteAsync(id);
            TempData["Success"] = "تم حذف الخاصية بنجاح!";
            return RedirectToAction(nameof(Index));
        }
    }
}
