using Microsoft.AspNetCore.Mvc;

namespace ComputerComponents.MVC.Controllers
{
    public class MobileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
