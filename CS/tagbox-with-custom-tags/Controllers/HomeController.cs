using Microsoft.AspNetCore.Mvc;

namespace TagBoxWithCustomTags.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(DataContext.GetEmployees(HttpContext));
        }
    }
}
