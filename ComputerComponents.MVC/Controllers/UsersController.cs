using Microsoft.AspNetCore.Mvc;
using ComputerComponents.Application.Interfaces;
using ComputerComponents.Domain.Entities;

namespace ComputerComponents.MVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.CreatedAt = DateTime.Now;
                user.Role = user.Role ?? "مستعمل";
                user.Status = user.Status ?? "نشط";
                await _userRepository.AddAsync(user);
                TempData["Success"] = "تمت إضافة المستخدم بنجاح!";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userRepository.GetByIdAsync(user.Id);
                if (existingUser != null)
                {
                    existingUser.FullName = user.FullName;
                    existingUser.Email = user.Email;
                    existingUser.Role = user.Role;
                    existingUser.Status = user.Status;
                    if (!string.IsNullOrWhiteSpace(user.Password))
                    {
                        existingUser.Password = user.Password;
                    }
                    await _userRepository.UpdateAsync(existingUser);
                    TempData["Success"] = "تم تعديل المستخدم بنجاح!";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _userRepository.DeleteAsync(id);
            TempData["Success"] = "تم حذف المستخدم بنجاح!";
            return RedirectToAction(nameof(Index));
        }
    }
}
