using Microsoft.AspNetCore.Mvc;

namespace TeamPlanner.Controllers
{
    public class UserNotFound : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
