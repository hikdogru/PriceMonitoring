using Microsoft.AspNetCore.Mvc;
using PriceMonitoring.WebUI.Models;

namespace PriceMonitoring.WebUI.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserModel model)
        {
            return View();
        }
    }
}
