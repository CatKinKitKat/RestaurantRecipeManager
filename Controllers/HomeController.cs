using Microsoft.AspNetCore.Mvc;

namespace RestaurantRecipeManager.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}