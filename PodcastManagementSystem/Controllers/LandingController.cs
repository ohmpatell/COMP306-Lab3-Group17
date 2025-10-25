using Microsoft.AspNetCore.Mvc;

namespace PodcastManagementSystem.Controllers
{
    public class LandingController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}